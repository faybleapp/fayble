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
    public bool Authors { get; }
    public bool Writers { get; }
    public bool Inkers { get; }
    public bool Editors { get; }
    public bool Pencillers { get; }
    public bool Letterers { get; }
    public bool Colorists { get; }
    public bool CoverArtists { get; }
    public bool Translators { get; }
    public bool Other { get; }

    public BookFieldLocks(
        bool title,
        bool summary,
        bool number,
        bool language,
        bool rating,
        bool releaseDate,
        bool coverDate,
        bool tags,
        bool authors,
        bool writers,
        bool inkers,
        bool editors,
        bool pencillers,
        bool letterers,
        bool colorists,
        bool coverArtists,
        bool translators,
        bool other)
    {
        Title = title;
        Summary = summary;
        Number = number;
        Language = language;
        Rating = rating;
        ReleaseDate = releaseDate;
        CoverDate = coverDate;
        Tags = tags;
        Authors = authors;
        Writers = writers;
        Inkers = inkers;
        Editors = editors;
        Pencillers = pencillers;
        Letterers = letterers;
        Colorists = colorists;
        CoverArtists = coverArtists;
        Translators = translators;
        Other = other;
    }
}