using Fayble.Domain.Aggregates.SystemConfiguration;
using Fayble.Domain.Extensions;
using SystemConfiguration = Fayble.Models.SystemConfiguration;

namespace Fayble.Services.System;

internal static class Mappings
{
    public static SystemConfiguration ToModel(
        this IEnumerable<Domain.Aggregates.SystemConfiguration.SystemConfiguration> entity)
    {
        return new SystemConfiguration(bool.Parse(entity.GetSetting(SystemConfigurationKey.FirstRun)));
    }

    public static IEnumerable<Domain.Aggregates.SystemConfiguration.SystemConfiguration> ToEntity(
        this SystemConfiguration configuration)
    {
        return new List<Domain.Aggregates.SystemConfiguration.SystemConfiguration>
        {
            new(SystemConfigurationKey.FirstRun, configuration.FirstRun.ToString())
        };
    }


}