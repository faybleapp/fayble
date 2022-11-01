namespace Fayble.Models.Metadata;

public class BookSummaryResult
{
    public Guid Id { get; }
    public string Title { get; }
    public string Number { get; }

    public BookSummaryResult(Guid id, string number, string title)
    {
        Id = id;
        Number = number;
        Title = title;
    }
}