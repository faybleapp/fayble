using Fayble.Domain.Aggregates.RefreshToken;
using Fayble.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fayble.Infrastructure.Repositories;

public class RefreshTokenRepository : RepositoryBase<FaybleDbContext, RefreshToken, Guid>, IRefreshTokenRepository
{
    public RefreshTokenRepository(FaybleDbContext context) : base(context)
    {
    }

    protected override IQueryable<RefreshToken> GetWithIncludes()
    {
        return Context.Set<RefreshToken>()
            .Include(x => x.User);
    }
}
