using Fayble.Domain.Enums;

namespace Fayble.Services.FileSystem;

public interface IFileSystemService
{
    string GetHash(string filePath);
    Task<IEnumerable<string>> GetFilePaths(string directory, MediaType mediaType);
}