using Fayble.Models;
using Fayble.Services.Tag;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
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