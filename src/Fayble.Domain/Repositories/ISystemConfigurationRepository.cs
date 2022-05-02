using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Aggregates.SystemConfiguration;

namespace Fayble.Domain.Repositories;

public interface ISystemConfigurationRepository : IRepositoryBase<SystemConfiguration, SystemConfigurationKey>
{

}
