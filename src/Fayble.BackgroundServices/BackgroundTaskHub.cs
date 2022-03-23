using Fayble.Domain;
using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Domain.Repositories;
using Microsoft.AspNetCore.SignalR;
using BackgroundTask = Fayble.Models.BackgroundTask.BackgroundTask;

namespace Fayble.Core.Hubs;

public class BackgroundTaskHub : Hub
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBackgroundTaskRepository _backgroundTaskRepository;

    public BackgroundTaskHub(IUnitOfWork unitOfWork, IBackgroundTaskRepository backgroundTaskRepository)
    {
        _unitOfWork = unitOfWork;
        _backgroundTaskRepository = backgroundTaskRepository;
    }

    //This is used for the client to send messages.
    public Task NewTaskEvent(string message)
    {
        return Clients.All.SendAsync("TaskEvent", message);
    }

    public override async Task OnConnectedAsync()
    {
        var runningTasks = await _backgroundTaskRepository.Get(t => t.Status == BackgroundTaskStatus.Running);
        //var runningTasks = await _unitOfWork.TaskRepository.Get();
        await Clients.Caller.SendAsync("Tasks", runningTasks.Select(t =>
            new BackgroundTask(t.Id, t.ItemId, t.Type.ToString())));
    }
}