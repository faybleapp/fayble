using Fayble.Domain.Aggregates.MediaSetting;
using Fayble.Models.Settings;

namespace Fayble.Services.Settings;

public static class Mapping
{
    public static MediaSettings ToModel(this List<MediaSetting> mediaSettings)
    {
        return new MediaSettings(
            mediaSettings.First(s => s.Id == MediaSettingKey.ComicBookStandardNamingFormat).Value,
                mediaSettings.First(s => s.Id == MediaSettingKey.ComicBookOneShotNamingFormat).Value,
                mediaSettings.First(s => s.Id == MediaSettingKey.ColonReplacement).Value,
                mediaSettings.First(s => s.Id == MediaSettingKey.MissingTokenReplacement).Value,
                bool.Parse(mediaSettings.First(s => s.Id == MediaSettingKey.RenameFiles).Value));
    }
}