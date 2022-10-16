namespace Fayble.Models.Settings;

public class MediaSettings
{
    public string BookNamingConvention { get; private set; }
    public string ComicBookStandardNamingConvention { get; private set; }
    public string ComicBookOneShotNamingConvention { get; private set; }
    public string ColonReplacement { get; private set; }
    public bool RenameFiles { get; private set; }

    public MediaSettings(
        string bookNamingConvention,
        string comicBookStandardNamingConvention,
        string comicBookOneShotNamingConvention,
        string colonReplacement,
        bool renameFiles)
    {
        BookNamingConvention = bookNamingConvention;
        ComicBookStandardNamingConvention = comicBookStandardNamingConvention;
        ComicBookOneShotNamingConvention = comicBookOneShotNamingConvention;
        ColonReplacement = colonReplacement;
        RenameFiles = renameFiles;
    }
}