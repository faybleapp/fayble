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
    public string MediaRoot { get; private set; }
    public string Language { get; private set; }
    public decimal Rating { get; private set; }
    public DateTime? ReleaseDate { get; private set; }
    public DateTime? CoverDate { get; private set; }
    public MediaType MediaType { get; private set; }
    public BookFile File { get; private set; }
    public Guid? SeriesId { get; private set; }
    public Series.Series Series { get; set; }
    public Guid? LibraryId { get; private set; }
    public Library.Library Library { get; private set; }
    public Guid? PublisherId { get; private set; }
    public Publisher.Publisher Publisher { get; private set; }
    public DateTimeOffset? LastMetadataUpdate { get; private set; }
    public DateTimeOffset? DeletedDate { get; private set; }
    public BookFieldLocks FieldLocks { get; private set; }
    public ICollection<BookTag> Tags { get; set; }

    private readonly List<ReadHistory> _readHistory = new ();
    public virtual IReadOnlyCollection<ReadHistory> ReadHistory => _readHistory;
    private readonly List<BookPerson> _people= new();
    public virtual IReadOnlyCollection<BookPerson> People => _people;

    public Book()
    {
    }

    public Book(
        Guid id,
        Guid libraryId,
        MediaType mediaType,
        string number,
        BookFile file,
        Guid? seriesId = null) : base(id)
    {
        LibraryId = libraryId;
        MediaType = mediaType;
        Number = number;
        File = file;
        SeriesId = seriesId;
        FieldLocks = new BookFieldLocks(id);
    }

    public void Update(
        string title,
        string number,
        string summary,
        decimal rating,
        string language,
        DateOnly? releaseDate,
        DateOnly? coverDate,
        ICollection<BookTag> tags,
        IEnumerable<BookPerson> people)
    {
        Title = title;
        Number = number;
        Summary = summary;
        Rating = rating;
        Language = language;
        ReleaseDate = releaseDate?.ToDateTime(TimeOnly.MinValue);
        CoverDate = coverDate?.ToDateTime(TimeOnly.MinValue);
        Tags = tags;
        
        if (people != null)
        {
            UpdatePeople(people?.ToArray());    
        }
        
    }

    public void UpdateFromMetadata(
        string? title,
        string? number,
        string? summary,
        DateOnly? coverDate,
        ICollection<BookTag>? tags,
        IEnumerable<BookPerson>? people)
    {
        if (!string.IsNullOrEmpty(title))
        {
            Title = title;
        }

        if (!string.IsNullOrEmpty(number))
        {
            Number = number;
        }

        if (!string.IsNullOrEmpty(summary))
        {
            Summary = Summary;
        }

        if (coverDate != null)
        {
            CoverDate = coverDate?.ToDateTime(TimeOnly.MinValue); ;
        }

        if (tags != null)
        {
            Tags = tags;
        }

        if (people != null)
        {
            UpdatePeople(people?.ToArray());
        }
    }

    public void UpdateSeries(Guid seriesId)
    {
        SeriesId = seriesId;
    }

    public void Delete()
    {
        DeletedDate = DateTimeOffset.UtcNow;
    }

    public void Restore()
    {
        DeletedDate = null;
    }

    public void SetMediaRoot(string mediaRoot)
    {
        MediaRoot = mediaRoot;
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

    public void UpdateFieldLock(string field, bool locked)
    {
        FieldLocks.UpdateLock(field, locked);
    }

    private void UpdatePeople(BookPerson[] people)
    {
        var updatedIds = people.Select(c => c.PersonId).ToArray();
        var existingIds = _people.Select(i => i.PersonId).ToArray();
        var itemsToRemove = existingIds.Except(updatedIds);
        foreach (var itemToRemove in itemsToRemove)
        {
            _people.Remove(_people.Find(i => i.PersonId == itemToRemove));
        }

        var itemsToAdd = updatedIds.Except(existingIds);
        _people.AddRange(people.Where(c => itemsToAdd.Contains(c.PersonId)));
    }
}
