using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.Book;

public class BookFile : IdentifiableEntity<Guid>
{
    public string FileName { get; private set; }
    public string FilePath { get; private set; }
    public long FileSize { get; private set; }
    public string FileExtension { get; private set; }
    public string FileHash { get; private set;  }
    public DateTimeOffset FileLastModifiedDate { get; private set; }
    public Guid BookId { get; private set; }
    public virtual Book Book { get; private set; }

    private readonly List<BookPage> _pages = new();
    public virtual IReadOnlyCollection<BookPage> Pages => _pages;

    public BookFile(){}

    public BookFile(
        Guid id,
        string fileName,
        string filePath,
        long fileSize,
        string fileExtension, 
        DateTimeOffset fileLastModifiedDate,
        string fileHash,
        IEnumerable<BookPage> pages) : base(id)
    {
        FileName = fileName;
        FilePath = filePath;
        FileSize = fileSize;
        FileExtension = fileExtension;
        FileLastModifiedDate = fileLastModifiedDate;
        FileHash = fileHash;
        
        _pages.AddRange(pages);
    }

    public void Update(long fileSize, string fileHash)
    {
        FileSize = fileSize;
        FileHash = fileHash;
    }
}