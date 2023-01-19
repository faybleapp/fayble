using System.Resources;

namespace Fayble.Models.Book;

public class 
    UpdateBook
{
    public Guid Id { get; }
    public string? Title { get; }
    public string? Summary { get; }
    public string? Notes { get; }
    public decimal Rating { get; }
    public string? Number { get; }
    public string? ReleaseDate { get; }
    public string? CoverDate { get; }
    public string? Language { get; }
    public string? Review { get; }
    public Guid? MatchId { get; }
    public string[] Tags { get; }
    public BookFieldLocks FieldLocks { get; }
    public List<BookPerson> People { get; }

    public UpdateBook(
        Guid id,
        string? notes,
        string? number,
        decimal rating,
        string? releaseDate,
        string? coverDate,
        string? summary,
        string? title,
        string? language,
        string? review,
        Guid? matchId,
        string[] tags,
        List<BookPerson> people,
        BookFieldLocks fieldLocks)
    {
        Id = id;
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
        People = people;
        FieldLocks = fieldLocks;
        MatchId = matchId;
    }


}