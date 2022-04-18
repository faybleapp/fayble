using Fayble.Models.Book;

namespace Fayble.Services.Book;

public interface IBookService
{
    Task<Models.Book.Book?> Get(Guid id);
    Task<Models.Book.Book> Update(Guid id, UpdateBook book);
    Task<RelatedBooks> GetRelated(Guid id);
}