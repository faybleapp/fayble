namespace Fayble.Models.Library;

public class LibrarySettings
{
    public bool ReviewOnImport { get;  }

    public LibrarySettings(bool reviewOnImport)
    {
        ReviewOnImport = reviewOnImport;
    }
}
