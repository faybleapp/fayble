namespace Fayble.Services.BackgroundServices.Services;

public interface IScannerService
{
    Task SeriesScan(Guid seriesId, Guid taskId);
    Task LibraryScan(Guid libraryId, Guid taskId);
}