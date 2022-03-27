using Fayble.Models.Book;
using Fayble.Services.Book;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("{id}")]
    public async Task<Book?> Get(Guid id)
    {
        return await _bookService.Get(id);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<Book>> Update(Guid id, [FromBody] UpdateBook series)
    {
        return await _bookService.Update(id, series);
    }
}
