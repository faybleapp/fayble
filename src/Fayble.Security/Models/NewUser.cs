namespace Fayble.Security.Models;

public class NewUser
{
    public string Username { get; }
    public string Password { get; }
    public string Role { get; }

    public NewUser(string username, string password, string role)
    {
        Username = username;
        Password = password;
        Role = role;
    }
}