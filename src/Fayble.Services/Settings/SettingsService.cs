using Fayble.Domain.Aggregates.MediaSetting;
using Fayble.Domain.Repositories;
using Fayble.Models.Settings;

namespace Fayble.Services.Settings;

public class SettingsService : ISettingsService
{
    private readonly IMediaSettingRepository _mediaSettingRepository;

    public SettingsService(IMediaSettingRepository mediaSettingRepository)
    {
        _mediaSettingRepository = mediaSettingRepository;
    }

    public async Task<MediaSettings> GetMediaSettings()
    {
        var mediaSettings = await _mediaSettingRepository.Get();
        return mediaSettings.ToList().ToModel();
    }
}