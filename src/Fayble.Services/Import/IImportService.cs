using Fayble.Models.FileSystem;
using Fayble.Models.Import;

namespace Fayble.Services.Import
{
    public interface IImportService
    {
        Task<IEnumerable<ComicFile>> Scan(string path);
    }
}