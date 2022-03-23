using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.FileType;

public class FileType : IdentifiableEntity<Guid>, IAggregateRoot
{
    public string FileExtension { get; private set; }

    public LibraryType LibraryType { get; private set; }

    public FileType()
    {
    }

    public FileType(string fileExtension, LibraryType libraryType)
    {
        FileExtension = fileExtension;
        LibraryType = libraryType;
    }
}