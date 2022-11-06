using Fayble.Models.BackgroundTask;
using Fayble.Services.BackgroundServices;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Fayble.EventHandlers.BackgroundTasks;

public class BackgroundTaskUpdatedEventHandler : INotificationHandler<Domain.Events.BackgroundTaskUpdated>
{
    private readonly IHubContext<BackgroundTaskHub> _hubContext;
    private readonly ILogger _logger;

    public BackgroundTaskUpdatedEventHandler(IHubContext<BackgroundTaskHub> hubContext, ILogger<BackgroundTaskUpdatedEventHandler> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task Handle(Domain.Events.BackgroundTaskUpdated notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {Event} event", nameof(BackgroundTaskUpdatedEventHandler));
        await _hubContext.Clients.All.SendAsync(
            nameof(BackgroundTaskUpdatedEventHandler),
            new BackgroundTask(
                notification.Id,
                notification.TaskId,
                notification.TaskName,
                notification.Type.ToString(),
                notification.Status.ToString()),
            cancellationToken);
    }
}