using System.Text.RegularExpressions;
using Fayble.Core.Exceptions;
using Fayble.Core.Extensions;
using Fayble.Domain.Aggregates.MediaSetting;
using Fayble.Domain.Enums;
using Fayble.Domain.Repositories;
using Fayble.Models.FileSystem;
using Fayble.Models.Import;
using Fayble.Models.Metadata;
using Fayble.Models.Settings;
using Fayble.Services.FileSystem;
using Fayble.Services.MetadataService;
using Fayble.Services.Settings;
using SharpCompress.Common;
using ColonReplacement = Fayble.Models.Settings.ColonReplacement;
using MissingTokenReplacement = Fayble.Models.Settings.MissingTokenReplacement;

namespace Fayble.Services.Import;

public class ImportService : IImportService
{
    private readonly IComicBookFileSystemService _comicBookFileSystemService;
    private readonly ILibraryRepository _libraryRepository;
    private readonly ISettingsService _settingsService;
    private readonly ISeriesRepository _seriesRepository;
    private readonly IMetadataService _metadataService;

    public ImportService(
        IComicBookFileSystemService comicBookFileSystemService,
        ILibraryRepository libraryRepository,
        ISettingsService settingsService,
        ISeriesRepository seriesRepository,
        IMetadataService metadataService)
    {
        _comicBookFileSystemService = comicBookFileSystemService;
        _libraryRepository = libraryRepository;
        _settingsService = settingsService;
        _seriesRepository = seriesRepository;
        _metadataService = metadataService;
    }

    public async Task<IEnumerable<ComicFile>> Scan(string path)
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
        return filePaths.Select(filePath => _comicBookFileSystemService.GetFile(filePath)).OrderBy(f => f.FileName).ToList();
    }

    public async Task Import (List<ImportFile> files)
    {

    }

  
}