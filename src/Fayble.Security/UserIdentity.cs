using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Fayble.Security;

public interface IUserIdentity
{
    bool IsAuthenticated { get; }
    Guid Id { get; }
    string? UserName { get; }
    string? Name { get; }
}

public class UserIdentity : IUserIdentity
{
    public bool IsAuthenticated { get; set; }
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Name { get; set; }

    public UserIdentity(IHttpContextAccessor _httpContextAccessor)
    {
        var principal = _httpContextAccessor.HttpContext?.User;

        if (principal?.Identity?.Name == null)
        {
            Id = Guid.Empty;
            Name = "Fayble";
            UserName = "Fayble";
        }
        else
        {
            var identity = principal.Identity as ClaimsIdentity;
            IsAuthenticated = principal.Identity?.IsAuthenticated ?? false;
            Name = principal?.Identity?.Name;
            Id = new Guid(identity?.Claims.FirstOrDefault(x => x.Type == "id")?.Value ?? string.Empty);
            UserName = identity?.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
        }
    }

    public UserIdentity(string username)
    {
        Id = Guid.Empty;
        Name = "Fayble";
        UserName = "Fayble";
    }
}