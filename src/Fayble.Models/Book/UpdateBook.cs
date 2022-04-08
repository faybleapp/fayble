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
    public string? ReleaseDate { get; }
    public string? CoverDate { get; }
    public string? Language { get; }
    public string? Review { get; }
    public string[] Tags { get; }

    public UpdateBook(
        Guid id,
        bool locked,
        string? notes,
        string? number,
        decimal rating,
        string? releaseDate,
        string? coverDate,
        string? summary,
        string? title,
        string? language,
        string? review,
        string[] tags)
    {
        Id = id;
        Locked = locked;
        Notes = notes;
        Number = number;
        Rating = rating;
        ReleaseDate = releaseDate;
        CoverDate = coverDate;
        Summary = summary;
        Title = title;
        Language = language;
        Review = review;
        Tags = tags;
    }


}