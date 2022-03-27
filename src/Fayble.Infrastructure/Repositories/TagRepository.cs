using Fayble.Domain.Aggregates.FileType;
using Fayble.Domain.Aggregates.Tag;
using Fayble.Domain.Repositories;

namespace Fayble.Infrastructure.Repositories;

public class TagRepository : RepositoryBase<FaybleDbContext, Tag, Guid>, ITagRepository
{
    public TagRepository(FaybleDbContext context) : base(context)
    {
    }
}