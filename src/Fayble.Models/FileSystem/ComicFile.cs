namespace Fayble.Models.FileSystem;

public class ComicFile
{
    public string? Series { get; }
    public string? Number { get; }
    public string? Year { get; }
    public string? Volume { get; }
    public string? FileFormat { get;  }
    public string FilePath { get; }
    public string? CoverImage { get; }
    public string FileName { get; }
    public int PageCount { get; }
    public ComicInfoXml? ComicInfoXml { get; }

    public ComicFile(
        string? series,
        string? number,
        string? year,
        string? volume,
        string? fileFormat,
        string filePath,
        string? coverImage,
        string fileName,
        int pageCount,
        ComicInfoXml? comicInfoXml)
    {
        Series = series;
        Number = number;
        Year = year;
        Volume = volume;
        FileFormat = fileFormat;
        FilePath = filePath;
        CoverImage = coverImage;
        FileName = fileName;
        PageCount = pageCount;
        ComicInfoXml = comicInfoXml;
    }
}