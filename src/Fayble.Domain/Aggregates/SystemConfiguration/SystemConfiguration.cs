using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.SystemConfiguration;

public class SystemConfiguration : IdentifiableEntity<SystemConfigurationKey>, IAggregateRoot
{
    public SystemConfigurationKey Configuration { get; private set; }
    public string Value { get; private set; }

    public SystemConfiguration()
    {
    }

    public SystemConfiguration(SystemConfigurationKey configuration, string value) : base(configuration)
    {
        Value = value;
    }
}