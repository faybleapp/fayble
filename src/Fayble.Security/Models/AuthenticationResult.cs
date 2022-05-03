using Fayble.Security.Models;

namespace Fayble.Security.Models;

public class AuthenticationResult
{
    public string Token { get; private set; }
    public DateTime Expiration { get; private set; }
    public bool IsAuthenticated { get; private set; }
    public Guid UserId { get; private set; }
    public string RefreshToken { get; private set; }

    public AuthenticationResult(Guid userId, DateTime expiration, string token, bool isAuthenticated, string refreshToken)
    {
        Expiration = expiration;
        IsAuthenticated = isAuthenticated;
        UserId = userId;
        RefreshToken = refreshToken;
        Token = token;
    }
}
