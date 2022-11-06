using Fayble.Domain.Events;

namespace Fayble.Domain.Entities;

/// <summary>
/// Base class for all entities.
/// </summary>
public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();
    /// <summary>
    /// Collection of Domain Events
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    /// <summary>
    /// Add domain event
    /// </summary>
    /// <param name="domainEvent"></param>
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Remove domain event
    /// </summary>
    /// <param name="domainEvent"></param>
    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents?.Remove(domainEvent);
    }

    /// <summary>
    /// Clear domain events
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

