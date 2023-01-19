using Fayble.Domain.Events;
using Fayble.Models.BackgroundTask;
using Fayble.Services.BackgroundServices;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Fayble.EventHandlers.BackgroundTasks;

public class BackgroundTaskCreatedEventHandler : INotificationHandler<BackgroundTaskCreated>
{
    private readonly IHubContext<BackgroundTaskHub> _hubContext;
    private readonly ILogger _logger;

    public BackgroundTaskCreatedEventHandler(
        IHubContext<BackgroundTaskHub> hubContext,
        ILogger<BackgroundTaskCreatedEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task Handle(BackgroundTaskCreated notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {Event} event", nameof(BackgroundTaskCreatedEventHandler));
        await _hubContext.Clients.All.SendAsync(
            "BackgroundTaskCreated",
            new BackgroundTask(
                notification.Id,
                notification.TaskId,
                notification.TaskName,
                notification.Type.ToString(),
                notification.Status.ToString()),
            cancellationToken);
    }
}