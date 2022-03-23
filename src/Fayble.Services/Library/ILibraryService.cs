namespace Fayble.Services.Library;

public interface ILibraryService
{
    Task<Models.Library.Library> Get(Guid libraryId);
    Task<IEnumerable<Models.Library.Library>> GetAll();
    Task Create(Models.Library.Library library);
    Task<Models.Library.Library> Update(Guid id, Models.Library.Library library);
    Task Delete(Guid id);
    Task<IEnumerable<Models.Series.Series>?> GetSeries(Guid libraryId);
}