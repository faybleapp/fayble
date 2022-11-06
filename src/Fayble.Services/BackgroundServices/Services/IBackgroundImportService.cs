using Fayble.Models.Import;

namespace Fayble.Services.BackgroundServices.Services;

public interface IBackgroundImportService
{
    Task Import(ImportFileRequest importFile, Guid backgroundTaskId);
}