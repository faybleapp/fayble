using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Entities;
using Fayble.Domain.Enums;

namespace Fayble.Domain.Aggregates.FileType;

public class FileType : IdentifiableEntity<Guid>, IAggregateRoot
{
    public string FileExtension { get; private set; }

    public MediaType MediaType { get; private set; }

    public FileType()
    {
    }

    public FileType(string fileExtension, MediaType mediaType)
    {
        FileExtension = fileExtension;
        MediaType = mediaType;
    }
}