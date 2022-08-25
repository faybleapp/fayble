using Fayble.Models;

namespace Fayble.Services.System;

public interface ISystemService
{
    Task<Models.SystemSettings> GetConfiguration();
    Task FirstRun(FirstRun firstRunConfiguration);
    Task UpdateConfiguration(SystemSettings updatedConfiguration);
}