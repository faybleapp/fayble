namespace Fayble.Models.Library;

public class LibrarySettings
{
    public bool ReviewOnImport { get; }
    public bool SeriesFolders { get;  }

    public LibrarySettings(bool reviewOnImport, bool seriesFolders)
    {
        ReviewOnImport = reviewOnImport;
        SeriesFolders = seriesFolders;
    }
}
