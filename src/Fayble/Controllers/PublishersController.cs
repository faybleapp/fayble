using Fayble.Models;
using Fayble.Security.Authorisation;
using Fayble.Services.Publisher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.User)]
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