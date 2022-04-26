using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Fayble.Security.Models;

public class User : IUser
{
    public Guid Id { get; set; }
    public string Username { get;  set; }
    public bool IsAuthenticated { get; set; }


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
            IsAuthenticated = principal.Identity?.IsAuthenticated ?? false;
            Id = new Guid(identity?.Claims.FirstOrDefault(x => x.Type == "id")?.Value ?? string.Empty);
            Username = identity?.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
        }
    }

    public User(Guid id, string username, bool isAuthenticated = false)
    {
        Id = id;
        IsAuthenticated = isAuthenticated;
        Username = username;
    }
}
