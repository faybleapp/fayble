using System.Reflection;
using Fayble.Domain.Entities;

namespace Fayble.Domain.Aggregates.Book;

public class BookFieldLocks
{
    public Guid BookId { get; private set; }
    public bool Title { get; private set; } = false;
    public bool Summary { get; private set; } = false;
    public bool Number { get; private set; } = false;
    public bool Language { get; private set; } = false;
    public bool Rating { get; private set; } = false;
    public bool ReleaseDate { get; private set; } = false;
    public bool CoverDate { get; private set; } = false;
    public bool Tags { get; private set; } = false;
    public bool Authors { get; private set; } = false;
    public bool Writers { get; private set; } = false;
    public bool Inkers { get; private set; } = false;
    public bool Editors { get; private set; } = false;
    public bool Pencillers { get; private set; } = false;
    public bool Letterers { get; private set; } = false;
    public bool Colorists { get; private set; } = false;
    public bool CoverArtists { get; private set; } = false;
    public bool Translators { get; private set; } = false;
    public bool Other { get; private set; } = false;


    public BookFieldLocks() { }

    public BookFieldLocks(Guid bookId)
    {
        BookId = bookId;
    }
    
    public void UpdateLock(string field, bool locked)
    {
        var property = GetType().GetProperty(field);
        property.SetValue(this, locked);
    }
}