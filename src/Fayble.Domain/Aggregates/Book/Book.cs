using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Aggregates.Tag;
using Fayble.Domain.Entities;
using Fayble.Domain.Enums;

namespace Fayble.Domain.Aggregates.Book;

public class Book : AuditableEntity<Guid>, IAggregateRoot
{
    public string Title { get; private set; }
    public string Summary { get; private set; }
    public string Number { get; private set; }
    public int? PageCount { get; private set; }
    public string MediaPath { get; private set; }
    public string Language { get; private set; }
    public decimal Rating { get; private set; }
    public DateOnly? ReleaseDate { get; private set; }
    public DateOnly? CoverDate { get; private set; }
    public MediaType MediaType { get; private set; }
    public BookFile File { get; private set; }
    public Guid? SeriesId { get; set; }
    public Series.Series Series { get; set; }
    public Guid? LibraryId { get; private set; }
    public Library.Library Library { get; private set; }
    public Guid? LibraryPathId { get; private set; }
    public LibraryPath LibraryPath { get; private set; }
    public Guid? PublisherId { get; private set; }
    public Publisher.Publisher Publisher { get; private set; }
    public DateTimeOffset? LastMetadataUpdate { get; private set; }

    private readonly List<ReadHistory> _readHistory = new ();
    public virtual IReadOnlyCollection<ReadHistory> ReadHistory => _readHistory;
    private readonly List<Person.Person> _people= new();
    public virtual IReadOnlyCollection<Person.Person> People => _people;

    public ICollection<BookTag> Tags { get; set; }
    
    public Book()
    {
    }

    public Book(
        Guid id,
        Guid libraryPathId,
        Guid libraryId,
        MediaType mediaType,
        int? pageCount,
        string number,
        BookFile file) : base(id)
    {
       
        LibraryPathId = libraryPathId;
        LibraryId = libraryId;
        MediaType = mediaType;
        PageCount = pageCount;
        Number = number;
        File = file;
    }

    public void Update(
        string title,
        string number,
        string summary,
        decimal rating,
        string language,
        DateOnly? releaseDate,
        DateOnly? coverDate,
        ICollection<BookTag> tags)
    {
        Title = title;
        Number = number;
        Summary = summary;
        Rating = rating;
        Language = language;
        ReleaseDate = releaseDate;
        CoverDate = coverDate;
        Tags = tags;
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
