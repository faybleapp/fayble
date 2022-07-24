using Fayble.Domain.Aggregates.SystemConfiguration;

namespace Fayble.Domain.Extensions;

public static class SystemConfiguration
{
    public static string GetSetting(
        this IEnumerable<Aggregates.SystemConfiguration.SystemSetting> entity,
        SystemSettingKey configuration)
    {
        return entity.First(s => s.Id == configuration).Value;
    }
}