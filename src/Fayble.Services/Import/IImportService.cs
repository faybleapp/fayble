using Fayble.Models.FileSystem;

namespace Fayble.Services.Import
{
    public interface IImportService
    {
        Task<IEnumerable<ComicFile>> Scan(string path);
    }
}