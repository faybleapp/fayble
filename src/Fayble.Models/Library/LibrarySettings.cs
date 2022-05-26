namespace Fayble.Models.Library;

public class LibrarySettings
{
    public bool ReviewOnImport { get; }
    public bool UseComicInfo { get; }
    public bool YearAsVolume { get; }

    public LibrarySettings(bool reviewOnImport, bool useComicInfo, bool yearAsVolume)
    {
        ReviewOnImport = reviewOnImport;
        UseComicInfo = useComicInfo;
        YearAsVolume = yearAsVolume;
    }
}
