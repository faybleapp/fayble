namespace Fayble.Services.BackgroundServices.Services;

public interface IBackgroundScannerService
{
    Task SeriesScan(Guid seriesId, Guid taskId);
    Task LibraryScan(Guid libraryId, Guid taskId);
    Task BookScan(Guid bookId, Guid backgroundTaskId);
}