using Fayble.Security.Models;

namespace Fayble.Security.Models;

public class AuthenticationResult
{
    public string Token { get; private set; }
    public DateTime Expiration { get; private set; }
    public bool LoggedIn { get; private set; }
    public User LoggedInUser { get; private set; }
    public string RefreshToken { get; private set; }

    public AuthenticationResult(User loggedInUser, DateTime expiration, string token, bool loggedIn, string refreshToken)
    {
        Expiration = expiration;
        LoggedIn = loggedIn;
        LoggedInUser = loggedInUser;
        RefreshToken = refreshToken;
        Token = token;
    }
}
