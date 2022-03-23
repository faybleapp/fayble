namespace Fayble.Models.BackgroundTask;

public class BackgroundTaskRequest
{
    public Guid ItemId { get; }
    public string TaskType { get; }

    public BackgroundTaskRequest(Guid itemId, string taskType)
    {
        ItemId = itemId;
        TaskType = taskType;
    }
}