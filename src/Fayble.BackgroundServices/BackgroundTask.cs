﻿namespace Fayble.BackgroundServices;

public class BackgroundTask
{
    public Guid Id { get; }
    public Guid? ItemId { get; }
    public string ItemName { get; }
    public string Status { get; }
    public string? Description { get; private set; }
    public string Type { get; }


    public BackgroundTask(Guid id, Guid? itemId, string itemName, string type, string status, string description = null)
    {
        Id = id;
        ItemId = itemId;
        ItemName = itemName;
        Type = type;
        Status = status;
        Description = description;
    }

    public void UpdateDescription(string description)
    {
        Description = description;
    }
}
