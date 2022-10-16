using Fayble.Models.Settings;
using Fayble.Services.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Fayble.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SettingsController : ControllerBase
{
    private readonly ISettingsService _settingsService;

    public SettingsController(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    [HttpGet("media")]
    public async Task<MediaSettings> GetMediaSettings()
    {
        return await _settingsService.GetMediaSettings();
    }

    [HttpPost("media")]
    public async Task<ActionResult<MediaSettings>> UpdateMediaSettings([FromBody] MediaSettings settings)
    {
        return await _settingsService.UpdateMediaSettings(settings);
    }
}

