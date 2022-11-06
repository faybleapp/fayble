using Fayble.Domain.Entities;
using Fayble.Domain.Events;

namespace Fayble.Domain.Aggregates.BackgroundTask;
public class BackgroundTask : IdentifiableEntity<Guid>, IAggregateRoot
{
    public string TaskId { get; private set; }

    public string TaskName { get; private set; }

    public BackgroundTaskType Type { get; private set; }

    public DateTimeOffset Started { get; private set; }

    public BackgroundTaskStatus Status { get; private set; }

    public Guid? StartedBy { get; private set; }

    private BackgroundTask() { }

    public BackgroundTask(string taskId, string taskName, BackgroundTaskType type, Guid? startedBy = null)
    {
        TaskId = taskId;
        TaskName = taskName;
        Type = type;
        StartedBy = startedBy;
        Started = DateTimeOffset.UtcNow;
        Status = BackgroundTaskStatus.Queued;

        AddDomainEvent(new BackgroundTaskCreated(Id, TaskId, TaskName, Type, Status));
    }

    public void UpdateStatus(BackgroundTaskStatus status)
    {
        Status = status;
        AddDomainEvent(new BackgroundTaskUpdated(Id, TaskId, TaskName, Type, Status));
    }
}
