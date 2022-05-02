namespace Fayble.Models.Series;

public class Series
{
    public Guid Id { get;  }

    public string? Name { get;  }

    public string? Volume { get;  }

    public string? Summary { get; }

    public string? Notes { get; }

    public int? Year { get; }

    public int? BookCount { get; }

    public Guid? ParentSeriesId { get; }

    public Series? ParentSeries { get; }

    public Guid? PublisherId { get; }

    public Publisher? Publisher { get; }

    public decimal Rating { get; }

    public Library.Library? Library { get; }

    public bool Read { get; }

    public Media? Media { get; }

    public bool Locked { get; }

    public DateTimeOffset? LastMetadataUpdate { get; }

    public Series(
        Guid id,
        string? name,
        string? volume,
        string? summary,
        string? notes,
        int? year,
        int? bookCount,
        Guid? parentSeriesId,
        Series? parentSeries,
        Guid? publisherId,
        Publisher? publisher,
        decimal rating,
        string? mediaPath,
        Library.Library? library,
        bool read,
        bool locked)
    {
        Id = id;
        Name = name;
        Volume = volume;
        Summary = summary;
        Notes = notes;
        Year = year;
        BookCount = bookCount;
        ParentSeriesId = parentSeriesId;
        ParentSeries = parentSeries;
        Publisher = publisher;
        PublisherId = publisherId;
        Rating = rating;
        Library = library;
        Read = read;
        Media = mediaPath != null ? new Media(mediaPath) : null;
        Locked = locked;
    }
}