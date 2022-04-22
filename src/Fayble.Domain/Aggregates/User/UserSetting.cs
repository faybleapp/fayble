using Fayble.Domain.Aggregates.User;

namespace Fayble.Domain.Aggregates.User;

public class UserSetting
{
    public UserSettingKey Setting { get; private set; }
    public string Value { get; private set; }
    public Guid UserId { get; private set; }
    public virtual User User { get; private set; }

    public UserSetting()
    {
    }

    public UserSetting(UserSettingKey setting, string value)
    {
        Value = value;
        Setting = setting;
    }

    public void Update(string value)
    {
        Value = value;
    }

}
