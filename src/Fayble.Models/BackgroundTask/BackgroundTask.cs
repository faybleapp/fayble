namespace Fayble.Models.BackgroundTask;

public class BackgroundTask
{
    public Guid Id { get; }
    public Guid? ItemId { get; }
    public int Progress { get; }
    public string? Description { get; }
    public string Type { get; }


    public BackgroundTask(Guid id, Guid? itemId, string type, string? description = null)
    {
        Id = id;
        ItemId = itemId;
        Type = type;
        Description = description;
    }
}
