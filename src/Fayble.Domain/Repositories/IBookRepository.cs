using Fayble.Domain.Aggregates.Book;

namespace Fayble.Domain.Repositories;

public interface IBookRepository : IRepositoryBase<Book, Guid>
{
}