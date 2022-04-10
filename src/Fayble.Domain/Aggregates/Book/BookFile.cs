using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.Book;

public class BookFile : IdentifiableEntity<Guid>
{
    public string FileName { get; private set; }
    public string FilePath { get; private set; }
    public long FileSize { get; private set; }
    public string FileType { get; private set; }
    public DateTimeOffset FileLastModifiedDate { get; private set; }
    public Guid BookId { get; private set; }
    public Book Book { get; private set; }

    public BookFile(){}

    public BookFile(
        Guid id,
        string fileName,
        string filePath,
        long fileSize,
        string fileType, 
        DateTimeOffset fileLastModifiedDate): base(id)
    {
        FileName = fileName;
        FilePath = filePath;
        FileSize = fileSize;
        FileType = fileType;
        FileLastModifiedDate = fileLastModifiedDate;
    }
}