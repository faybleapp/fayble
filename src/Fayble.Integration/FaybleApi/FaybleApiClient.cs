using Fayble.Models.Configuration;
using Fayble.Models.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Fayble.Integration.FaybleApi;

public class FaybleApiClient : IFaybleApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;
    private readonly FaybleApiConfiguration _faybleApiConfiguration;

    public FaybleApiClient(
        ILogger<FaybleApiClient> logger,
        IHttpClientFactory httpClientFactory,
        IOptions<FaybleApiConfiguration> faybleApiConfiguration)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _faybleApiConfiguration = faybleApiConfiguration.Value;
    }

    public async Task<IEnumerable<SeriesSearchResult>> SearchSeries(string name, int? year)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(
            $"{_faybleApiConfiguration.BaseUrl}/api/series/search?name={name}{(year != null ? $"&year={year}" : string.Empty)}");

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "An error occurred while retrieving metadata from Fayble API: '{ReasonPhrase}' | {@Details}",
                response.ReasonPhrase,
                new
                {
                    Name = name,
                    Year = year,
                    StatusCode = (int) response.StatusCode,
                    Repsonse = response.ReasonPhrase
                });
            throw;
        }

        var responseString = await response.Content.ReadAsStringAsync();
        var searchResults = JsonConvert.DeserializeObject<IEnumerable<SeriesSearchResult>>(responseString);
        return searchResults;
    }

    public async Task<SeriesResult> GetSeries(Guid id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(
            $"{_faybleApiConfiguration.BaseUrl}/api/series/{id}");

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "An error occurred while retrieving series from Fayble API: '{ReasonPhrase}' | {@Details}",
                response.ReasonPhrase,
                new
                {
                    Id = id,
                    StatusCode = (int)response.StatusCode,
                    Repsonse = response.ReasonPhrase
                });
            throw;
        }

        var responseString = await response.Content.ReadAsStringAsync();
        var seriesResult = JsonConvert.DeserializeObject<SeriesResult>(responseString);
        return seriesResult;
    }
}