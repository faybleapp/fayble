namespace Fayble.Models;

public class Publisher
{
    public Guid? Id { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public string MediaPath { get; private set; }

    public Publisher(Guid? id, string name, string description, string mediaPath)
    {
        Id = id;
        Name = name;
        Description = description;
        MediaPath = mediaPath;
    }
}
