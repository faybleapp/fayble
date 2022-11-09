using Fayble.Models.Import;

namespace Fayble.Models.FileSystem;

public class ComicFile
{
    public string FileExtension { get;  }
    public string FilePath { get; }
    public string? CoverImage { get; }
    public string FileName { get; }
    public long FileSize { get; }
    public DateTimeOffset FileLastModifiedDate { get; }
    public ComicInfoXml? ComicInfoXml { get; }
    public List<ComicPage> Pages { get; }

    public ComicFile(
        string fileExtension,
        string filePath,
        string? coverImage,
        string fileName,
        long fileSize,
        DateTimeOffset fileLastModifiedDate,
        ComicInfoXml? comicInfoXml,
        List<ComicPage> pages)
    {
        FileExtension = fileExtension.ToLower().TrimStart('.');
        FilePath = filePath;
        CoverImage = coverImage;
        FileName = fileName;
        FileSize = fileSize;
        FileLastModifiedDate = fileLastModifiedDate;
        ComicInfoXml = comicInfoXml;
        Pages = pages;
    }
}