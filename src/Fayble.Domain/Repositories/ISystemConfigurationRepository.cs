using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Aggregates.SystemConfiguration;
using Fayble.Domain.Aggregates.SystemSetting;

namespace Fayble.Domain.Repositories;

public interface ISystemConfigurationRepository : IRepositoryBase<SystemSetting, SystemSettingKey>
{

}
