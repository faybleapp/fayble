using Fayble.Models.Publisher;
using Fayble.Models.Tag;
using Fayble.Services.Publisher;
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

    [HttpGet]
    public async Task<IEnumerable<Tag>?> GetAll()
    {
        return await _tagService.GetAll();
    }
}