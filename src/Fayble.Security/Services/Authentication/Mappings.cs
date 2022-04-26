using Fayble.Security.Models;

namespace Fayble.Security.Services.Authentication;

public static class Mappings
{
    public static User ToModel(this Domain.Aggregates.User.User entity)
    {
        return new User(
            entity.Id,
            entity.UserName
        );
    }
}
