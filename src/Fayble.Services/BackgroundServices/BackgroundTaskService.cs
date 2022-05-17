using Fayble.Domain;
using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Domain.Repositories;
using Fayble.Services.BackgroundServices.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fayble.Services.BackgroundServices;

public interface IBackgroundTaskService
{
    Task Run(Guid itemId, BackgroundTaskType taskType);
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

    public BackgroundTaskService(
        ILogger<BackgroundTaskService> logger,
        IBackgroundTaskQueue queue,
        IUnitOfWork unitOfWork,
        IServiceScopeFactory serviceScopeFactory,
        IHubContext<BackgroundTaskHub> hubContext,
        IBackgroundTaskRepository backgroundTaskRepository,
        ILibraryRepository libraryRepository,
        ISeriesRepository seriesRepository)
    {
        _logger = logger;
        _queue = queue;
        _unitOfWork = unitOfWork;
        _serviceScopeFactory = serviceScopeFactory;
        _backgroundTaskRepository = backgroundTaskRepository;
        _libraryRepository = libraryRepository;
        _seriesRepository = seriesRepository;
    }

    public async Task Run(Guid itemId, BackgroundTaskType taskType)
    {
        var existing = await CheckForExisting(itemId, taskType);
        if (existing)
        {
            _logger.LogInformation("Existing {TaskType} task for {ItemId} already running or queued", taskType, itemId);
            return;
        }

        switch (taskType)
        {
            case BackgroundTaskType.LibraryScan:
                await LibraryScan(itemId);
                break;
            case BackgroundTaskType.SeriesScan:
                await SeriesScan(itemId);
                break;
            default:
                return;
        }
    }

    private async Task LibraryScan(Guid libraryId)
    {
        var library = await _libraryRepository.Get(libraryId);
        var taskId = await CreateTask(library.Id, library.Name, BackgroundTaskType.LibraryScan);
        
        await _queue.QueueBackgroundWorkItemAsync(
            async token =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var comicLibraryScanner = scopedServices.GetService<IScannerService>()!;
                await comicLibraryScanner.LibraryScan(libraryId, taskId);
            });
    }
    private async Task SeriesScan(Guid seriesId)
    {
       var library = await _seriesRepository.Get(seriesId);
       var taskId = await CreateTask(library.Id, library.Name, BackgroundTaskType.LibraryScan);
       await _queue.QueueBackgroundWorkItemAsync(
            async token =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var comicLibraryScanner = scopedServices.GetService<IScannerService>()!;
                await comicLibraryScanner.SeriesScan(seriesId, taskId);
            });
    }

    private async Task<bool> CheckForExisting(Guid itemId, BackgroundTaskType taskType)
    {
        return (await _backgroundTaskRepository.Get(
            t => t.ItemId == itemId && t.Type == taskType &&
                 (t.Status == BackgroundTaskStatus.Queued || t.Status == BackgroundTaskStatus.Running))).Any();
    }

    private async Task<Guid> CreateTask(Guid itemId, string itemName, BackgroundTaskType taskType)
    {
        var task = _backgroundTaskRepository.Add(new BackgroundTask(itemId, itemName, taskType));
        await _unitOfWork.Commit();
        return task.Id;
    }
}