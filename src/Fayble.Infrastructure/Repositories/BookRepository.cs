using Fayble.Domain.Aggregates.Book;
using Fayble.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fayble.Infrastructure.Repositories;

public class BookRepository : RepositoryBase<FaybleDbContext, Book, Guid>, IBookRepository
{
    public BookRepository(FaybleDbContext context) : base(context)
    {
    }

    protected override IQueryable<Book> GetWithIncludes()
    {
        return Context.Set<Book>()
            .Include(b => b.Series)
            .Include(b => b.Library)
            .ThenInclude(l => l.Settings);

    }
}