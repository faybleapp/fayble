using Fayble.Domain;
using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Domain.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Fayble.Core.Hubs;

public class BackgroundTasksHub : Hub
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBackgroundTaskRepository _backgroundTaskRepository;

    public BackgroundTasksHub(IUnitOfWork unitOfWork, IBackgroundTaskRepository backgroundTaskRepository)
    {
        _unitOfWork = unitOfWork;
        _backgroundTaskRepository = backgroundTaskRepository;
    }

    //This is used for the client to send messages.
    public Task NewBackgroundTaskEvent(string message)
    {
        return Clients.All.SendAsync("BackgroundTaskEvent", message);
    }

    public override async Task OnConnectedAsync()
    {
        var runningTasks = await _backgroundTaskRepository.Get(t => t.Status == BackgroundTaskStatus.Running);
        //var runningTasks = await _unitOfWork.TaskRepository.Get();
        await Clients.Caller.SendAsync("BackgroundTasks", runningTasks.Select(t =>
            new BackgroundTask(t.ItemId, t.Type)));
    }
}