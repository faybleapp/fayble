namespace Fayble.Security.Models;

public class RefreshTokenRequest
{
    public string RefreshToken { get; private set; }

    public RefreshTokenRequest(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}