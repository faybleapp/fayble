namespace Fayble.Domain.Aggregates.Tag
{
    public class BookTag : Tag
    {
        public ICollection<Book.Book> Books { get; private set; }

        public BookTag() { }

        public BookTag(Guid id, string name) : base(id)
        {
            Name = name;
        }
    }
}
