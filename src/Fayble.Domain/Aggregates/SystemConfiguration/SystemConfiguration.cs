using System.Text;
using System.Windows.Markup;
using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.SystemConfiguration;

public class SystemConfiguration : IdentifiableEntity<SystemConfigurationKey>, IAggregateRoot
{
    public string Value { get; private set; }

    public SystemConfiguration()
    {
    }

    public SystemConfiguration(SystemConfigurationKey configuration, string value) : base(configuration)
    {
        Value = value;
    }

    public void Update(string value)
    {
        Value = value;
    }
}