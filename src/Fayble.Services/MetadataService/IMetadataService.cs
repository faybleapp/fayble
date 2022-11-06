using Fayble.Models.Metadata;

namespace Fayble.Services.MetadataService;

public interface IMetadataService
{
    Task<IEnumerable<SeriesSearchResult>> SearchSeries(string searchQuery);
    Task<SeriesResult> GetSeries(Guid id);
    Task<BookResult> GetBook(Guid id);
}