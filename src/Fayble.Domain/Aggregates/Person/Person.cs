using Fayble.Domain.Aggregates.Book;
using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.Person
{
    public class Person: IdentifiableEntity<Guid>, IAggregateRoot
    {
        public string Name { get; private set; }
        private readonly List<BookPerson> _books = new();
        public virtual IReadOnlyCollection<BookPerson> Books => _books;

        public Person(){}

        public Person(Guid id, string name) : base(id)
        {
            Name = name;
        }
    }
}
