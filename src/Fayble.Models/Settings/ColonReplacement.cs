namespace Fayble.Models.Settings;

public class ColonReplacement
{
    public const string Dash = "-";
    public const string SpaceDash = " -";
    public const string SpaceDashSpace = " - ";

    public static string? GetColonReplacementValue(string colonReplacement) =>
        typeof(ColonReplacement).GetField(colonReplacement)?.GetValue(null)?.ToString();
}