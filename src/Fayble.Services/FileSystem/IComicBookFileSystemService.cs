using Fayble.Models.FileSystem;

namespace Fayble.Services.FileSystem;

public interface IComicBookFileSystemService
{
    ComicInfoXml ParseComicInfoXml(string filePath);
    Task<IEnumerable<string>> GetSeriesDirectories(string libraryPath);
    Task<IEnumerable<string>> GetFiles(string directory);
    int GetPageCount(string filePath);
    void ExtractComicCoverImage(string filePath, string mediaPath);
}