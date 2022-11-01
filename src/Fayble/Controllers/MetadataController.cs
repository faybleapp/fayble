using Fayble.Models.Metadata;
using Fayble.Security.Authorisation;
using Fayble.Services.MetadataService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.User)]
public class MetadataController : ControllerBase
{
    private readonly IMetadataService _metadataService;

    public MetadataController(IMetadataService metadataService)
    {
        _metadataService = metadataService;
    }

    [HttpGet("searchseries")]
    public async Task<IEnumerable<SeriesSearchResult>> SearchSeries([FromQuery] string searchQuery)
    {
        return await _metadataService.SearchSeries(searchQuery);
    }

    [HttpGet("series/{id}")]
    public async Task<SeriesResult> Series(Guid id)
    {
        return await _metadataService.GetSeries(id);
    }

    [HttpGet("book/{id}")]
    public async Task<BookResult> Book(Guid id)
    {
        return await _metadataService.GetBook(id);
    }
}
