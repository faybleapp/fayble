namespace Fayble.Models.Settings;

public class MissingTokenReplacement
{
    public const string Empty = "";
    public const string Unknown = "Unknown";
    public const string TBA = "TBA";
    public const string TBD = "TBD";

    public static string? GetMissingTokenReplacementValue(string tokenReplacement) =>
        typeof(MissingTokenReplacement).GetField(tokenReplacement)?.GetValue(null)?.ToString();
}