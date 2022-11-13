namespace Fayble.Services.BackgroundServices.Services;

public interface IBackgroundMetadataService
{
    Task RefreshSeriesMetadata(Guid seriesId, Guid backgroundTaskId);
}