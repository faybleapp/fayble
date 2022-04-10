namespace Fayble.Models.FileSystem;

public class ComicFile
{
    public string? Series { get; }
    public string? Number { get; }
    public string? Year { get; }
    public string? Volume { get; }
    public string? FileType { get;  }
    public string FilePath { get; }
    public string? CoverImage { get; }
    public string FileName { get; }
    public int PageCount { get; }
    public long FileSize { get; }
    public DateTimeOffset FileLastModifiedDate { get; }
    public ComicInfoXml? ComicInfoXml { get; }

    public ComicFile(
        string? series,
        string? number,
        string? year,
        string? volume,
        string? fileType,
        string filePath,
        string? coverImage,
        string fileName,
        int pageCount,
        long fileSize,
        DateTimeOffset fileLastModifiedDate,
        ComicInfoXml? comicInfoXml)
    {
        Series = series;
        Number = number;
        Year = year;
        Volume = volume;
        FileType = fileType;
        FilePath = filePath;
        CoverImage = coverImage;
        FileName = fileName;
        PageCount = pageCount;
        FileSize = fileSize;
        FileLastModifiedDate = fileLastModifiedDate;
        ComicInfoXml = comicInfoXml;
    }
}