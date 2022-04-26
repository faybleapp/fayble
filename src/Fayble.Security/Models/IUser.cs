using System.Security.Principal;

namespace Fayble.Security.Models;

public interface IUser
{
    bool IsAuthenticated { get; }
    Guid Id { get; }
    string Username { get; }
}


