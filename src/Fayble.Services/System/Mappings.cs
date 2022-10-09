using Fayble.Domain.Aggregates.SystemConfiguration;
using Fayble.Domain.Aggregates.SystemSetting;
using Fayble.Domain.Extensions;
using SystemSettings = Fayble.Models.SystemSettings;

namespace Fayble.Services.System;

internal static class Mappings
{
    public static SystemSettings ToModel(
        this IEnumerable<SystemSetting> entity)
    {
        return new SystemSettings(bool.Parse(entity.GetSetting(SystemSettingKey.FirstRun)));
    }

    public static IEnumerable<SystemSetting> ToEntity(
        this SystemSettings configuration)
    {
        return new List<SystemSetting>
        {
            new(SystemSettingKey.FirstRun, configuration.FirstRun.ToString())
        };
    }


}