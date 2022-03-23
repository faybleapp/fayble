namespace Fayble.Services.Publisher;

public interface IPublisherService
{
    Task<IEnumerable<Models.Publisher.Publisher>> GetAll();
    Task<Models.Publisher.Publisher?> Get(Guid libraryId);
}