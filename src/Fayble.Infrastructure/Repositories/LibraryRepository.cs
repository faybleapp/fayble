using System.Globalization;
using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fayble.Infrastructure.Repositories;

public class LibraryRepository : RepositoryBase<FaybleDbContext, Library, Guid>, ILibraryRepository
{
    public LibraryRepository(FaybleDbContext context) : base(context)
    {
    }

    protected override IQueryable<Library> GetWithIncludes()
    {
        return base.GetWithIncludes()
            .Include(l => l.Settings);

    }
}