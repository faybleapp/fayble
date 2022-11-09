using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.Book;

public class BookPage : IdentifiableEntity<Guid>
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public string FileName { get; private set; }
    public long FileSize { get; private set; }
    public string? FileHash { get; private set;  }
    public string MediaType { get; private set; }
    public int Number { get; private set; }
    public Guid BookFileId { get; private set; }
    public virtual BookFile BookFile { get; private set; }

    public BookPage(
        Guid id,
        int width,
        int height,
        string fileName,
        long fileSize,
        int number,
        string mediaType,
        string? fileHash = null) : base(id)
    {
        Width = width;
        Height = height;
        FileName = fileName;
        FileSize = fileSize;
        FileHash = fileHash;
        Number = number;
        MediaType = mediaType;
    }
}