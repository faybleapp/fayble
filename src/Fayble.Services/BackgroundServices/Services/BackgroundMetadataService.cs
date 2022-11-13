using Fayble.Domain;
using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Domain.Repositories;
using Fayble.Models.BackgroundTask;
using Microsoft.Extensions.Logging;

namespace Fayble.Services.BackgroundServices.Services;

public class BackgroundMetadataService
{
    private readonly IBackgroundTaskRepository _backgroundTaskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public BackgroundMetadataService(IBackgroundTaskRepository backgroundTaskRepository, IUnitOfWork unitOfWork, ILogger<BackgroundMetadataService> logger)
    {
        _backgroundTaskRepository = backgroundTaskRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task RefreshLibraryMetadata(Guid libraryId)
    {
        try
        {
            await UpdateTaskStatus(libraryId, BackgroundTaskStatus.Running);
            _logger.LogInformation("Refreshing metadata for library: {LibraryId}", libraryId);
        }
        catch (Exception ex)
        {

        }
    }

    private async Task UpdateTaskStatus(Guid id, BackgroundTaskStatus status)
    {
        var task = await _backgroundTaskRepository.Get(id);
        task.UpdateStatus(status);
        await _unitOfWork.Commit();
    }
}