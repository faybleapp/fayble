using Fayble.Models.User;

namespace Fayble.Services.Authentication;

public static class Mappings
{
    public static User ToModel(this Domain.Aggregates.User.User entity)
    {
        return new User(
            entity.Id,
            entity.Name,
            entity.UserName
        );
    }
}
