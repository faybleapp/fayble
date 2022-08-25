using System.Text;
using System.Windows.Markup;
using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.SystemConfiguration;

public class SystemSetting : IdentifiableEntity<SystemSettingKey>, IAggregateRoot
{
    public string Value { get; private set; }

    public SystemSetting()
    {
    }

    public SystemSetting(SystemSettingKey configuration, string value) : base(configuration)
    {
        Value = value;
    }

    public void Update(string value)
    {
        Value = value;
    }
}