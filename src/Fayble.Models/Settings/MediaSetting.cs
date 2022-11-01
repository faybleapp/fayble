namespace Fayble.Models.Settings;

public class MediaSettings
{
    public string ComicBookStandardNamingFormat{ get; private set; }
    public string ComicBookOneShotNamingFormat{ get; private set; }
    public string ColonReplacement { get; private set; }
    public string MissingTokenReplacement { get; private set; }
    public bool RenameFiles { get; private set; }

    public MediaSettings(
        string comicBookStandardNamingFormat,
        string comicBookOneShotNamingFormat,
        string colonReplacement,
        string missingTokenReplacement,
        bool renameFiles)
    {
        ComicBookStandardNamingFormat = comicBookStandardNamingFormat;
        ComicBookOneShotNamingFormat = comicBookOneShotNamingFormat;
        ColonReplacement = colonReplacement;
        MissingTokenReplacement = missingTokenReplacement;
        RenameFiles = renameFiles;
    }
}