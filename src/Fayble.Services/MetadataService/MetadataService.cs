using Fayble.Integration.FaybleApi;
using Fayble.Models.Metadata;

namespace Fayble.Services.MetadataService;

public class MetadataService : IMetadataService
{
    private readonly IFaybleApiClient _faybleClient;

    public MetadataService(IFaybleApiClient faybleClient)
    {
        _faybleClient = faybleClient;
    }

    public async Task<IEnumerable<SeriesSearchResult>> SearchSeries(string name, int? year)
    {
        return await _faybleClient.SearchSeries(name, year);
    }

    public async Task<SeriesResult> GetSeries(Guid id)
    {
        return await _faybleClient.GetSeries(id);
    }
}