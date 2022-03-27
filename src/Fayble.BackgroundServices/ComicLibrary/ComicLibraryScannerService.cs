using System.Globalization;
using Fayble.Core.Extensions;
using Fayble.Core.Helpers;
using Fayble.Core.Hubs;
using Fayble.Domain;
using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Domain.Aggregates.Configuration;
using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Enums;
using Fayble.Domain.Repositories;
using Fayble.Models.FileSystem;
using Fayble.Services.FileSystemService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Fayble.BackgroundServices.ComicLibrary;

public class ComicLibraryScannerService : IComicLibraryScannerService
{
    private readonly IBookRepository _bookRepository;
    private readonly IComicBookFileSystemService _comicBookFileSystemService;
    private readonly IConfigurationRepository _configurationRepository;
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
        IConfigurationRepository configurationRepository,
        IComicBookFileSystemService comicBookFileSystemService,
        ISeriesRepository seriesRepository,
        IHubContext<BackgroundTaskHub> hubContext,
        IBackgroundTaskRepository backgroundTaskRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _bookRepository = bookRepository;
        _libraryRepository = libraryRepository;
        _configurationRepository = configurationRepository;
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
            task.Id, libraryId, library.Name, BackgroundTaskType.LibraryScan.ToString(),
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
            _logger.LogError(ex, "An error occurred while scanning library: {libraryId}", libraryId);
            task.UpdateStatus(BackgroundTaskStatus.Failed);
            _backgroundTaskRepository.Update(task);
            await _unitOfWork.Commit();
        }
        finally
        {
            await _hubContext.Clients.All.SendAsync("BackgroundTaskCompleted",
                new BackgroundTask(task.Id, task.ItemId, library.Name, BackgroundTaskType.LibraryScan.ToString(), BackgroundTaskStatus.Complete.ToString()));
        }
    }

    private async Task ScanNewBooks(LibraryPath libraryPath)
    {
        _backgroundTask.UpdateDescription("Scanning new books");
        await _hubContext.Clients.All.SendAsync("BackgroundTaskUpdated", _backgroundTask);

        // TODO: Configuration service?
        var reviewImportedIssue = bool.Parse((await _configurationRepository.Get(Setting.ReviewOnImport)).Value);
        
        _logger.LogDebug("Retrieving new files from library paths.");
        var newFiles = await GetNewFiles(libraryPath.Path);
        _logger.LogDebug("{fileCount} new files found in path", newFiles.Count);

        foreach (var newFile in newFiles)
        {
            _logger.LogDebug("Processing issue: {FilePath}", newFile.FilePath);

            var comicIssue = new Domain.Aggregates.Book.Book(
                Guid.NewGuid(),
                newFile.FileFormat,
                Path.GetFileName(newFile.FilePath),
                newFile
                    .FilePath?.ToLower().Replace(libraryPath.Path.ToLower(), "")
                    .TrimPathSeparators(),
                libraryPath.Id,
                libraryPath.LibraryId,
                MediaType.ComicBook,
                newFile.PageCount,
                newFile.Number);

            comicIssue.UpdateMediaPath(
                ApplicationHelpers.GetMediaDirectory(comicIssue.GetType().Name, comicIssue.Id));

            if (newFile.ComicInfoXml != null)
                comicIssue.Update(
                    newFile.ComicInfoXml.Title,
                    newFile.ComicInfoXml?.Number ?? newFile.Number,
                    newFile.ComicInfoXml?.Summary,
                    newFile.ComicInfoXml?.Notes,
                    0,
                    false,
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
                    null);

            try
            {
                _logger.LogDebug("Extracting cover image from: {FilePath}", newFile.FilePath);
                _comicBookFileSystemService.ExtractComicCoverImage(newFile.FilePath!, comicIssue.MediaPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while extracting cover image.");
            }

            _logger.LogDebug("Matching issue to series: {Series}", newFile.Series);
            var series = await GetSeries(newFile, libraryPath.LibraryId);
            comicIssue.SeriesId = series.Id;

            _bookRepository.Add(comicIssue);
            await _unitOfWork.Commit();

            _logger.LogDebug(
                "New issue added to library: {@ComicIssue}",
                new
                {
                    comicIssue.Id,
                    comicIssue.FileFormat,
                    comicIssue.FilePath,
                    comicIssue.LibraryPathId,
                    comicIssue.LibraryId
                });
        }
    }

    private async Task<List<ComicFile>> GetNewFiles(string path)
    {
        var newFiles = new List<ComicFile>();

        var files = await _comicBookFileSystemService.ScanDirectory(path);

        foreach (var file in files)
        {
            var exists = (await _bookRepository.Get(
                b => b.FilePath.ToLower() ==
                     file.FilePath.ToLower().Replace(path.ToLower(), "").TrimStart('\\'))).Any();

            if (!exists)
                newFiles.Add(file);
        }

        return newFiles;
    }

    private async Task<Domain.Aggregates.Series.Series> GetSeries(ComicFile file, Guid libraryId)
    {
        var series = _seriesRepository.Get().Result.FirstOrDefault(
            x =>
                x.LibraryId == libraryId && x.Name.ToLower() == file.Series?.ToLower() && x.Volume == file.Volume);

        if (series == null)
        {
            series = new Domain.Aggregates.Series.Series(Guid.NewGuid(), file.Series, file.Volume, libraryId);
            series.SetMediaPath(ApplicationHelpers.GetMediaDirectory(series.GetType().Name, series.Id));

            _logger.LogDebug("Extracting cover image from: {FilePath}", file.FilePath);

            try
            {
                _comicBookFileSystemService.ExtractComicCoverImage(file.FilePath, series.MediaPath);
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
        }
        else
        {
            _logger.LogDebug("Matched to series: {@Series}", new {series.Id, series.Name, series.Volume});
        }

        return series;
    }
}