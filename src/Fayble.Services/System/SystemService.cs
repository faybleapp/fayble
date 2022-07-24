using Fayble.Core.Exceptions;
using Fayble.Domain;
using Fayble.Domain.Aggregates.SystemConfiguration;
using Fayble.Domain.Repositories;
using Fayble.Models;
using Fayble.Security.Models;
using Fayble.Security.Services.Authentication;
using SystemSettings = Fayble.Models.SystemSettings;

namespace Fayble.Services.System;

public class SystemService : ISystemService
{
    private readonly ISystemConfigurationRepository _systemConfigurationRepository;
    private readonly IAuthenticationService _authenticationService;
    private readonly IUnitOfWork _unitOfWork;

    public SystemService(
        ISystemConfigurationRepository systemConfigurationRepository,
        IAuthenticationService authenticationService,
        IUnitOfWork unitOfWork)
    {
        _systemConfigurationRepository = systemConfigurationRepository;
        _authenticationService = authenticationService;
        _unitOfWork = unitOfWork;
    }

    public async Task FirstRun(FirstRun firstRunConfiguration)
    {
        var systemConfiguration = await GetConfiguration();

        if (!systemConfiguration.FirstRun)
            throw new DomainException("First run configuration already completed.");

        await _authenticationService.CreateUser(
            new NewUser(
                firstRunConfiguration.OwnerCredentials.Username,
                firstRunConfiguration.OwnerCredentials.Password,
                UserRoles.Owner));

        await UpdateConfiguration(SystemSettingKey.FirstRun, false.ToString());
    }

    public async Task UpdateConfiguration(SystemSettings updatedConfiguration)
    {
        var systemConfiguration = await GetConfiguration();
        if (systemConfiguration.FirstRun != updatedConfiguration.FirstRun)
        {
            await UpdateConfiguration(SystemSettingKey.FirstRun, updatedConfiguration.FirstRun.ToString());
        }
    }

    private async Task UpdateConfiguration(SystemSettingKey configurationKey, string value)
    {
        var configuration = await _systemConfigurationRepository.Get(configurationKey);
        configuration.Update(value);
        await _unitOfWork.Commit();
    }


    public async Task<SystemSettings> GetConfiguration()
    {
        return (await _systemConfigurationRepository.Get()).ToModel();
    }
}