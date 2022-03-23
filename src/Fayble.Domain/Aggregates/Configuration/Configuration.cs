using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.Configuration;

public class Configuration : IdentifiableEntity<Setting>, IAggregateRoot
{
    public string Value { get; private set; }

    public Configuration()
    {
    }

    public Configuration(Setting setting, string value) : base(setting)
    {
        Value = value;
    }
}