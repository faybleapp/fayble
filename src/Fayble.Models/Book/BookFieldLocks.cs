namespace Fayble.Models.Book;

public class BookFieldLocks
{
    public bool Title { get; }
    public bool Summary { get; }
    public bool Number { get; }
    public bool Language { get; }
    public bool Rating { get; }
    public bool ReleaseDate { get; }
    public bool CoverDate { get; }
    public bool Tags { get; }


    public BookFieldLocks(
        bool coverDate,
        bool language,
        bool number,
        bool rating,
        bool releaseDate,
        bool summary,
        bool title,
        bool tags)
    {
        CoverDate = coverDate;
        Language = language;
        Number = number;
        Rating = rating;
        ReleaseDate = releaseDate;
        Summary = summary;
        Title = title;
        Tags = tags;
    }
}