using Fayble.Domain.Aggregates.FileType;
using Fayble.Domain.Repositories;

namespace Fayble.Infrastructure.Repositories;

public class FileTypeRepository : RepositoryBase<FaybleDbContext, FileType, Guid>, IFileTypeRepository
{
    public FileTypeRepository(FaybleDbContext context) : base(context)
    {
    }
}