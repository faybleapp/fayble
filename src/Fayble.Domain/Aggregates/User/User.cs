using Microsoft.AspNetCore.Identity;

namespace Fayble.Domain.Aggregates.User;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; }

    private readonly List<UserSetting> _settings = new();
    public virtual IReadOnlyCollection<UserSetting> Settings => _settings;

    public User()
    {
    }

    public User(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}