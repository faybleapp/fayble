using Fayble.Models.Publisher;
using Fayble.Services.Publisher;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PublishersController : ControllerBase
{
    private readonly IPublisherService _publisherService;

    public PublishersController(IPublisherService publisherService)
    {
        _publisherService = publisherService;
    }

    [HttpGet("{id}")]
    public async Task<Publisher?> Get(Guid id)
    {
        return await _publisherService.Get(id);
    }

    [HttpGet]
    public async Task<IEnumerable<Publisher>?> GetAll()
    {
        return await _publisherService.GetAll();
    }
}