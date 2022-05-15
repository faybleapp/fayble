namespace Fayble.Models.Book;

public class BookPerson : Person
{
    public string Role { get; private set; }

    public BookPerson(Guid id, string name, string role):  base(id, name)
    {
        Role = role;
    }
}