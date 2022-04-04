using Fayble.Domain.Aggregates.RefreshToken;
using Fayble.Domain.Aggregates.Tag;

namespace Fayble.Domain.Repositories;

public interface ITagRepository : IRepositoryBase<Tag, Guid>
{
    Task<Tag?> GetByName(string name);
}
