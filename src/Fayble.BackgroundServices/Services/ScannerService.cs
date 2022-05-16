using System.Globalization;
using Fayble.Core.Helpers;
using Fayble.Core.Hubs;
using Fayble.Domain;
using Fayble.Domain.Aggregates;
using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Domain.Aggregates.Book;
using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Aggregates.Person;
using Fayble.Domain.Aggregates.Series;
using Fayble.Domain.Enums;
using Fayble.Domain.Repositories;
using Fayble.Models.FileSystem;
using Fayble.Services.FileSystem;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Fayble.BackgroundServices.Services;

public class ScannerService
{
    private readonly IBookRepository _bookRepository;
    private readonly IComicBookFileSystemService _comicBookFileSystemService;
    private readonly ILibraryRepository _libraryRepository;
    private readonly IBackgroundTaskRepository _backgroundTaskRepository;
    private readonly IPersonRepository _personRepository;
    private readonly ILogger _logger;
    private readonly ISeriesRepository _seriesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<BackgroundTaskHub> _hubContext;
    private BackgroundTask _backgroundTask;

    public ScannerService(
        ILogger<ScannerService> logger,
        IUnitOfWork unitOfWork,
        IBookRepository bookRepository,
        ILibraryRepository libraryRepository,
        IComicBookFileSystemService comicBookFileSystemService,
        ISeriesRepository seriesRepository,
        IHubContext<BackgroundTaskHub> hubContext,
        IBackgroundTaskRepository backgroundTaskRepository,
        IPersonRepository personRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _bookRepository = bookRepository;
        _libraryRepository = libraryRepository;
        _comicBookFileSystemService = comicBookFileSystemService;
        _seriesRepository = seriesRepository;
        _hubContext = hubContext;
        _backgroundTaskRepository = backgroundTaskRepository;
        _personRepository = personRepository;
    }

    public async Task SeriesScan(Guid seriesId, Guid taskId)
    {
        try
        {
            var series = await _seriesRepository.Get(seriesId);
            _backgroundTask = new BackgroundTask(
                taskId,
                seriesId,
                series.Name,
                BackgroundTaskType.SeriesScan.ToString(),
                BackgroundTaskStatus.Queued.ToString());

            await UpdateTaskStatus(BackgroundTaskStatus.Running);
            await SendClientUpdate("Scanning", BackgroundTaskStatus.Running, "BackgroundTaskStarted");
            _logger.LogInformation("Scanning series: {SeriesId}", seriesId);
            await UpdateTaskStatus(BackgroundTaskStatus.Complete);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while scanning series: {SeriesId}", seriesId);
            await UpdateTaskStatus(BackgroundTaskStatus.Failed);
        }
        finally
        {
            await SendClientUpdate("Complete", BackgroundTaskStatus.Complete, "BackgroundTaskCompleted");
        }
    }

    public async Task LibraryScan(Guid libraryId, Guid taskId)
    {
        try
        {
            var library = await _libraryRepository.Get(libraryId);
            _backgroundTask = new BackgroundTask(
                taskId,
                libraryId,
                library.Name,
                BackgroundTaskType.LibraryScan.ToString(),
                BackgroundTaskStatus.Queued.ToString());

            await UpdateTaskStatus(BackgroundTaskStatus.Running);
            await SendClientUpdate("Scanning", BackgroundTaskStatus.Running, "BackgroundTaskStarted");
            _logger.LogInformation("Scanning library for new books: {LibraryId}", libraryId);
            await ScanLibrary(library);
            await UpdateTaskStatus(BackgroundTaskStatus.Complete);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while scanning library: {LibraryId}", libraryId);
            await UpdateTaskStatus(BackgroundTaskStatus.Failed);
        }
        finally
        {
            await SendClientUpdate("Complete", BackgroundTaskStatus.Complete, "BackgroundTaskCompleted");
        }
    }


