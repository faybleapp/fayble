using Fayble.Core.Exceptions;
using Fayble.Domain.Aggregates.FileType;
using Fayble.Domain.Aggregates.Tag;
using Fayble.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fayble.Infrastructure.Repositories;

public class TagRepository : RepositoryBase<FaybleDbContext, Tag, Guid>, ITagRepository
{
    public TagRepository(FaybleDbContext context) : base(context)
    {
    }

    public async Task<Tag?> GetByName(string name)
    {
        return await GetWithIncludes().FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
    }
}