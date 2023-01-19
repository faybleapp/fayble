using System;
using System.Runtime.InteropServices;
using Fayble.Domain;
using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Domain.Repositories;
using Fayble.Models.BackgroundTask;
using Fayble.Services.MetadataService;
using Microsoft.Extensions.Logging;

namespace Fayble.Services.BackgroundServices.Services;

public class BackgroundMetadataService : IBackgroundMetadataService
{
    private readonly IBackgroundTaskRepository _backgroundTaskRepository;
    private readonly ISeriesRepository _seriesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMetadataService _metadataService;
    private readonly IBookRepository _bookRepository;
    private readonly ILogger _logger;

    public BackgroundMetadataService(
        IBackgroundTaskRepository backgroundTaskRepository,
        IUnitOfWork unitOfWork,
        ILogger<BackgroundMetadataService> logger,
        ISeriesRepository seriesRepository,
        IMetadataService metadataService,
        IBookRepository bookRepository)
    {
        _backgroundTaskRepository = backgroundTaskRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _seriesRepository = seriesRepository;
        _metadataService = metadataService;
        _bookRepository = bookRepository;
    }

    public async Task RefreshSeriesMetadata(Guid seriesId, Guid backgroundTaskId)
    {
        try
        {
            await UpdateTaskStatus(backgroundTaskId, BackgroundTaskStatus.Running);
            _logger.LogInformation("Refreshing metadata for series: {SeriesId}", seriesId);
            var series = await _seriesRepository.Get(seriesId);

            if (series.MatchId == null)
            {
                _logger.LogInformation("Series is not matched.");
                return;
            }

            var metadata = await _metadataService.GetSeries((Guid)series.MatchId);
            series.UpdateFromMetadata(metadata.Name, metadata.DescriptionWithoutTags, metadata.StartYear);
            await _unitOfWork.Commit();
            await UpdateTaskStatus(backgroundTaskId, BackgroundTaskStatus.Complete);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "An error occurred while refreshing metadata for series; {SeriesId}", seriesId);
            await UpdateTaskStatus(backgroundTaskId, BackgroundTaskStatus.Failed);
        }
    }

    public async Task RefreshBookMetadata(Guid bookId, Guid backgroundTaskId)
    {
        try
        {
            await UpdateTaskStatus(backgroundTaskId, BackgroundTaskStatus.Running);
            _logger.LogInformation("Refreshing metadata for book: {BookId}", bookId);
            var book = await _bookRepository.Get(bookId);

            if (book.MatchId == null)
            {
                _logger.LogInformation("Book is not matched.");
                return;
            }

            var metadata = await _metadataService.GetBook((Guid) book.MatchId);
            book.UpdateFromMetadata(
                metadata.Title,
                metadata.Number,
                metadata.Summary,
                metadata.CoverDate,
                //TODO: Update tags and people
                null,
                null);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "An error occurred while refreshing metadata for book; {BookId}", bookId);
            await UpdateTaskStatus(backgroundTaskId, BackgroundTaskStatus.Failed);
        }
    }

    private async Task UpdateTaskStatus(Guid id, BackgroundTaskStatus status)
    {
        var task = await _backgroundTaskRepository.Get(id);
        task.UpdateStatus(status);
        await _unitOfWork.Commit();
    }
}