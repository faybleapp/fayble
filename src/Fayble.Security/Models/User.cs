using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Fayble.Security.Models;

public class User : IUser
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string? Role { get; set; }

    public User(IHttpContextAccessor _httpContextAccessor)
    {
        var principal = _httpContextAccessor.HttpContext?.User;

        if (principal?.Identity?.Name == null)
        {
            Id = Guid.Empty;
            Username = "Fayble";
        }
        else
        {
            var identity = principal.Identity as ClaimsIdentity;
            Role = identity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value ?? string.Empty;
            Id = new Guid(identity?.Claims.FirstOrDefault(x => x.Type == "id")?.Value ?? string.Empty);
            Username = identity?.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
        }
    }

    public User(Guid id, string username, string? role = null)
    {
        Id = id;
        Role = role;
        Username = username;
    }
}
