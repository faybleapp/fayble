namespace Fayble.Models.Book;

public class Book
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Summary { get; private set; }
    public int? PageCount { get; private set; }
    public string MediaPath { get; private set; }
    public string FileName { get; private set; }
    public string FileType { get; private set; }
    public string FilePath { get; private set; }
    public double FileSize { get; private set; }
    public decimal Rating { get; private set; }
    public Media Media { get; private set; }
    public Publisher? Publisher { get; private set; }
    public bool Read { get; private set; }
    public DateTimeOffset Created { get; private set; }
    public DateTimeOffset Modified { get; private set; }
    public string Number { get; private set; }
    public string Language { get; private set; }
    public Series.Series? Series { get; private set; }
    public Library.Library? Library { get; private set; }
    public string? ReleaseDate { get; private set; }
    public string? CoverDate { get; private set; }
    public DateTimeOffset LastMetadataUpdate { get; private set; }
    public string MediaType { get; private set; }    
    public IEnumerable<string> Tags {get; private set;}


    public Book(
        Guid id,
        string title,
        string summary,
        int? pageCount,
        string mediaPath,
        string fileName,
        string fileType,
        double fileSize,
        string filePath,
        decimal rating,
        Publisher? publisher,
        bool read,
        string number,
        Series.Series? series,
        Library.Library? library,
        string mediaType,
        string? releaseDate,
        string? coverDate,
        string language,
        IEnumerable<string>? tags
    )
    {
        Id = id;
        Title = title;
        Summary = summary;
        PageCount = pageCount;
        MediaPath = mediaPath;
        FileName = fileName;
        FileType = fileType;
        FileSize = fileSize;
        FilePath = filePath;
        Rating = rating;
        Publisher = publisher;
        Read = read;
        Number = number;
        Series = series;
        Library = library;
        MediaType = mediaType;
        ReleaseDate = releaseDate;
        CoverDate = coverDate;
        Language = language;
        Tags = tags;
        Media = new Media(mediaPath);
    }
}