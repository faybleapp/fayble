namespace Fayble.Services.Publisher;

public interface IPublisherService
{
    Task<IEnumerable<Models.Publisher>> GetAll();
    Task<Models.Publisher?> Get(Guid libraryId);
}