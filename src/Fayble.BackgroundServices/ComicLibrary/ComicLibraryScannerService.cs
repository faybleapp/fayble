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
            
            foreach (var libraryPath in library.Paths)
            {
                _logger.LogInformation("Scanning library for new books: {Library}", library.Name);
                await ScanNewBooks(libraryPath);
            }

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
            await _hubContext.Clients.All.SendAsync("BackgroundTaskCompleted",
                new BackgroundTask(
                    task.Id,
                    task.ItemId,
                    library.Name,
                    BackgroundTaskType.LibraryScan.ToString(),
                    BackgroundTaskStatus.Complete.ToString()));
        }
    }

    private async Task ScanNewBooks(LibraryPath libraryPath)
    {
        _backgroundTask.UpdateDescription("Scanning new books");
        await _hubContext.Clients.All.SendAsync("BackgroundTaskUpdated", _backgroundTask);
        
        _logger.LogDebug("Retrieving new files from library paths");

        var seriesDirectories = await _comicBookFileSystemService.GetSeriesDirectories(libraryPath.Path);

        foreach (var seriesDirectory in seriesDirectories)
        {
            var newFiles = await GetNewFiles(libraryPath.Path, libraryPath.LibraryId);
            _logger.LogDebug("{FileCount} new files found in path", newFiles.Count);

            foreach (var newFile in newFiles)
            {
                _logger.LogDebug("Processing issue: {FilePath}", newFile.FilePath);
                
                var relativePath = Path.GetDirectoryName(newFile.FilePath)!.Replace(
                    libraryPath.Path.ToLower(),
                    string.Empty,
                    StringComparison.InvariantCultureIgnoreCase);
                
                var bookFile = new BookFile(
                    Guid.NewGuid(),
                    newFile.FileName,
                    relativePath,
                    Path.Combine(relativePath, newFile.FileName),
                    newFile.FileSize,
                    newFile.FileType,
                    newFile.FileLastModifiedDate);

                var comicIssue = new Book(
                    Guid.NewGuid(),
                    libraryPath.Id,
                    libraryPath.LibraryId,
                    MediaType.ComicBook,
                    newFile.PageCount,
                    newFile.Number,
                    bookFile);
                
                comicIssue.UpdateMediaPath(
                    ApplicationHelpers.GetMediaDirectory(comicIssue.GetType().Name, comicIssue.Id));
                
                // if settings allow parsing ComicInfoXml
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

                var seriesId = await GetSeries(relativePath, libraryPath);
                comicIssue.UpdateSeries(seriesId);

                _bookRepository.Add(comicIssue);
                await _unitOfWork.Commit();

                _logger.LogDebug(
                    "New issue added to library: {@ComicIssue}",
                    new
                    {
                        comicIssue.Id,
                        comicIssue.File.FileType,
                        comicIssue.File.FullPath,
                        comicIssue.LibraryPathId,
                        comicIssue.LibraryId
                    });

            }
        }
    }

    private async Task<List<ComicFile>> GetNewFiles(string path, Guid libraryId)
    {
        var newFiles = new List<ComicFile>();

        var filePaths = await _comicBookFileSystemService.GetFiles(path);

        foreach (var filePath in filePaths)
        {
            var exists = (await _bookRepository.Get(
                b => b.File.FullPath.ToLower() ==
                    filePath.ToLower().Replace(path.ToLower(), string.Empty).TrimStart('\\') && b.LibraryId == libraryId)).Any();

            if (exists)
                continue;
            
            var file = new FileInfo(filePath);
            
            var pageCount = _comicBookFileSystemService.GetPageCount(filePath);
            var fileName = Path.GetFileName(filePath);
            var fileSize = file.Length;
            var lastModified = file.LastAccessTimeUtc;
            var number = ComicBookHelpers.ParseIssueNumber(fileName);
            var year = ComicBookHelpers.ParseYear(fileName);
            var fileFormat = Path.GetExtension(fileName);
            var comicInfoXml = _comicBookFileSystemService.ParseComicInfoXml(path);
            
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

    private async Task<Guid> GetSeries(string filePath, LibraryPath libraryPath)
    {
        var existingBookInSeries =
            (await _bookRepository.Get(
                b => b.LibraryId == libraryPath.LibraryId && b.File.Directory == filePath && b.Series != null))
            .FirstOrDefault();

        if (existingBookInSeries != null)
        {
            _logger.LogDebug(
                "Matched to series: {@Series}",
                new
                {
                    existingBookInSeries.Series.Id,
                    existingBookInSeries.Series.Name,
                    existingBookInSeries.Series.Year
                });
            return existingBookInSeries.Series.Id;
        }

        var seriesPath = Path.GetDirectoryName(Path.Combine(libraryPath.Path, filePath));
        var seriesName = ComicBookHelpers.ParseSeriesDirectory(seriesPath!);
        var seriesYear = ComicBookHelpers.ParseYear(seriesPath!);

        var series = new Series(Guid.NewGuid(), seriesName, seriesYear, libraryPath.Id);
        series.SetMediaPath(ApplicationHelpers.GetMediaDirectory(series.GetType().Name, series.Id));

        _logger.LogDebug("Extracting cover image from: {FilePath}", Path.Combine(libraryPath.Path, filePath));

        try
        {
            _comicBookFileSystemService.ExtractComicCoverImage(
                Path.Combine(libraryPath.Path, filePath),
                series.MediaPath);
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

        _seriesRepository.Add(series);
        await _unitOfWork.Commit();
        
        return series.Id;
    }
}