using Fayble.Models.Metadata;

namespace Fayble.Services.MetadataService;

public interface IMetadataService
{
    Task<IEnumerable<SeriesSearchResult>> SearchSeries(string name, int? year);
    Task<SeriesResult> GetSeries(Guid id);
}