    private async Task ScanLibrary(Library library)
    {
        _logger.LogDebug("Retrieving new files from library paths");

        var seriesDirectories = await _comicBookFileSystemService.GetSeriesDirectories(library.FolderPath);
        foreach (var seriesDirectory in seriesDirectories)
        {
            var series = await GetSeries(seriesDirectory, library);
            await ScanSeries(series);
        }
    }

    private async Task ScanSeries(Series series)
    {
        await SendClientUpdate($"Scanning {series.FolderName}");

        await ScanExistingBooks(series);
        await ScanNewBooks(series);
    }

    private async Task ScanExistingBooks(Series series)
    {
        _logger.LogInformation("Scanning existing books for series: {Series}", series.Name);

        if (series.Books == null || !series.Books.Any())
        {
            _logger.LogDebug("No existing books");
            return;
        }

        foreach (var book in series.Books)
        {
            var file = new FileInfo(Path.Combine(book.Library.FolderPath, book.File.FilePath));
            if (!file.Exists)
            {
                if (book.DeletedDate == null)
                {
                    _logger.LogInformation("File no longer exists, flagging as deleted: {File}", file.FullName);
                    book.Delete();
                }

                continue;
            }

            if (file.LastWriteTimeUtc != book.File.FileLastModifiedDate)
            {
                if (book.DeletedDate != null)
                {
                    _logger.LogInformation("File previously flagged as deleted but is now present, restoring: {File}", file.FullName);
                    book.Restore();
                }

                _logger.LogInformation("File modified date changed, updating: {File}", file.FullName);

                book.File.Update(
                    file.Length,
                    _comicBookFileSystemService.GetHash(file.FullName),
                    _comicBookFileSystemService.GetPageCount(file.FullName));

                if (Convert.ToBoolean(series.Library.GetSetting(LibrarySettingKey.UseComicInfo)))
                {
                    await UpdateFromComicInfo(book);
                }

                continue;
            }

            _logger.LogDebug("File not modified: {File}", file.FullName);
        }

        _logger.LogInformation("Scanning existing books for series: {Series}", series.Name);
        await _unitOfWork.Commit();
    }

