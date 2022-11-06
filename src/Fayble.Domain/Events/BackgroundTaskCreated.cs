using Fayble.Domain.Aggregates.BackgroundTask;

namespace Fayble.Domain.Events;

public class BackgroundTaskCreated : IDomainEvent
{
    public Guid Id { get; }
    public string TaskId { get; }
    public string TaskName { get; }
    public BackgroundTaskType Type { get; }
    public BackgroundTaskStatus Status { get; }

    public BackgroundTaskCreated(
        Guid id,
        string taskId,
        string taskName,
        BackgroundTaskType type,
        BackgroundTaskStatus status)
    {
        Id = id;
        TaskId = taskId;
        TaskName = taskName;
        Type = type;
        Status = status;
    }
}