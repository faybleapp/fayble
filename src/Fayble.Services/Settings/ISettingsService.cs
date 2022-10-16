using Fayble.Models.Settings;

namespace Fayble.Services.Settings;

public interface ISettingsService
{
    Task<MediaSettings> GetMediaSettings();
    Task<MediaSettings> UpdateMediaSettings(MediaSettings settings);
}