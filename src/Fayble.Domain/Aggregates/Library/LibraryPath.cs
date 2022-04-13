using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.Library;

public class LibraryPath : IdentifiableEntity<Guid>
{
    public string Path { get; set; }

    public Guid LibraryId { get; set;  }

    public virtual Library Library { get; set; }

    public LibraryPath()
    {
    }

    public LibraryPath(Guid id, string path) : base(id)
    {
        Path = path;
    }
}