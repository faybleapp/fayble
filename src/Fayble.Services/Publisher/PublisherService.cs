using Fayble.Domain.Repositories;

namespace Fayble.Services.Publisher;

public class PublisherService : IPublisherService
{
    private readonly IPublisherRepository _publisherRepository;

    public PublisherService(IPublisherRepository publisherRepository)
    {
        _publisherRepository = publisherRepository;
    }

    public async Task<IEnumerable<Models.Publisher>> GetAll()
    {
        return (await _publisherRepository.Get()).OrderBy(p => p.Name).Select(p => p.ToModel());
    }

    public async Task<Models.Publisher?> Get(Guid libraryId)
    {
        return (await _publisherRepository.Get(libraryId))?.ToModel();
    }
}