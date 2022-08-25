using Fayble.Integration.FaybleApi;
using Fayble.Models.Metadata;
using System.Text.RegularExpressions;

namespace Fayble.Services.MetadataService;

public class MetadataService : IMetadataService
{
    private readonly IFaybleApiClient _faybleClient;

    public MetadataService(IFaybleApiClient faybleClient)
    {
        _faybleClient = faybleClient;
    }

    public async Task<IEnumerable<SeriesSearchResult>> SearchSeries(string searchQuery)
    {
        var year = ParseYear(searchQuery);
        var name = ParseName(searchQuery);
        var providerId = ParseProviderId(searchQuery);
        return await _faybleClient.SearchSeries(name, year, providerId);
    }

    public async Task<SeriesResult> GetSeries(Guid id)
    {
        return await _faybleClient.GetSeries(id);
    }

    private int? ParseYear(string searchQuery)
    {
        var regex = new Regex(@"\w*year:\w*");
        var yearMatch = regex.Match(searchQuery.ToLower()).Value.Replace("year:", string.Empty);

        return yearMatch.Length == 4 && int.TryParse(yearMatch, out var year) ? year : null;
    }

    private string? ParseName(string searchQuery)
    {
        var year = ParseYear(searchQuery)?.ToString();

        if (year != null)
        {
            searchQuery = searchQuery.Replace($"YEAR:{year}", string.Empty, StringComparison.OrdinalIgnoreCase).Trim();
        }

        var providerId = ParseProviderId(searchQuery);

        if (providerId != null)
        {
            searchQuery = searchQuery.Replace($"ID:{providerId}", string.Empty, StringComparison.OrdinalIgnoreCase).Trim();
        }

        return !string.IsNullOrEmpty(searchQuery) ? searchQuery.Trim() : null;
    }

    private string? ParseProviderId(string searchQuery)
    {
        var regex = new Regex(@"\w*id:\w*");
        var match = regex.Match(searchQuery.ToLower()).Value.Replace("id:", string.Empty);
        return !string.IsNullOrEmpty(match) ? match.Trim() : null;
    }
}