using Fayble.Domain.Aggregates.SystemConfiguration;
using Fayble.Domain.Repositories;

namespace Fayble.Infrastructure.Repositories;

public class SystemConfigurationRepository : RepositoryBase<FaybleDbContext, SystemSetting, SystemSettingKey>, ISystemConfigurationRepository
{
    public SystemConfigurationRepository(FaybleDbContext context) : base(context)
    {
    }
}