using Fayble.Models.Metadata;
using Fayble.Security.Authorisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.User)]
public class MetadataController : ControllerBase
{
    [HttpGet("searchseries")]
    public async Task<IEnumerable<SeriesSearchResult>> SearchSeries([FromQuery] string name, [FromQuery] int? year)
    {
        return await Task.FromResult(new List<SeriesSearchResult>());
    }
}