using Fayble.Domain.Aggregates.BackgroundTask;

namespace Fayble.Domain.Repositories;

public interface IBackgroundTaskRepository : IRepositoryBase<BackgroundTask, Guid>
{
}