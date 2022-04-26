using Microsoft.AspNetCore.Identity;

namespace Fayble.Domain.Aggregates.User;

public class User : IdentityUser<Guid>
{
    private readonly List<UserSetting> _settings = new();
    public virtual IReadOnlyCollection<UserSetting> Settings => _settings;
    
    public User()
    {
    }

    
    public User(Guid id, string username, string password)
    {
        Id = id;
        UserName = username;
        UpdatePassword(password);
    }

    public void UpdatePassword(string password)
    {
        var hasher = new PasswordHasher<User>();
        PasswordHash = hasher.HashPassword(this, password);
        SecurityStamp = Guid.NewGuid().ToString();
    }
}