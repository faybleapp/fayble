using Fayble.BackgroundServices.ComicLibrary;
using Fayble.Core.Hubs;
using Fayble.Domain;
using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Domain.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fayble.BackgroundServices;

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
                LibraryScan(itemId, task);
                break;
            case BackgroundTaskType.SeriesScan:
                break;
            default:
                return;
        }
    }

    private void LibraryScan(Guid libraryId, Domain.Aggregates.BackgroundTask.BackgroundTask task)
    {
        _queue.QueueBackgroundWorkItem(
            async token =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var comicLibraryScanner = scopedServices.GetService<IComicLibraryScannerService>();

                await comicLibraryScanner.Run(libraryId, task.Id);
            });
    }
}