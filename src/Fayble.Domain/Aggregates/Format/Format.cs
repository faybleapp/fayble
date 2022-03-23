using Fayble.Domain.Entities;
using Fayble.Domain.Enums;

namespace Fayble.Domain.Aggregates.Format;

public class Format : IdentifiableEntity<Guid>, IAggregateRoot
{
    public string Name { get; set; }

    public MediaType MediaType { get; set; }
}