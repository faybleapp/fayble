using Fayble.Models;
using Fayble.Security.Authorisation;
using Fayble.Services.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.User)]
public class TagsController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagsController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet("booktags")]
    public async Task<IEnumerable<Tag>?> BookTags()
    {
        return await _tagService.GetAllBookTags();
    }
}