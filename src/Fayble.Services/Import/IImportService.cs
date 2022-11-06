using Fayble.Models.FileSystem;
using Fayble.Models.Import;

namespace Fayble.Services.Import
{
    public interface IImportService
    {
        Task<IEnumerable<ImportScanFile>> Scan(string path);
        Task Import(List<ImportFileRequest> importFiles);
    }
}