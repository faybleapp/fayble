using Fayble.Models.FileSystem;
using Fayble.Models.Import;

namespace Fayble.Services.FileSystem;

public interface IComicBookFileSystemService: IFileSystemService
{
    ComicInfoXml? ReadComicInfoXml(string filePath);
    Task<IEnumerable<string>> GetSeriesDirectories(string libraryPath);
    void ExtractComicCoverImage(string filePath, string mediaRoot, Guid id);
    ComicFile GetFile(string filePath);
    Task<string> GenerateFilename(GenerateFilenameRequest request);
    Task<bool> FileExists(FileExistsRequest request);
}
