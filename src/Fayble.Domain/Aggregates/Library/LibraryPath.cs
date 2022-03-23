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

    public LibraryPath(string path, Guid libraryId)
    {
        Path = path;
        LibraryId = libraryId;
    }
}