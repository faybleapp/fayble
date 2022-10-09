namespace Fayble.Models.Settings;

public class MediaSettings
{
    public string BookNamingConvention { get; }
    public string ComicBookStandardNamingConvention { get; }
    public string ComicBookOneShotNamingConvention { get; }
    public string SeriesFolderFormat { get; }
    public bool ReplaceIllegalCharacters { get; }
    public bool RenameFiles { get; }

    public MediaSettings(
        string bookNamingConvention,
        string comicBookStandardNamingConvention,
        string comicBookOneShotNamingConvention,
        string seriesFolderFormat,
        bool replaceIllegalCharacters,
        bool renameFiles)
    {
        BookNamingConvention = bookNamingConvention;
        ComicBookStandardNamingConvention = comicBookStandardNamingConvention;
        ComicBookOneShotNamingConvention = comicBookOneShotNamingConvention;
        SeriesFolderFormat = seriesFolderFormat;
        ReplaceIllegalCharacters = replaceIllegalCharacters;
        RenameFiles = renameFiles;
    }
}