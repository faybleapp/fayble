using Fayble.Domain;
using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Domain.Repositories;
using Microsoft.AspNetCore.SignalR;

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
        await Clients.Caller.SendAsync("Tasks", runningTasks.Select(t =>
            new Models.BackgroundTask.BackgroundTask(t.Id, t.ItemId, t.Type.ToString())));
    }
}