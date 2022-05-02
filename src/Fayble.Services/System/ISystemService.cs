using Fayble.Models;

namespace Fayble.Services.System;

public interface ISystemService
{
    Task<Models.SystemConfiguration> GetConfiguration();
    Task FirstRun(FirstRun firstRunConfiguration);
    Task UpdateConfiguration(SystemConfiguration updatedConfiguration);
}