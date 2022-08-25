namespace Fayble.Models.Metadata;

public class BookResult
{
    public Guid Id { get; }
    public string Title { get; }
    public string Number { get; }

    public BookResult(Guid id, string number, string title)
    {
        Id = id;
        Number = number;
        Title = title;
    }
}