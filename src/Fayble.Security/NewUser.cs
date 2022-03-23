namespace Fayble.Security;

public class NewUser
{
    public string Username { get; }
    public string Password { get; }
    public string Name { get; }
    public bool Admin { get; }

    public NewUser(string username, string password, string name, bool admin)
    {
        Username = username;
        Password = password;
        Name = name;
        Admin = admin;
    }
}