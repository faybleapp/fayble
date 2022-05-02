using Fayble.Domain.Aggregates.SystemConfiguration;
using Fayble.Domain.Repositories;

namespace Fayble.Infrastructure.Repositories;

public class SystemConfigurationRepository : RepositoryBase<FaybleDbContext, SystemConfiguration, SystemConfigurationKey>, ISystemConfigurationRepository
{
    public SystemConfigurationRepository(FaybleDbContext context) : base(context)
    {
    }
}