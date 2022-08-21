using Fayble.Models.Metadata;

namespace Fayble.Integration.FaybleApi;

public interface IFaybleApiClient
{
    Task<IEnumerable<SeriesSearchResult>> SearchSeries(string name, int? year, string? providerId);
    Task<SeriesResult> GetSeries(Guid id);
}