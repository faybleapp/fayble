namespace Fayble.Domain.Entities;

public interface IAuditable
{
    DateTimeOffset Created { get; }
    Guid CreatedBy { get; }
    DateTimeOffset LastModified { get; }
    Guid LastModifiedBy { get; }

    void SetLastModified(Guid lastModifiedBy, DateTimeOffset lastModified);
    void SetCreated(Guid createdBy, DateTimeOffset created);
}

public abstract class AuditableEntity<T> : IdentifiableEntity<T>, IAuditable
{
    protected AuditableEntity()
    {
    }

    protected AuditableEntity(T id) : base(id)
    {
    }

    public DateTimeOffset Created { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    public Guid LastModifiedBy { get; set; }

    public void SetLastModified(Guid lastModifiedBy, DateTimeOffset lastModified)
    {
        LastModifiedBy = lastModifiedBy;
        LastModified = lastModified;
    }

    public void SetCreated(Guid createdBy, DateTimeOffset created)
    {
        CreatedBy = createdBy;
        Created = created;
    }
}