using System.Resources;

namespace Fayble.Models.Book;

public class 
    UpdateBook
{
    public Guid Id { get; }
    public string? Title { get; }
    public string? Summary { get; }
    public string? Notes { get; }
    public bool Locked { get; }
    public decimal Rating { get; }
    public string? Number { get; }
    public string? CoverDate { get; }
    public string? StoreDate { get; }
    public string? Language { get; }
    public string? Review { get; }
    public string[] Tags { get; }

    public UpdateBook(
        Guid id,
        string? coverDate,
        bool locked,
        string? notes,
        string? number,
        decimal rating,
        string? storeDate,
        string? summary,
        string? title,
        string? language,
        string? review,
        string[] tags)
    {
        CoverDate = coverDate;
        Id = id;
        Locked = locked;
        Notes = notes;
        Number = number;
        Rating = rating;
        StoreDate = storeDate;
        Summary = summary;
        Title = title;
        Language = language;
        Review = review;
        Tags = tags;
    }


}