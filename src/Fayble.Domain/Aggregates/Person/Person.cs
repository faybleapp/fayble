using Fayble.Domain.Aggregates.People;
using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.Person
{
    public class Person: IdentifiableEntity<Guid>
    {
        public string Name { get; private set; }
        public PersonType Type { get; private set; }

        public ICollection<Book.Book> Books { get; private set; }

        public Person(){}

        public Person(Guid id, string name, PersonType type) : base(id)
        {
            Name = name;
            Type = type;
        }
    }
}
