using Fayble.Models.Import;

namespace Fayble.Models.FileSystem;

public class ComicFile
{
    public string? FileExtension { get;  }
    public string FilePath { get; }
    public string? CoverImage { get; }
    public string FileName { get; }
    public int PageCount { get; }
    public long FileSize { get; }
    public DateTimeOffset FileLastModifiedDate { get; }
    public ComicInfoXml? ComicInfoXml { get; }

    public ComicFile(
        string? fileExtension,
        string filePath,
        string? coverImage,
        string fileName,
        int pageCount,
        long fileSize,
        DateTimeOffset fileLastModifiedDate,
        ComicInfoXml? comicInfoXml)
    {
        FileExtension = fileExtension?.ToLower().TrimStart('.');
        FilePath = filePath;
        CoverImage = coverImage;
        FileName = fileName;
        PageCount = pageCount;
        FileSize = fileSize;
        FileLastModifiedDate = fileLastModifiedDate;
        ComicInfoXml = comicInfoXml;
    }
}