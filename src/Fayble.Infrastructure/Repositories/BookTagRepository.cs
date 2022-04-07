using Fayble.Domain.Aggregates.Tag;
using Fayble.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fayble.Infrastructure.Repositories;

public class BookTagRepository : RepositoryBase<FaybleDbContext, BookTag, Guid>, IBookTagRepository
{
    public BookTagRepository(FaybleDbContext context) : base(context)
    {
    }

    public async Task<BookTag?> GetByName(string name)
    {
        return await GetWithIncludes().FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
    }
}