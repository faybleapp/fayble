
using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.Tag;

public class Tag : IdentifiableEntity<Guid>, IAggregateRoot
{
    public string Name { get; private set; }

    public ICollection<Book.Book> Books { get; private set; }

    public ICollection<Series.Series> Series { get; private set; }

    public Tag(Guid id, string name) : base(id)
    {
        Name = name;
    }
}