using System.Globalization;
using Fayble.Core.Extensions;
using Fayble.Core.Helpers;
using Fayble.Core.Hubs;
using Fayble.Domain;
using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Domain.Aggregates.Book;
using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Aggregates.Series;
using Fayble.Domain.Enums;
using Fayble.Domain.Repositories;
using Fayble.Models.FileSystem;
using Fayble.Services.FileSystem;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp.Processing;

namespace Fayble.BackgroundServices.ComicLibrary;

public class ComicLibraryScannerService : IComicLibraryScannerService
{
    private readonly IBookRepository _bookRepository;
    private readonly IComicBookFileSystemService _comicBookFileSystemService;
    private readonly ILibraryRepository _libraryRepository;
    private readonly IBackgroundTaskRepository _backgroundTaskRepository;
    private readonly ILogger _logger;
    private readonly ISeriesRepository _seriesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<BackgroundTaskHub> _hubContext;
    private BackgroundTask _backgroundTask;

    public ComicLibraryScannerService(
        ILogger<ComicLibraryScannerService> logger,
        IUnitOfWork unitOfWork,
        IBookRepository bookRepository,
        ILibraryRepository libraryRepository,
        IComicBookFileSystemService comicBookFileSystemService,
        ISeriesRepository seriesRepository,
        IHubContext<BackgroundTaskHub> hubContext,
        IBackgroundTaskRepository backgroundTaskRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _bookRepository = bookRepository;
        _libraryRepository = libraryRepository;
        _comicBookFileSystemService = comicBookFileSystemService;
        _seriesRepository = seriesRepository;
        _hubContext = hubContext;
        _backgroundTaskRepository = backgroundTaskRepository;
    }

    public async Task Run(Guid libraryId, Guid taskId)
    {
        var library = await _libraryRepository.Get(libraryId);
        var task = await _backgroundTaskRepository.Get(taskId);
        _backgroundTask = new BackgroundTask(
            task.Id,
            libraryId,
            library.Name,
            BackgroundTaskType.LibraryScan.ToString(),
            BackgroundTaskStatus.Running.ToString());

        try
        {
            task.UpdateStatus(BackgroundTaskStatus.Running);
            _backgroundTaskRepository.Update(task);
            await _unitOfWork.Commit();

            await _hubContext.Clients.All.SendAsync("BackgroundTaskStarted", _backgroundTask);
            
            _logger.LogInformation("Scanning library for new books: {Library}", library.Name);
            await Scan(library);

            task.UpdateStatus(BackgroundTaskStatus.Complete);
            _backgroundTaskRepository.Update(task);
            await _unitOfWork.Commit();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while scanning library: {LibraryId}", libraryId);
            task.UpdateStatus(BackgroundTaskStatus.Failed);
            _backgroundTaskRepository.Update(task);
            await _unitOfWork.Commit();
        }
        finally
        {
            await _hubContext.Clients.All.SendAsync(
                "BackgroundTaskCompleted",
                new BackgroundTask(
                    task.Id,
                    task.ItemId,
                    library.Name,
                    BackgroundTaskType.LibraryScan.ToString(),
                    BackgroundTaskStatus.Complete.ToString()));
        }
    }

    private async Task Scan(Library library)
    {
        _backgroundTask.UpdateDescription("Scanning new books");
        await _hubContext.Clients.All.SendAsync("BackgroundTaskUpdated", _backgroundTask);

        _logger.LogDebug("Retrieving new files from library paths");

        var seriesDirectories = await _comicBookFileSystemService.GetSeriesDirectories(library.FolderPath);

        foreach (var seriesDirectory in seriesDirectories)
        {
            var series = await GetSeries(seriesDirectory, library);
            await ScanExistingBooks(series);
            await ScanNewBooks(series);
        }
    }

    private async Task ScanExistingBooks(Series series)
    {
        _logger.LogInformation("Scanning existing books for Series: {Series}", series.Name);
        if (series.Books == null || !series.Books.Any())
        {
            _logger.LogDebug("No existing books.");
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
                var comicInfo = _comicBookFileSystemService.ReadComicInfoXml(file.FullName);

                book.File.Update(
                    file.Length,
                    _comicBookFileSystemService.GetHash(file.FullName),
                    _comicBookFileSystemService.GetPageCount(file.FullName));

                // TODO: if settings allow parsing ComicInfoXml
                if (comicInfo != null)
                    book.Update(
                        comicInfo?.Title,
                        comicInfo?.Number,
                        comicInfo?.Summary,
                        0,
                        null,
                        null,
                        DateOnly.TryParseExact(
                            $"{comicInfo?.Year}-{comicInfo?.Month}-{comicInfo?.Day}",
                            "yyyy-M-dd",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out var coverDate)
                            ? coverDate
                            : null,
                        null
                    );


               

                continue;
            }

            _logger.LogDebug("File not modified: {File}", file.FullName);
        }

        _logger.LogInformation("Scanning existing books for Series: {Series}", series.Name);
        await _unitOfWork.Commit();
    }

    private async Task ScanNewBooks(Series series)
    {
        var newFiles = await GetNewFiles(series);
        _logger.LogDebug("{FileCount} new files found in path", newFiles.Count);

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

            comicIssue.UpdateMediaPath(
                ApplicationHelpers.GetMediaDirectory(comicIssue.GetType().Name, comicIssue.Id));

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
                    null
                );

            try
            {
                _logger.LogDebug("Extracting cover image from: {FilePath}", newFile.FilePath);
                _comicBookFileSystemService.ExtractComicCoverImage(newFile.FilePath!, comicIssue.MediaPath);
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

        series.SetMediaPath(ApplicationHelpers.GetMediaDirectory(series.GetType().Name, series.Id));

        _logger.LogDebug("Extracting cover image from: {FilePath}", firstFile);

        try
        {
            _comicBookFileSystemService.ExtractComicCoverImage(firstFile, series.MediaPath);
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
                series.MediaPath
            });

        var newSeries = _seriesRepository.Add(series);
        await _unitOfWork.Commit();
        
        return newSeries;
    }
}