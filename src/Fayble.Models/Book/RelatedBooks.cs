namespace Fayble.Models.Book;

public class RelatedBooks
{
    public List<Book>? BooksInSeries { get; set; }
    public List<Book>? BooksByPublisher { get; set; }
    public List<Book>? BooksByAuthor { get; set; }
    public List<Book>? BooksByWriter { get; set; }
    public List<Book>? BooksReleasedSameMonth { get; set; }
    public List<Book>? BooksReleasedSameYear { get; set; }
}