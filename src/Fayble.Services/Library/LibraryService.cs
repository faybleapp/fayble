using Fayble.Domain;
using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Enums;
using Fayble.Domain.Repositories;
using Fayble.Services.Series;
using Microsoft.Extensions.Logging;

namespace Fayble.Services.Library;

public class LibraryService : ILibraryService
{
    private readonly ILogger _logger;
    private readonly ILibraryRepository _libraryRepository;
    private readonly ISeriesRepository _seriesRepository;
    private readonly IUnitOfWork _unitOfWork;


    public LibraryService(
        ILogger<LibraryService> logger,
        ILibraryRepository libraryRepository,
        IUnitOfWork unitOfWork,
        ISeriesRepository seriesRepository)
    {
        _logger = logger;
        _libraryRepository = libraryRepository;
        _unitOfWork = unitOfWork;
        _seriesRepository = seriesRepository;
    }

    public async Task<Models.Library.Library> Get(Guid libraryId)
    {
        return (await _libraryRepository.Get(libraryId)).ToModel();
    }

    public async Task<IEnumerable<Models.Library.Library>> GetAll()
    {
        return (await _libraryRepository.Get()).Select(x => x.ToModel());
    }

    public async Task Create(Models.Library.Library library)
    {
        _logger.LogInformation("Creating library: {LibraryName}", library.Name);
        
        var entity = new Domain.Aggregates.Library.Library(
            Guid.NewGuid(),
            library.Name,
            Enum.Parse<MediaType>(library.LibraryType),
            library.Paths, library.Settings.ToEntity());

        var libraryEntity = _libraryRepository.Add(entity);
        await _unitOfWork.Commit();

        _logger.LogDebug("Library created: {@Library}",
            new
            {
                libraryEntity.Name,
                libraryEntity.Type,
                Paths = libraryEntity.Paths.Select(x => x.Path)
            });

        _logger.LogInformation("Created library: {LibraryName}", library.Name);
    }

    public async Task<Models.Library.Library> Update(Guid id, Models.Library.Library library)
    {
        var entity = await _libraryRepository.Get(id);
        entity.Update(library.Name, library.Paths);
        entity.UpdateSetting(LibrarySettingKey.ReviewOnImport, library.Settings.ReviewOnImport.ToString());

        await _unitOfWork.Commit();
        return entity.ToModel();
    }

    public async Task Delete(Guid id)
    {
        await _libraryRepository.Delete(id);
        await _unitOfWork.Commit();
    }


    public async Task<IEnumerable<Models.Series.Series>?> GetSeries(Guid libraryId)
    {
        //TODO: Pass through current user.
        return (await _seriesRepository.Get()).Where(x => x.LibraryId == libraryId)
            ?.Select(x => x.ToModel());
    }
}