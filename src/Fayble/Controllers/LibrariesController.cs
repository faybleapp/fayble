using Fayble.Models.Library;
using Fayble.Models.Series;
using Fayble.Security.Authorisation;
using Fayble.Services.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.Administrator)]
public class LibrariesController : ControllerBase
{
    private readonly ILibraryService _libraryService;

    public LibrariesController(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    [HttpGet]
    [Authorize(Policy = Policies.User)]
    public async Task<IEnumerable<Library>> GetAll()
    {
        return await _libraryService.GetAll();
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.User)]
    public async Task<Library> Get(Guid id)
    {
        return await _libraryService.Get(id);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Library library)
    {
        await _libraryService.Create(library);
        return Ok();
    }

    [HttpGet("{id}/series")]
    [Authorize(Policy = Policies.User)]
    public async Task<IEnumerable<Series>?> GetSeries(Guid id)
    {
        return await _libraryService.GetSeries(id);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<Library>> Update(Guid id, [FromBody] Library library)
    {
        return await _libraryService.Update(id, library);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Library>> Delete(Guid id)
    {
        await _libraryService.Delete(id);
        return NoContent();
    }
}