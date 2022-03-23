using Fayble.Domain.Aggregates.BackgroundTask;

namespace Fayble.BackgroundServices;

public interface IBackgroundTaskService
{
    Task Run(Guid itemId, BackgroundTaskType taskType);
}