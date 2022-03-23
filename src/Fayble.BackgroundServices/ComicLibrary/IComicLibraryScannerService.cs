namespace Fayble.BackgroundServices.ComicLibrary;

public interface IComicLibraryScannerService
{
    Task Run(Guid libraryId, Guid taskId);
}