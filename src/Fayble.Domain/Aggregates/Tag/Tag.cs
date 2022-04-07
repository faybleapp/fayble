using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.Tag;

public abstract class Tag : IdentifiableEntity<Guid>, IAggregateRoot
{
    public string Name { get; internal set; }

    public Tag() { }

    public Tag(Guid id) : base(id)
    {
    }
}