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
            .ThenInclude(s => s.FieldLocks)
            .Include(b => b.Library)
            .ThenInclude(l => l.Settings)
            .Include(b => b.Tags)
            .Include(b => b.FieldLocks)
            .Include(b => b.File)
            .Include(b => b.People)
            .ThenInclude(p => p.Person)
            .Include(b => b.File);
            
    }
}