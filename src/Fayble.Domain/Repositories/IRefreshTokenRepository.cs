using Fayble.Domain.Aggregates.RefreshToken;

namespace Fayble.Domain.Repositories;

public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken, Guid>
{
}
