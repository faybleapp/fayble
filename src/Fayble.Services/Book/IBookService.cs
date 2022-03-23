namespace Fayble.Services.Book;

public interface IBookService
{
    Task<Models.Book.Book?> Get(Guid id);
}