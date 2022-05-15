using Fayble.Domain.Aggregates.Series;
using Fayble.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fayble.Infrastructure.Repositories;

public class SeriesRepository : RepositoryBase<FaybleDbContext, Series, Guid>, ISeriesRepository
{
    public SeriesRepository(FaybleDbContext context) : base(context)
    {
    }

    protected override IQueryable<Series> GetWithIncludes()
    {
        return Context.Set<Series>()
            .Include(s => s.Books)
            .ThenInclude(s => s.ReadHistory)
            .Include(s => s.Books)
            .ThenInclude(s => s.File)
            
            // TODO: Include when user identity is setup
            //.ThenInclude(s => s.User)
            .Include(s => s.Library)
            .ThenInclude(l => l.Settings)
            .Include(s => s.Library)
            .Include(s => s.ParentSeries)
            .Include(s => s.Publisher);
    }
}
