namespace Fayble.Domain.Entities;

public interface IAuditableEntity
{
    DateTimeOffset CreatedDate { get; }
    Guid CreatedBy { get; }
    DateTimeOffset LastModifiedDate { get; }
    Guid LastModifiedBy { get; }

    void SetLastModified(Guid lastModifiedBy, DateTimeOffset lastModified);
    void SetCreated(Guid createdBy, DateTimeOffset created);
}

public abstract class AuditableEntity<T> : IdentifiableEntity<T>, IAuditableEntity
{
    protected AuditableEntity()
    {
    }

    protected AuditableEntity(T id) : base(id)
    {
    }

    public DateTimeOffset CreatedDate { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTimeOffset LastModifiedDate { get; set; }

    public Guid LastModifiedBy { get; set; }

    public void SetLastModified(Guid lastModifiedBy, DateTimeOffset lastModifiedDate)
    {
        LastModifiedBy = lastModifiedBy;
        LastModifiedDate = lastModifiedDate;
    }

    public void SetCreated(Guid createdBy, DateTimeOffset createdDate)
    {
        CreatedBy = createdBy;
        CreatedDate = createdDate;
    }
}