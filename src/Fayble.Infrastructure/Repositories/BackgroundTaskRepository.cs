using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Domain.Repositories;

namespace Fayble.Infrastructure.Repositories;

public class BackgroundTaskRepository : RepositoryBase<FaybleDbContext, BackgroundTask, Guid>, IBackgroundTaskRepository
{
    public BackgroundTaskRepository(FaybleDbContext context) : base(context)
    {
    }
}