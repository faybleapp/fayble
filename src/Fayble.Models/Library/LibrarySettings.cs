namespace Fayble.Models.Library;

public class LibrarySettings
{
    public bool ReviewOnImport { get; }
    public bool UseComicInfo { get;  }

    public LibrarySettings(bool reviewOnImport, bool useComicInfo)
    {
        ReviewOnImport = reviewOnImport;
        UseComicInfo = useComicInfo;
    }
}
