using DbUp;
using Fayble.Domain;
using Fayble.Domain.Aggregates.MediaSetting;
using Fayble.Domain.Repositories;
using Fayble.Models.Settings;

namespace Fayble.Services.Settings;

public class SettingsService : ISettingsService
{
    private readonly IMediaSettingRepository _mediaSettingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SettingsService(IMediaSettingRepository mediaSettingRepository, IUnitOfWork unitOfWork)
    {
        _mediaSettingRepository = mediaSettingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<MediaSettings> GetMediaSettings()
    {
        var mediaSettings = await _mediaSettingRepository.Get();
        return mediaSettings.ToList().ToModel();
    }

    public async Task<MediaSettings> UpdateMediaSettings(MediaSettings settings)
    {
        var mediaSettings = await _mediaSettingRepository.Get();

        foreach (var mediaSetting in mediaSettings)
        {
            mediaSetting.Update(settings.GetType().GetProperty(mediaSetting.Id.ToString())?.GetValue(settings, null)?.ToString());
        }

        await _unitOfWork.Commit();
        return mediaSettings.ToList().ToModel();
    }
}