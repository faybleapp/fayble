using Fayble.Domain.Aggregates.SystemConfiguration;
using Fayble.Domain.Aggregates.SystemSetting;

namespace Fayble.Domain.Extensions;

public static class SystemConfiguration
{
    public static string GetSetting(this IEnumerable<SystemSetting> entity, SystemSettingKey configuration)
    {
        return entity.First(s => s.Id == configuration).Value;
    }
}