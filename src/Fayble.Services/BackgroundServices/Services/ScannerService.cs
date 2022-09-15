using Fayble.Core.Helpers;
using Fayble.Domain;
using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Domain.Aggregates.Book;
using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Aggregates.Person;
using Fayble.Domain.Enums;
using Fayble.Domain.Repositories;
using Fayble.Models.FileSystem;
using Fayble.Services.FileSystem;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Fayble.Services.BackgroundServices.Services;

public class ScannerService : IScannerService
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
    private Fayble.Models.BackgroundTask.BackgroundTask _backgroundTask;

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
            _backgroundTask = new Fayble.Models.BackgroundTask.BackgroundTask(
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
            _logger.LogInformation("Scanning series complete: {SeriesId}", seriesId);
            await SendClientUpdate("Complete", BackgroundTaskStatus.Complete, "BackgroundTaskCompleted");
            
        }
    }

    public async Task LibraryScan(Guid libraryId, Guid taskId)
    {
        try
        {
            var library = await _libraryRepository.Get(libraryId);
            _backgroundTask = new Fayble.Models.BackgroundTask.BackgroundTask(
                taskId,
                libraryId,
                library.Name,
                BackgroundTaskType.LibraryScan.ToString(),
                BackgroundTaskStatus.Queued.ToString());

            await UpdateTaskStatus(BackgroundTaskStatus.Running);
            await SendClientUpdate("Scanning", BackgroundTaskStatus.Running, "BackgroundTaskStarted");
            _logger.LogInformation("Scanning library: {LibraryId}", libraryId);
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
            _logger.LogInformation("Scanning library complete: {LibraryId}", libraryId);
            await SendClientUpdate("Complete", BackgroundTaskStatus.Complete, "BackgroundTaskCompleted");
        }
    }


    private async Task ScanLibrary(Domain.Aggregates.Library.Library library)
    {
        _logger.LogDebug("Retrieving series directories");
        var seriesDirectories = await _comicBookFileSystemService.GetSeriesDirectories(library.FolderPath);
        foreach (var seriesDirectory in seriesDirectories)
        {
            var series = await GetSeries(seriesDirectory, library);
            await ScanSeries(series);
        }
    }

    private async Task ScanSeries(Domain.Aggregates.Series.Series series)
    {
        await ScanExistingBooks(series);
        await ScanNewBooks(series);
    }

    private async Task ScanExistingBooks(Domain.Aggregates.Series.Series series)
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

                var comicFile = _comicBookFileSystemService.GetFile(file.FullName);

                book.File.Update(
                    file.Length,
                    _comicBookFileSystemService.GetHash(file.FullName),
                    comicFile.PageCount);

                if (series.Library.GetSetting<bool>(LibrarySettingKey.UseComicInfo))
                {
                    await UpdateFromComicInfo(book, file.FullName);
                }

                continue;
            }

            _logger.LogDebug("File not modified: {File}", file.FullName);
        }

        _logger.LogInformation("Scanning existing books complete");
        await _unitOfWork.Commit();
    }

    private async Task ScanNewBooks(Domain.Aggregates.Series.Series series)
    {
        _logger.LogInformation("Scanning new books for series: {Series}", series.Name);
        var newFiles = await GetNewFiles(series);
        
        if (!newFiles.Any())
        {
            _logger.LogDebug("No new books");
            return;
        }

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

            var book = new Domain.Aggregates.Book.Book(
                Guid.NewGuid(),
                series.Library.Id,
                MediaType.ComicBook,
                newFile.Number,
                bookFile,
                series.Id);

            book.SetMediaRoot(ApplicationHelpers.GetMediaDirectoryRoot(book.Id));

            if (series.Library.GetSetting<bool>(LibrarySettingKey.UseComicInfo))
            {
                await UpdateFromComicInfo(book, newFile.FilePath);
            }
            
            try
            {
                _logger.LogDebug("Extracting cover image from: {FilePath}", newFile.FilePath);
                _comicBookFileSystemService.ExtractComicCoverImage(newFile.FilePath!, book.MediaRoot, book.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while extracting cover image");
            }

            book.UpdateSeries(series.Id);

            _bookRepository.Add(book);

            _logger.LogDebug(
                "New issue added to library: {@ComicIssue}",
                new
                {
                    book.Id,
                    book.File.FileType,
                    book.File.FilePath,
                    book.LibraryId
                });
        }
        
        _logger.LogDebug("Scanning new books complete");
        await _unitOfWork.Commit();
    }

    private async Task<List<ComicFile>> GetNewFiles(Domain.Aggregates.Series.Series series)
    {
        var newFiles = new List<ComicFile>();
        var path = Path.Combine(series.Library.FolderPath, series.FolderPath);
        var filePaths = await _comicBookFileSystemService.GetFilePaths(path, MediaType.ComicBook);

        foreach (var filePath in filePaths)
        {            
            var exists = series.Books?.Any(
                b => b.File.FilePath.ToLower() ==
                     PathHelpers.GetRelativePath(filePath, series.Library.FolderPath).ToLower()) ?? false;

            if (exists)
                continue;

            var comicFile = _comicBookFileSystemService.GetFile(filePath);
            newFiles.Add(comicFile);
        }

        return newFiles;
    }

    private async Task UpdateFromComicInfo(Domain.Aggregates.Book.Book book, string filepath)
    {
        var file = new FileInfo(filepath);
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
                personEntity = _personRepository.Add(new Domain.Aggregates.Person.Person(Guid.NewGuid(), name.Trim()));
                await _unitOfWork.Commit();
            }

            peopleIds.Add(personEntity.Id);
        }

        return peopleIds;
    }

    private async Task<Domain.Aggregates.Series.Series> GetSeries(string seriesPath, Domain.Aggregates.Library.Library library)
    {
        _logger.LogDebug("Retrieving series for directory: {SeriesPath}", seriesPath);
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

        _logger.LogDebug("Series does not exist, creating");
        var relativeSeriesPath = PathHelpers.GetRelativePath(seriesPath, library.FolderPath);
        var seriesName = ComicBookHelpers.ParseSeriesDirectory(new DirectoryInfo(seriesPath).Name);
        var seriesYear = ComicBookHelpers.ParseYear(seriesPath!);
        var volume = library.GetSetting<bool>(LibrarySettingKey.YearAsVolume) ? seriesYear?.ToString() : null;

        var series = new Domain.Aggregates.Series.Series(
            Guid.NewGuid(),
            seriesName,
            seriesYear,
            volume,
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
            "Creating series: {@Series}",
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