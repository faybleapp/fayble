using Fayble.Models.User;

namespace Fayble.Security;

public class Authentication
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public bool LoggedIn { get; set; }
    public User LoggedInUser { get; set; }
    public string RefreshToken { get; set; }

}
