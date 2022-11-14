using System.Runtime.CompilerServices;
using Fayble.Core.Extensions;
using Fayble.Domain;
using Fayble.Domain.Repositories;
using Fayble.Models.Series;
using Fayble.Services.BackgroundServices;
using Fayble.Services.Book;
using Microsoft.Extensions.Logging;

namespace Fayble.Services.Series;

public class SeriesService : ISeriesService
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookRepository _bookRepository;
    private readonly ISeriesRepository _seriesRepository;
    private readonly IBackgroundTaskService _backgroundTaskService;

    public SeriesService(
        ILogger<SeriesService> logger,
        IUnitOfWork unitOfWork,
        ISeriesRepository seriesRepository,
        IBookRepository bookRepository,
        IBackgroundTaskService backgroundTaskService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _seriesRepository = seriesRepository;
        _bookRepository = bookRepository;
        _backgroundTaskService = backgroundTaskService;
    }


    public async Task<Models.Series.Series?> Get(Guid seriesId)
    {
        return (await _seriesRepository.Get(seriesId))?.ToModel(Guid.NewGuid());
    }

    public async Task<IEnumerable<Models.Series.Series>?> GetAll()
    {
        //TODO: Pass through current user.
        return (await _seriesRepository.Get())?.Select(x => x.ToModel(Guid.NewGuid())).OrderBy(s => s.Name);
    }

    public async Task<IEnumerable<Models.Book.Book>?> GetBooks(Guid seriesId)
    {
        return (await _bookRepository.Get()).Where(x => x.SeriesId == seriesId).OrderByAlphaNumeric(b => b.Number )
            ?.Select(x => x.ToModel());
    }

    public async Task<Models.Series.Series> Update(Guid seriesId, UpdateSeries series)
    {
        var entity = await _seriesRepository.Get(seriesId);

        var refreshRequired = entity.MatchId != series.MatchId;

        entity.Update(
            series.Name,
            series.Year,
            series.Summary,
            series.Notes,
            series.Volume,
            series.Rating,
            series.PublisherId,
            series.ParentSeriesId, 
            series.MatchId);

        foreach (var fieldLockProperty in series.FieldLocks.GetType().GetProperties())
        {
            entity.UpdateFieldLock(fieldLockProperty.Name, (bool)fieldLockProperty.GetValue(series.FieldLocks)!);
        }

        await _unitOfWork.Commit();

        if (refreshRequired)
        {
            await RefreshMetadata(seriesId);
        }

        return entity.ToModel();
    }

    public async Task RefreshMetadata(Guid seriesId)
    {
        await _backgroundTaskService.QueueSeriesMetadataRefresh(seriesId);
    }
}