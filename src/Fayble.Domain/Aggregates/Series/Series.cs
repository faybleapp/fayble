using Fayble.Domain.Aggregates.Book;
using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.Series;

public class Series : AuditableEntity<Guid>, IAggregateRoot
{
    public string Name { get; private set; }
    public string Volume { get; private set; }
    public string Summary { get; private set; }
    public string Notes { get; private set; }
    public int? Year { get; private set; }
    public int? BookCount => Books?.Count;
    public decimal Rating { get; private set; }
    public string MediaRoot { get; private set; }
    public string FolderPath { get; private set; }
    public string FolderName { get; private set; }
    public bool Locked { get; private set; }
    public Guid? MatchId { get; private set; }
    public DateTimeOffset? LastMetadataUpdate { get; private set; }
    public virtual ICollection<Book.Book> Books { get; private set; }
    public SeriesFieldLocks FieldLocks { get; private set; }

    public Guid? ParentSeriesId { get; private set; }
    public virtual Series ParentSeries { get; }
    public Guid? PublisherId { get; private set; }
    public virtual Publisher.Publisher Publisher { get; private set; }
    public Guid? FormatId { get; private set; }
    public virtual Format.Format Format { get; private set; }
    public Guid? LibraryId { get; private set; }
    public virtual Library.Library Library { get; private set; }


    private Series()
    {
    }

    public Series(Guid id, string name, int? year, string volume, Guid libraryId, string folderName, string folderPath) : base(id)
    {
        Guard.AgainstNullOrWhitespace(name, nameof(Name));

        Name = name;
        Year = year;
        LibraryId = libraryId;
        Volume = volume;
        Locked = false;
        FolderPath = folderPath;
        FolderName = folderName;
        FieldLocks = new SeriesFieldLocks(id);
    }

    public void Update(
        string name,
        int? year,
        string summary,
        string notes,
        string volume,
        decimal rating,
        Guid? publisherId,
        Guid? parentSeriesId,
        Guid? matchId)
    {
        Guard.AgainstNullOrWhitespace(name, nameof(Name));

        Name = name;
        Year = year;
        Summary = summary;
        Notes = notes;
        Volume = volume;
        Rating = rating;
        PublisherId = publisherId;
        ParentSeriesId = parentSeriesId;
        MatchId = matchId;
    }

    public void SetMediaRoot(string mediaRoot)
    {
        MediaRoot = mediaRoot;
    }

    public void UpdateFieldLock(string field, bool locked)
    {
        FieldLocks.UpdateLock(field, locked);
    }

    public bool IsRead(Guid userId)
    {
        return Books?.Where(x => !x.ReadHistory.Any(y => y.UserId == userId)).Any() == false;
    }
}
