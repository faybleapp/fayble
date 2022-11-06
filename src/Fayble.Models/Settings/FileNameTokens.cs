namespace Fayble.Models.Settings;

public static class FilenameTokens
{
    public const string SeriesName = "{Series Name}";
    public const string SeriesYear = "{Series Year}";
    public const string BookCoverDateShort = "{Book CoverDate:YYYY-MM}";
    public const string BookCoverDateLong = "{Book CoverDate:MMM YYYY}";
    public const string BookCoverDateLongComma = "{Book CoverDate:MMM, YYYY}";
    public const string BookCoverDateFull = "{Book CoverDate:MMMM YYYY}";
    public const string BookCoverDateFullComma = "{Book CoverDate:MMMM YYYY}";
    public const string SeriesVolume = "{Series Volume}";
    public const string BookNumberPadding = "{Book Number(.*?)}";
}