namespace Fayble.Domain.Entities;

public abstract class IdentifiableEntity<TId> : Entity
{
    protected IdentifiableEntity()
    {
        Id = default;
    }

    protected IdentifiableEntity(TId id)
    {
        Id = id;
    }

    public TId Id { get; private set; }
}