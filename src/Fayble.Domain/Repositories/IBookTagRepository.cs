using Fayble.Domain.Aggregates.Tag;

namespace Fayble.Domain.Repositories;

public interface IBookTagRepository : IRepositoryBase<BookTag, Guid>
{
    Task<BookTag?> GetByName(string name);
}
