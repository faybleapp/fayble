using Fayble.Core.Exceptions;
using Fayble.Domain.Enums;
using Fayble.Domain.Repositories;
using Fayble.Models.FileSystem;
using Fayble.Services.FileSystem;

namespace Fayble.Services.Import;

public class ImportService : IImportService
{
    private readonly IComicBookFileSystemService _comicBookFileSystemService;
    private readonly ILibraryRepository _libraryRepository;

    public ImportService(IComicBookFileSystemService comicBookFileSystemService, ILibraryRepository libraryRepository)
    {
        _comicBookFileSystemService = comicBookFileSystemService;
        _libraryRepository = libraryRepository;
    }

    public async Task<IEnumerable<ComicFile>> Scan(string path)
    {
        path = path.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
        var existingPath = await _libraryRepository.Get(
            x => path.ToLower().StartsWith(
                x.FolderPath.ToLower().TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar));
        if (existingPath.Any())
        {
            throw new DomainException("Cannot search scan directory that is part of an existing library");
        }

        var filePaths = await _comicBookFileSystemService.GetFilePaths(path, MediaType.ComicBook);
        return filePaths.Select(filePath => _comicBookFileSystemService.GetFile(filePath)).OrderBy(f => f.FileName).ToList();
    }
}