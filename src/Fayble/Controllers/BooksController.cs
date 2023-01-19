using Fayble.Models.Book;
using Fayble.Security.Authorisation;
using Fayble.Services.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.Administrator)]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("{id}")]
    [Authorize(Policy = Policies.User)]
    public async Task<Book?> Get(Guid id)
    {
        return await _bookService.Get(id);
    }

    [HttpPatch("{id}")]
    public async Task<Book> Update(Guid id, [FromBody] UpdateBook series)
    {
        return await _bookService.Update(id, series);
    }

    [Authorize(Policy = Policies.User)]
    [HttpGet("{id}/related")]
    public async Task<RelatedBooks> GetRelated(Guid id)
    {
        return await _bookService.GetRelated(id);
    }

    [HttpPost("{id}/refresh-metadata")]
    public async Task<ActionResult> RefreshMetadata(Guid id)
    {
        //await _bookService.RefreshMetadata(id);
        return Accepted();
    }
}
