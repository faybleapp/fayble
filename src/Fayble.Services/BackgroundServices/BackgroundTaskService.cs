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
    private readonly IHubContext<BackgroundTaskHub> _hubContext;
    private readonly ILogger _logger;
    private readonly IBackgroundTaskQueue _queue;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IUnitOfWork _unitOfWork;

    public BackgroundTaskService(
        ILogger<BackgroundTaskService> logger,
        IBackgroundTaskQueue queue,
        IUnitOfWork unitOfWork,
        IServiceScopeFactory serviceScopeFactory,
        IHubContext<BackgroundTaskHub> hubContext,
        IBackgroundTaskRepository backgroundTaskRepository)
    {
        _logger = logger;
        _queue = queue;
        _unitOfWork = unitOfWork;
        _serviceScopeFactory = serviceScopeFactory;
        _hubContext = hubContext;
        _backgroundTaskRepository = backgroundTaskRepository;
    }

    public async Task Run(Guid itemId, BackgroundTaskType taskType)
    {
        var task = _backgroundTaskRepository.Add(new Domain.Aggregates.BackgroundTask.BackgroundTask(itemId, taskType));
        await _unitOfWork.Commit();
        switch (taskType)
        {
            case BackgroundTaskType.LibraryScan:
                await LibraryScan(itemId, task.Id);
                break;
            case BackgroundTaskType.SeriesScan:
                await SeriesScan(itemId, task.Id);
                break;
            default:
                return;
        }
    }

    private async Task LibraryScan(Guid libraryId, Guid taskId)
    {
        await _queue.QueueBackgroundWorkItemAsync(
            async token =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var comicLibraryScanner = scopedServices.GetService<IScannerService>()!;
                await comicLibraryScanner.LibraryScan(libraryId, taskId);
            });
    }
    private async Task SeriesScan(Guid seriesId, Guid taskId)
    {
       await _queue.QueueBackgroundWorkItemAsync(
            async token =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var comicLibraryScanner = scopedServices.GetService<IScannerService>()!;
                await comicLibraryScanner.SeriesScan(seriesId, taskId);
            });
    }
}