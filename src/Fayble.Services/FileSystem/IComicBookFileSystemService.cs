using Fayble.Models.FileSystem;

namespace Fayble.Services.FileSystem;

public interface IComicBookFileSystemService: IFileSystemService
{
    ComicInfoXml? ReadComicInfoXml(string filePath);
    Task<IEnumerable<string>> GetSeriesDirectories(string libraryPath);
    int GetPageCount(string filePath);
    void ExtractComicCoverImage(string filePath, string mediaPath);
}