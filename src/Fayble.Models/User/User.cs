namespace Fayble.Models.User;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Username { get; private set; }

    public User(Guid id, string name, string username)
    {
        Id = id;
        Name = name;
        Username = username;
    }
}
