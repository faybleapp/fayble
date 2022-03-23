using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.Publisher;

public class Publisher : IdentifiableEntity<Guid>, IAggregateRoot
{
    public string Name { get; private set; }

    public string Description { get; private set; }

    public string MediaPath { get; private set; }

    public DateTimeOffset LastMetadataUpdate { get; private set; }

    private Publisher()
    {
    }

    public Publisher(string name, string description = null) : base(
        Guid.NewGuid())
    {
        Name = name;
        Description = description;
    }

    public void SetMediaPath(string mediaPath)
    {
        MediaPath = mediaPath;
    }
}