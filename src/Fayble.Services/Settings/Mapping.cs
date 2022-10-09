using Fayble.Domain.Aggregates.MediaSetting;
using Fayble.Models.Settings;

namespace Fayble.Services.Settings;

public static class Mapping
{
    public static MediaSettings ToModel(this List<MediaSetting> mediaSettings)
    {
        return new MediaSettings(
                mediaSettings.First(s => s.Id == MediaSettingKey.BookNamingConvention).Value,
                mediaSettings.First(s => s.Id == MediaSettingKey.ComicBookStandardNamingConvention).Value,
                mediaSettings.First(s => s.Id == MediaSettingKey.ComicBookOneShotNamingConvention).Value,
                mediaSettings.First(s => s.Id == MediaSettingKey.SeriesFolderFormat).Value,
                bool.Parse(mediaSettings.First(s => s.Id == MediaSettingKey.ReplaceIllegalCharacters).Value),
                bool.Parse(mediaSettings.First(s => s.Id == MediaSettingKey.RenameFiles).Value));
    }
}