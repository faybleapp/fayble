using Fayble.Core.Exceptions;
using Fayble.Core.Helpers;
using Fayble.Domain;
using Fayble.Domain.Enums;
using Fayble.Domain.Repositories;
using Fayble.Models.Import;
using Fayble.Services.BackgroundServices;
using Fayble.Services.FileSystem;
using Fayble.Services.MetadataService;
using Fayble.Services.Settings;

namespace Fayble.Services.Import;

public class ImportService : IImportService
{
    private readonly IComicBookFileSystemService _comicBookFileSystemService;
    private readonly ILibraryRepository _libraryRepository;
    private readonly ISettingsService _settingsService;
    private readonly ISeriesRepository _seriesRepository;
    private readonly IMetadataService _metadataService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBackgroundTaskService _backgroundTaskService;

    public ImportService(
        IComicBookFileSystemService comicBookFileSystemService,
        ILibraryRepository libraryRepository,
        ISettingsService settingsService,
        ISeriesRepository seriesRepository,
        IMetadataService metadataService,
        IUnitOfWork unitOfWork,
        IBackgroundTaskService backgroundTaskService)
    {
        _comicBookFileSystemService = comicBookFileSystemService;
        _libraryRepository = libraryRepository;
        _settingsService = settingsService;
        _seriesRepository = seriesRepository;
        _metadataService = metadataService;
        _unitOfWork = unitOfWork;
        _backgroundTaskService = backgroundTaskService;
    }

    public async Task<IEnumerable<ImportScanFile>> Scan(string path)
    {
        path = path.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
        var existingPath = await _libraryRepository.Get(
            x => path.ToLower().StartsWith(
                x.FolderPath.ToLower().TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar));
        if (existingPath.Any())
        {
            throw new DomainException("Cannot scan directory that is part of an existing library");
        }

        var filePaths = await _comicBookFileSystemService.GetFilePaths(path, MediaType.ComicBook);
        var files = filePaths.Select(filePath => _comicBookFileSystemService.GetFile(filePath)).OrderBy(f => f.FileName)
            .ToList();

        return files.Select(file => new ImportScanFile(
            file.FileName,
            file.FilePath,
            file.Pages.Count,
            file.FileSize,
            ComicBookHelpers.ParseIssueNumber(file.FileName),
            file.ComicInfoXml));
    }

    public async Task Import (List<ImportFileRequest> importFiles)
    {
        foreach (var importFile in importFiles)
        {
            await _backgroundTaskService.QueueImport(importFile);
        }
    }
}