using Fayble.Domain.Aggregates.SystemConfiguration;
using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.SystemSetting;

public class SystemSetting : IdentifiableEntity<SystemSettingKey>, IAggregateRoot
{
    public string Value { get; private set; }

    public SystemSetting(){}

    public SystemSetting(SystemSettingKey setting, string value) : base(setting)
    {
        Value = value;
    }

    public void Update(string value)
    {
        Value = value;
    }
}