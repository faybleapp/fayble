using Fayble.Models.Series;

namespace Fayble.Services.Series;

public interface ISeriesService
{
    Task<Models.Series.Series?> Get(Guid id);
    Task<IEnumerable<Models.Series.Series>?> GetAll();
    Task<IEnumerable<Models.Book.Book>?> GetBooks(Guid seriesId);
    Task<Models.Series.Series> Update(Guid id, UpdateSeries series);
    Task RefreshMetadata(Guid seriesId);
}