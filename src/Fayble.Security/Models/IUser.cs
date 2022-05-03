using System.Security.Principal;

namespace Fayble.Security.Models;

public interface IUser
{
    string Role{ get; }
    Guid Id { get; }
    string Username { get; }
}


