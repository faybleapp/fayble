using Fayble.Models.FileSystem;

namespace Fayble.Services.FileSystemService;

public interface IComicBookFileSystemService
{
    Task<IEnumerable<ComicFile>> ScanDirectory(string directory);
    ComicInfoXml ParseComicInfoXml(string filePath);
    void ExtractComicCoverImage(string filePath, string mediaPath);
}