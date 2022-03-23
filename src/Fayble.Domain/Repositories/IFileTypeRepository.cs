using Fayble.Domain.Aggregates.FileType;

namespace Fayble.Domain.Repositories;

public interface IFileTypeRepository : IRepositoryBase<FileType, Guid>
{
}