    private async Task ScanNewBooks(Series series)
    {
        _logger.LogInformation("Scanning new books for series: {Series}", series.Name);
        var newFiles = await GetNewFiles(series);

        foreach (var newFile in newFiles)
        {
            _logger.LogDebug("Processing issue: {FilePath}", newFile.FilePath);

            var bookFile = new BookFile(
                Guid.NewGuid(),
                newFile.FileName,
                PathHelpers.GetRelativePath(newFile.FilePath, series.Library.FolderPath),
                newFile.FileSize,
                newFile.FileType,
                newFile.FileLastModifiedDate,
                newFile.PageCount,
                _comicBookFileSystemService.GetHash(newFile.FilePath));

            var comicIssue = new Book(
                Guid.NewGuid(),
                series.Library.Id,
                MediaType.ComicBook,
                newFile.Number,
                bookFile,
                series.Id);

            comicIssue.SetMediaRoot(ApplicationHelpers.GetMediaDirectoryRoot(comicIssue.Id));

            // TODO: if settings allow parsing ComicInfoXml
            if (newFile.ComicInfoXml != null)
                comicIssue.Update(
                    newFile.ComicInfoXml.Title,
                    newFile.ComicInfoXml?.Number ?? newFile.Number,
                    newFile.ComicInfoXml?.Summary,
                    0,
                    null,
                    null,
                    DateOnly.TryParseExact(
                        $"{newFile.ComicInfoXml?.Year}-{newFile.ComicInfoXml?.Month}-{newFile.ComicInfoXml?.Day}",
                        "yyyy-M-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var coverDate)
                        ? coverDate
                        : null,
                    null,
                    null
                );

            try
            {
                _logger.LogDebug("Extracting cover image from: {FilePath}", newFile.FilePath);
                _comicBookFileSystemService.ExtractComicCoverImage(newFile.FilePath!, comicIssue.MediaRoot, comicIssue.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while extracting cover image");
            }

            comicIssue.UpdateSeries(series.Id);

            _bookRepository.Add(comicIssue);

            _logger.LogDebug(
                "New issue added to library: {@ComicIssue}",
                new
                {
                    comicIssue.Id,
                    comicIssue.File.FileType,
                    comicIssue.File.FilePath,
                    comicIssue.LibraryId
                });
        }

        await _unitOfWork.Commit();
    }

    private async Task<List<ComicFile>> GetNewFiles(Series series)
    {
        var newFiles = new List<ComicFile>();

        var filePaths = await _comicBookFileSystemService.GetFiles(
            Path.Combine(series.Library.FolderPath, series.FolderPath),
            MediaType.ComicBook);

        foreach (var filePath in filePaths)
        {
            _logger.LogDebug("Scanning directory for new files: {Directory}", filePath);
            var exists = series.Books?.Any(
                b => b.File.FilePath.ToLower() ==
                     PathHelpers.GetRelativePath(filePath, series.Library.FolderPath).ToLower()) ?? false;

            if (exists)
                continue;

            var file = new FileInfo(filePath);

            var pageCount = _comicBookFileSystemService.GetPageCount(filePath);
            var fileName = Path.GetFileName(filePath);
            var fileSize = file.Length;
            var lastModified = file.LastWriteTimeUtc;
            var number = ComicBookHelpers.ParseIssueNumber(fileName);
            var year = ComicBookHelpers.ParseYear(fileName);
            var fileFormat = Path.GetExtension(fileName);
            var comicInfoXml = _comicBookFileSystemService.ReadComicInfoXml(filePath);

            newFiles.Add(new ComicFile(
                number,
                year,
                fileFormat,
                filePath,
                null,
                fileName,
                pageCount,
                fileSize,
                lastModified,
                comicInfoXml));
        }

        return newFiles;
    }

    private async Task UpdateFromComicInfo(Book book)
    {
        var file = new FileInfo(Path.Combine(book.Library.FolderPath, book.File.FilePath));
        var comicInfo = _comicBookFileSystemService.ReadComicInfoXml(file.FullName);

        if (comicInfo == null) return;

        var people = new List<BookPerson>();

        if (!string.IsNullOrEmpty(comicInfo.Writer))
        {
            var ids = await GetPeopleIds(comicInfo.Writer.Split(","));
            people.AddRange(ids.Select(p => new BookPerson(book.Id, p, RoleType.Writer)));
        }

        if (!string.IsNullOrEmpty(comicInfo.Inker))
        {
            var ids = await GetPeopleIds(comicInfo.Inker.Split(","));
            people.AddRange(ids.Select(p => new BookPerson(book.Id, p, RoleType.Inker)));
        }

        if (!string.IsNullOrEmpty(comicInfo.Editor))
        {
            var ids = await GetPeopleIds(comicInfo.Editor.Split(","));
            people.AddRange(ids.Select(p => new BookPerson(book.Id, p, RoleType.Editor)));
        }

        if (!string.IsNullOrEmpty(comicInfo.Penciller))
        {
            var ids = await GetPeopleIds(comicInfo.Penciller.Split(","));
            people.AddRange(ids.Select(p => new BookPerson(book.Id, p, RoleType.Penciller)));
        }

        if (!string.IsNullOrEmpty(comicInfo.Letterer))
        {
            var ids = await GetPeopleIds(comicInfo.Letterer.Split(","));
            people.AddRange(ids.Select(p => new BookPerson(book.Id, p, RoleType.Letterer)));
        }

        if (!string.IsNullOrEmpty(comicInfo.Colorist))
        {
            var ids = await GetPeopleIds(comicInfo.Colorist.Split(","));
            people.AddRange(ids.Select(p => new BookPerson(book.Id, p, RoleType.Colorist)));
        }

        if (!string.IsNullOrEmpty(comicInfo.CoverArtist))
        {
            var ids = await GetPeopleIds(comicInfo.CoverArtist.Split(","));
            people.AddRange(ids.Select(p => new BookPerson(book.Id, p, RoleType.CoverArtist)));
        }

        if (!string.IsNullOrEmpty(comicInfo.Translator))
        {
            var ids = await GetPeopleIds(comicInfo.Translator.Split(","));
            people.AddRange(ids.Select(p => new BookPerson(book.Id, p, RoleType.Translator)));
        }

        book.UpdateFromMetadata(
            comicInfo?.Title,
            comicInfo?.Number,
            comicInfo?.Summary,
            DateOnly.TryParseExact(
                $"{comicInfo?.Year}-{comicInfo?.Month}-{comicInfo?.Day}",
                "yyyy-M-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var coverDate)
                ? coverDate
                : null,
            null,
            people
        );
    }

    private async Task<IEnumerable<Guid>> GetPeopleIds(IEnumerable<string> peopleNames)
    {
        var peopleIds = new List<Guid>();

        foreach (var name in peopleNames)
        {
            var personEntity = await _personRepository.GetByName(name);
            if (personEntity == null)
            {
                personEntity = _personRepository.Add(new Person(Guid.NewGuid(), name.Trim()));
                await _unitOfWork.Commit();
            }

            peopleIds.Add(personEntity.Id);
        }

        return peopleIds;
    }

    private async Task<Series> GetSeries(string seriesPath, Library library)
    {
        var existingSeries = (await _seriesRepository.Get(
            s => s.FolderPath.ToLower() ==
                 PathHelpers.GetRelativePath(seriesPath, library.FolderPath).ToLower() &&
                 s.LibraryId == library.Id)).FirstOrDefault();

        if (existingSeries != null)
        {
            _logger.LogDebug(
                "Matched to series: {@Series}",
                new
                {
                    existingSeries.Id,
                    existingSeries.Name,
                    existingSeries.Year
                });
            return existingSeries;
        }

        var relativeSeriesPath = PathHelpers.GetRelativePath(seriesPath, library.FolderPath);
        var seriesName = ComicBookHelpers.ParseSeriesDirectory(new DirectoryInfo(seriesPath).Name);
        var seriesYear = ComicBookHelpers.ParseYear(seriesPath!);

        var series = new Series(
            Guid.NewGuid(),
            seriesName,
            seriesYear,
            library.Id,
            new DirectoryInfo(seriesPath).Name,
            relativeSeriesPath);

        var firstFile = Path.Combine(library.FolderPath, Directory.GetFiles(seriesPath).OrderBy(f => f).First());

        series.SetMediaRoot(ApplicationHelpers.GetMediaDirectoryRoot(series.Id));

        _logger.LogDebug("Extracting cover image from: {FilePath}", firstFile);

        try
        {
            _comicBookFileSystemService.ExtractComicCoverImage(firstFile, series.MediaRoot, series.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while extracting cover image");
        }

        _logger.LogDebug(
            "No existing Series found in database, adding new series: {@Series}",
            new
            {
                series.Id,
                series.Name,
                series.Volume,
                series.LibraryId,
                MediaPath = series.MediaRoot
            });

        var newSeries = _seriesRepository.Add(series);
        await _unitOfWork.Commit();

        return newSeries;
    }
    
    private async Task UpdateTaskStatus(BackgroundTaskStatus status)
    {
        var task = await _backgroundTaskRepository.Get(_backgroundTask.Id);
        task.UpdateStatus(status);
        _backgroundTaskRepository.Update(task);
        await _unitOfWork.Commit();
    }

    private async Task SendClientUpdate(
        string description,
        BackgroundTaskStatus? status = null,
        string type = "BackgroundTaskUpdated")
    {
        _backgroundTask.Update(description, status?.ToString());
        await _hubContext.Clients.All.SendAsync(
            type,
            _backgroundTask);
    }
}