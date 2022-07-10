using Fayble.Domain.Aggregates.Person;

namespace Fayble.Domain.Aggregates.Book;

public class BookPerson
{
    public Guid BookId { get; set; }

    public Guid PersonId { get; set; }
    public Book Book { get; set; }
    public Person.Person Person { get; set; }
    public RoleType Role { get; set; }

    public BookPerson(Guid bookId, Guid personId, RoleType role)
    {
        BookId = bookId;
        PersonId = personId;
        Role = role;
    }
}