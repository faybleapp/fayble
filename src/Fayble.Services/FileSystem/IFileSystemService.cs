using Fayble.Domain.Enums;

namespace Fayble.Services.FileSystem;

public interface IFileSystemService
{
    string GetHash(string filePath);
    Task<IEnumerable<string>> GetFiles(string directory, MediaType mediaType);
}