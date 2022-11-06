namespace Fayble.Models.BackgroundTask;

public class BackgroundTask
{
    public Guid Id { get; }
    public string TaskId { get; }
    public string TaskName { get; }
    public string Status { get; private set; }
    public string? Description { get; private set; }
    public string Type { get; }


    public BackgroundTask(
        Guid id,
        string taskId,
        string taskName,
        string type,
        string status,
        string? description = null)
    {
        Id = id;
        TaskId = taskId;
        TaskName = taskName;
        Type = type;
        Status = status;
        Description = description;
    }

    public void Update(string description, string? status = null)
    {
        if (status != null) Status = status;

        Description = description;
    }
}