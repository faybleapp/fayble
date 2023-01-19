using Fayble.Domain;
using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Domain.Repositories;
using Fayble.Models.Import;
using Fayble.Services.BackgroundServices.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fayble.Services.BackgroundServices;

public interface IBackgroundTaskService
{
    Task QueueSeriesScan(Guid seriesId);
    Task QueueImport(ImportFileRequest request);
    Task QueueLibraryScan(Guid libraryId);
    Task QueueSeriesMetadataRefresh(Guid seriesId);
    Task QueueBookMetadataRefresh(Guid bookId);
}

public class BackgroundTaskService : IBackgroundTaskService
{
    private readonly IBackgroundTaskRepository _backgroundTaskRepository;
    private readonly ILogger _logger;
    private readonly IBackgroundTaskQueue _queue;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILibraryRepository _libraryRepository;
    private readonly ISeriesRepository _seriesRepository;
    private readonly IBookRepository _bookRepository;

    public BackgroundTaskService(
        ILogger<BackgroundTaskService> logger,
        IBackgroundTaskQueue queue,
        IUnitOfWork unitOfWork,
        IServiceScopeFactory serviceScopeFactory,
        IHubContext<BackgroundTaskHub> hubContext,
        IBackgroundTaskRepository backgroundTaskRepository,
        ILibraryRepository libraryRepository,
        ISeriesRepository seriesRepository,
        IBookRepository bookRepository)
    {
        _logger = logger;
        _queue = queue;
        _unitOfWork = unitOfWork;
        _serviceScopeFactory = serviceScopeFactory;
        _backgroundTaskRepository = backgroundTaskRepository;
        _libraryRepository = libraryRepository;
        _seriesRepository = seriesRepository;
        _bookRepository = bookRepository;
    }


    public async Task QueueLibraryScan(Guid libraryId)
    {
        var library = await _libraryRepository.Get(libraryId);
        var taskId = await CreateTask(library.Id.ToString(), library.Name, BackgroundTaskType.LibraryScan);
        
        await _queue.QueueBackgroundWorkItemAsync(
            async token =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var comicLibraryScanner = scopedServices.GetService<IBackgroundScannerService>()!;
                await comicLibraryScanner.LibraryScan(libraryId, taskId);
            });
    }
    public async Task QueueSeriesScan(Guid seriesId)
    {
       var series = await _seriesRepository.Get(seriesId);
       var taskId = await CreateTask(series.Id.ToString(), series.Name, BackgroundTaskType.LibraryScan);
       await _queue.QueueBackgroundWorkItemAsync(
            async token =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var comicLibraryScanner = scopedServices.GetService<IBackgroundScannerService>()!;
                await comicLibraryScanner.SeriesScan(seriesId, taskId);
            });
    }

    public async Task QueueImport(ImportFileRequest request)
    {
        var existing = await CheckForExisting(request.DestinationFileName, BackgroundTaskType.BookImport);

        if (existing)
        {
            _logger.LogInformation("Existing {TaskType} task for {ItemId} already running or queued", BackgroundTaskType.BookImport.ToString(), request.DestinationFileName);
            return;
        }

        var backgroundTaskId = await CreateTask(request.DestinationFileName, request.DestinationFileName, BackgroundTaskType.LibraryScan);

        await _queue.QueueBackgroundWorkItemAsync(
            async token =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var importService = scopedServices.GetService<IBackgroundImportService>()!;
                await importService.Import(request, backgroundTaskId);
            });
    }

    public async Task QueueSeriesMetadataRefresh(Guid seriesId)
    {
        var series = await _seriesRepository.Get(seriesId);
        var taskId = await CreateTask(series.Id.ToString(), series.Name, BackgroundTaskType.SeriesMetadataRefresh);
        await _queue.QueueBackgroundWorkItemAsync(
            async token =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var metadataService = scopedServices.GetService<IBackgroundMetadataService>()!;
                await metadataService.RefreshSeriesMetadata(seriesId, taskId);
            });
    }

    public async Task QueueBookMetadataRefresh(Guid bookId)
    {
        var book = await _bookRepository.Get(bookId);
        var taskId = await CreateTask(book.Id.ToString(), book.Number, BackgroundTaskType.BookMetadataRefresh);
        //await _queue.QueueBackgroundWorkItemAsync(
        //    async token =>
        //    {
        //        using var scope = _serviceScopeFactory.CreateScope();
        //        var scopedServices = scope.ServiceProvider;
        //        var metadataService = scopedServices.GetService<IBackgroundMetadataService>()!;
        //        await metadataService.RefreshSeriesMetadata(seriesId, taskId);
        //    });
    }

    private async Task<bool> CheckForExisting(string taskId, BackgroundTaskType taskType)
    {
        return (await _backgroundTaskRepository.Get(
            t => t.TaskId == taskId && t.Type == taskType &&
                 (t.Status == BackgroundTaskStatus.Queued || t.Status == BackgroundTaskStatus.Running))).Any();
    }

    private async Task<Guid> CreateTask(string taskId, string taskName, BackgroundTaskType taskType)
    {
        var task = _backgroundTaskRepository.Add(new BackgroundTask(taskId, taskName, taskType));
        await _unitOfWork.Commit();
        return task.Id;
    }
}