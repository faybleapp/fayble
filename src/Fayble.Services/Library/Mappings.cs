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
            entity.FolderPath,
            new LibrarySettings(
                entity.GetSetting<bool>(LibrarySettingKey.ReviewOnImport),
                entity.GetSetting<bool>(LibrarySettingKey.UseComicInfo),
                entity.GetSetting<bool>(LibrarySettingKey.YearAsVolume)));
    }

    public static IEnumerable<LibrarySetting> ToEntity(this LibrarySettings settings)
    {
        var librarySettings = new List<LibrarySetting>
        {
            new(LibrarySettingKey.ReviewOnImport, settings.ReviewOnImport.ToString()),
            new(LibrarySettingKey.UseComicInfo, settings.UseComicInfo.ToString()),
            new(LibrarySettingKey.YearAsVolume, settings.YearAsVolume.ToString())
        };

        return librarySettings;
    }
}