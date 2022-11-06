using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.MediaSetting;

public class MediaSetting : IdentifiableEntity<MediaSettingKey>, IAggregateRoot
{
    public string Value { get; private set; }

    public MediaSetting(){}

    public MediaSetting(MediaSettingKey setting, string value) : base(setting)
    {
        Value = value;
    }

    public void Update(string value)
    {
        Value = value;
    }
}