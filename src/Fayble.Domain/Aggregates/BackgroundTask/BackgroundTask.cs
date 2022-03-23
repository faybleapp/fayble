using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.BackgroundTask;
public class BackgroundTask : IdentifiableEntity<Guid>, IAggregateRoot
{
    public Guid? ItemId { get; private set; }

    public BackgroundTaskType Type { get; private set; }

    public DateTimeOffset Started { get; private set; }

    public BackgroundTaskStatus Status { get; private set; }

    public Guid? StartedBy { get; private set; }

    private BackgroundTask() { }

    public BackgroundTask(Guid? itemId, BackgroundTaskType type, Guid? startedBy = null)
    {
        ItemId = itemId;
        Type = type;
        StartedBy = startedBy;
        Started = DateTimeOffset.UtcNow;
        Status = BackgroundTaskStatus.Queued;
    }

    public void UpdateStatus(BackgroundTaskStatus status)
    {
        Status = status;
    }
}
