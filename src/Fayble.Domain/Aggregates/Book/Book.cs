using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Entities;
using Fayble.Domain.Enums;

namespace Fayble.Domain.Aggregates.Book;

public class Book : AuditableEntity<Guid>, IAggregateRoot
{
    public string Title { get; private set; }

    public string Summary { get; private set; }

    public string Notes { get; private set; }

    public string Number { get; private set; }

    public int? PageCount { get; private set; }

    public string MediaPath { get; private set; }

    public string Filename { get; private set; }

    public string Language { get; private set; }

    public string FileFormat { get; private set; }

    public string FilePath { get; private set; }

    public bool Locked { get; private set; }

    public decimal Rating { get; private set; }

    public DateOnly? CoverDate { get; private set; }

    public DateOnly? StoreDate { get; private set; }

    public MediaType MediaType { get; private set; }

    public Guid? SeriesId { get; set; }

    public virtual Series.Series Series { get; set; }

    public Guid? LibraryId { get; private set; }

    public virtual Library.Library Library { get; private set; }

    public Guid? LibraryPathId { get; private set; }

    public virtual LibraryPath LibraryPath { get; private set; }

    public Guid? PublisherId { get; private set; }

    public virtual Publisher.Publisher Publisher { get; private set; }

    public Guid? FormatId { get; private set; }

    public virtual Format.Format Format { get; private set; }

    public DateTimeOffset? LastMetadataUpdate { get; private set; }

    public string Review { get; private set; }

    private readonly List<ReadHistory> _readHistory = new ();

    public virtual IReadOnlyCollection<ReadHistory> ReadHistory => _readHistory;

    public ICollection<Tag.Tag> Tags { get; set; }

    public Book()
    {
    }

    public Book(
        Guid id,
        string fileFormat,
        string filename,
        string filePath,
        Guid libraryPathId,
        Guid libraryId,
        MediaType mediaType,
        int? pageCount,
        string number) : base(id)
    {
        FileFormat = fileFormat;
        Filename = filename;
        FilePath = filePath;
        LibraryPathId = libraryPathId;
        LibraryId = libraryId;
        MediaType = mediaType;
        PageCount = pageCount;
        Number = number;
        Locked = false;
    }

    public void Update(
        string title,
        string number,
        string summary,
        string notes,
        decimal rating,
        bool locked,
        string language,
        string review,
        DateOnly? coverDate,
        DateOnly? storeDate)
    {
        Title = title;
        Number = number;
        Summary = summary;
        Notes = notes;
        Rating = rating;
        Locked = locked;
        Language = language;
        Review = review;
        CoverDate = coverDate;
        StoreDate = storeDate;
    }

    public void UpdateMediaPath(string mediaPath)
    {
        MediaPath = mediaPath;
    }

    public void UpdateReadStatus(Guid userId, bool read)
    {
        // _readHistory.Remove(_readHistory.FirstOrDefault(x => x.UserId == userId));

        if (read)
        {
            //_readHistory.Add(new ReadHistory(Id, userId));
        }
    }

    public bool IsRead(Guid userId)
    {
        return true;
        // return _readHistory.Any(x => x.UserId == userId);
    }
}
