
using System.Text.RegularExpressions;

namespace Fayble.Core.Extensions;
public static class StringExtensions
{
    public static string RemoveIllegalCharacters(this string text)
    {
        text = text.Replace(":", " -");
        text = text.Replace("/", " - ");

        var invalidChars = Path.GetInvalidFileNameChars();

        return new string(text
            .Where(x => !invalidChars.Contains(x))
            .ToArray());
    }

    public static string TrimPathSeparators(this string path)
    {
        path = path.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        path = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        return path;
    }

    public static string? Sanitise(this string value)
    {
        return string.IsNullOrWhiteSpace(value?.Trim()) ? null : value.Trim();
    }

    public static string RemoveHtmlTags(this string? text)
    {
        if (text == null)
            return text;

        text = text.Replace("<h1>", "\r\n\r\n");
        text = text.Replace("<h2>", "\r\n\r\n");
        text = text.Replace("<h3>", "\r\n\r\n");
        text = text.Replace("<h4>", "\r\n\r\n");
        text = text.Replace("<strong>", "\r\n\r\n");
        text = text.Replace("<li>", "\r\n - ");

        var rx = new Regex("<[^>]*>");

        text = rx.Replace(text, "");

        return text;
    }

}