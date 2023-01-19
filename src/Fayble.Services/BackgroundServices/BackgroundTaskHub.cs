using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Domain.Repositories;
using Fayble.Models.BackgroundTask;
using Microsoft.AspNetCore.SignalR;
using BackgroundTask = Fayble.Models.BackgroundTask.BackgroundTask;

namespace Fayble.Services.BackgroundServices;

public class BackgroundTaskHub : Hub
{
    private readonly IBackgroundTaskRepository _backgroundTaskRepository;

    public BackgroundTaskHub(IBackgroundTaskRepository backgroundTaskRepository)
    {
        _backgroundTaskRepository = backgroundTaskRepository;
    }

    public override async Task OnConnectedAsync()
    {
        var runningTasks = await _backgroundTaskRepository.Get(t => t.Status == BackgroundTaskStatus.Running);
        await Clients.Caller.SendAsync(
            "BackgroundTasks",
            runningTasks.Select(
                t =>
                    new BackgroundTask(t.Id, t.TaskId, t.TaskName,  t.Type.ToString(), t.Status.ToString())));
    }
}   