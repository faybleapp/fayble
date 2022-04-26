using Fayble.Domain.Aggregates.Library;
using Fayble.Models.Library;

namespace Fayble.Services.Library;

internal static class Mappings
{
    public static Models.Library.Library ToModel(this Domain.Aggregates.Library.Library entity)
    {
        return new Models.Library.Library(
            entity.Id,
            entity.Name,
            entity.Type.ToString(),
            entity.Paths.Select(p => p.Path).ToList(),
            new LibrarySettings(bool.Parse(entity.GetSetting(LibrarySettingKey.ReviewOnImport))));
    }

    public static List<LibrarySetting> ToEntity(this LibrarySettings settings)
    {
        var librarySettings = new List<LibrarySetting>
            {new(LibrarySettingKey.ReviewOnImport, settings.ReviewOnImport.ToString())};

        return librarySettings;
    }
}