
using Fayble.Core.Helpers;

namespace Fayble.Models;

public class Media
{
    public string? CoverFull { get; }
    public string? CoverMed { get; }
    public string? CoverSm { get; }

    public Media()
    {
    }

    public Media(string mediaPath)
    {
        //TODO: Blank cover image/placeholder
        var blankCover = "";

        //TODO: Do we need three sizes?
        var coverFull = Path.Combine(ApplicationHelpers.GetAppDirectory(), mediaPath, "cover.jpg");
        var coverMed = Path.Combine(ApplicationHelpers.GetAppDirectory(), mediaPath, "cover-500.jpg");
        var coverSm = Path.Combine(ApplicationHelpers.GetAppDirectory(), mediaPath, "cover-250.jpg");

        CoverFull = File.Exists(coverFull) ? Path.Combine(mediaPath, "cover.jpg") : "blankCover";
        CoverMed = File.Exists(coverMed) ? Path.Combine(mediaPath, "cover-500.jpg") : blankCover;
        CoverSm = File.Exists(coverSm) ? Path.Combine(mediaPath, "cover-250.jpg") : blankCover;
    }


}
