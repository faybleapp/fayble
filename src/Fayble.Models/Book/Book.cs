
namespace Fayble.Models.Book;
public class Book
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }

    public string Summary { get; private set; }

    public string Notes { get; private set; }

    public int? PageCount { get; private set; }

    public string MediaPath { get; private set; }

    public string Filename { get; private set; }

    public string FileFormat { get; private set; }

    public string? FilePath { get; private set; }

    public bool Locked { get; private set; }

    public decimal Rating { get; private set; }

    public Media Media { get; private set; }

    public Publisher.Publisher? Publisher { get; private set; }

    public bool Read { get; private set; }

    public DateTimeOffset Created { get; private set; }

    public DateTimeOffset Modified { get; private set; }

    public string Number { get; private set; }

    public Series.Series? Series { get; private set; }

    public Library.Library? Library { get; private set; }

    public string? CoverDate { get; private set; }

    public string? StoreDate { get; private set; }

    public DateTimeOffset LastMetadataUpdate { get; private set; }

    public string MediaType { get; private set; }

    public Book(
        Guid id,
        string title,
        string summary,
        string notes,
        int? pageCount,
        string mediaPath,
        string filename,
        string fileFormat,
        bool locked,
        decimal rating,
        Publisher.Publisher? publisher,
        bool read,
        string number,
        Series.Series? series,
        Library.Library? library,
        string mediaType,
        string? coverDate,
        string? storeDate
    )
    {
        Id = id;
        Title = title;
        Summary = summary;
        Notes = notes;
        PageCount = pageCount;
        MediaPath = mediaPath;
        Filename = filename;
        FileFormat = fileFormat;
        Locked = locked;
        Rating = rating;
        Publisher = publisher;
        Read = read;
        Number = number;
        Series = series;
        Library = library;
        MediaType = mediaType;
        CoverDate = coverDate;
        StoreDate = storeDate;
        Media = new Media(mediaPath);
    }
}