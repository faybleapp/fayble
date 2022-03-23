using Microsoft.AspNetCore.Identity;

namespace Fayble.Domain.Aggregates.User;

public class User : IdentityUser<Guid>
{
    public User()
    {
        Id = Guid.NewGuid();
    }

    public string Name { get; set; }
}