namespace Fayble.Domain.Aggregates.BackgroundTask;

public enum BackgroundTaskType
{
    LibraryScan,
    SeriesScan,
    BookImport,
    SeriesMetadataRefresh,
    BookMetadataRefresh
}