namespace Fayble.Services.ComicLibrary;

public interface IComicLibraryService
{
    Task Scan(Guid libraryId);
}