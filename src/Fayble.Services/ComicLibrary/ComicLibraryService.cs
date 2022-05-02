﻿using System.Globalization;
using Fayble.Core.Extensions;
using Fayble.Core.Helpers;
using Fayble.Domain;
using Fayble.Domain.Aggregates.Book;
using Fayble.Domain.Aggregates.Library;
using Fayble.Domain.Enums;
using Fayble.Domain.Repositories;
using Fayble.Models.FileSystem;
using Fayble.Services.FileSystemService;
using Microsoft.Extensions.Logging;

namespace Fayble.Services.ComicLibrary;

//TODO: Remove this service?
public class ComicLibraryService : IComicLibraryService
{
    private readonly IBookRepository _bookRepository;
    private readonly IComicBookFileSystemService _comicBookFileSystemService;
    private readonly ILibraryRepository _libraryRepository;
    private readonly ILogger _logger;
    private readonly ISeriesRepository _seriesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ComicLibraryService(
        ILogger<ComicLibraryService> logger,
        IUnitOfWork unitOfWork,
        IBookRepository bookRepository,
        ILibraryRepository libraryRepository,
        IComicBookFileSystemService comicBookFileSystemService,
        ISeriesRepository seriesRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _bookRepository = bookRepository;
        _libraryRepository = libraryRepository;
        _comicBookFileSystemService = comicBookFileSystemService;
        _seriesRepository = seriesRepository;
    }
    
    public async Task Scan(Guid libraryId)
    {
        var library = await _libraryRepository.Get(libraryId);

        foreach (var libraryPath in library.Paths)
        {
            _logger.LogInformation("Scanning library for new books: {Library}", library.Name);
            await ScanNewBooks(libraryPath);
        }
    }

    private async Task ScanNewBooks(LibraryPath libraryPath)
    {
        // TODO: Configuration service?
       
        
        _logger.LogDebug("Retrieving new files from library paths.");
        var newFiles = await GetNewFiles(libraryPath.Path);
        _logger.LogDebug("{fileCount} new files found in path", newFiles.Count);

        foreach (var newFile in newFiles)
        {
            _logger.LogDebug("Processing issue: {FilePath}", newFile.FilePath);

            var bookFile = new BookFile(
                Guid.NewGuid(),
                newFile.FileName,
                newFile
                    .FilePath.ToLower().Replace(libraryPath.Path.ToLower(), "")
                    .TrimPathSeparators(),
                newFile.FileSize,
                newFile.FileType,
                newFile.FileLastModifiedDate);

            var comicIssue = new Domain.Aggregates.Book.Book(
                Guid.NewGuid(),
                libraryPath.Id,
                libraryPath.LibraryId,
                MediaType.ComicBook,
                newFile.PageCount,
                newFile.Number,
                bookFile);

            comicIssue.UpdateMediaPath(
                ApplicationHelpers.GetMediaDirectory(comicIssue.GetType().Name, comicIssue.Id));

            if (newFile.ComicInfoXml != null)
                comicIssue.Update(
                    newFile.ComicInfoXml.Title,
                    newFile.ComicInfoXml?.Number ?? newFile.Number,
                   newFile.ComicInfoXml?.Summary,
                    0,
                    null,
                    null,
                    DateOnly.TryParseExact(
                        $"{newFile.ComicInfoXml?.Year}-{newFile.ComicInfoXml?.Month}-{newFile.ComicInfoXml?.Day}",
                        "yyyy-M-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var coverDate)
                        ? coverDate
                        : null,
                    null
                    );

            try
            {
                _logger.LogDebug("Extracting cover image from: {FilePath}", newFile.FilePath);
                _comicBookFileSystemService.ExtractComicCoverImage(newFile.FilePath!, comicIssue.MediaPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while extracting cover image.");
            }

            _logger.LogDebug("Matching issue to series: {Series}", newFile.Series);
            var series = await GetSeries(newFile, libraryPath.LibraryId);
            comicIssue.SeriesId = series.Id;

            _bookRepository.Add(comicIssue);
            await _unitOfWork.Commit();

            _logger.LogDebug(
                "New issue added to library: {@ComicIssue}",
                new
                {
                    comicIssue.Id,
                    comicIssue.File.FileType,
                    comicIssue.File.FilePath,
                    comicIssue.LibraryPathId,
                    comicIssue.LibraryId
                });
        }
    }

    private async Task<List<ComicFile>> GetNewFiles(string path)
    {
        var newFiles = new List<ComicFile>();

        var files = await _comicBookFileSystemService.ScanDirectory(path);

        foreach (var file in files)
        {
            var exists = (await _bookRepository.Get(
                b => b.File.FilePath.ToLower() ==
                     file.FilePath.ToLower().Replace(path.ToLower(), "").TrimStart('\\'))).Any();

            if (!exists)
                newFiles.Add(file);
        }

        return newFiles;
    }

    private async Task<Domain.Aggregates.Series.Series> GetSeries(ComicFile file, Guid libraryId)
    {
        var series = _seriesRepository.Get().Result.FirstOrDefault(
            x =>
                x.LibraryId == libraryId && x.Name.ToLower() == file.Series?.ToLower() && x.Volume == file.Volume);

        if (series == null)
        {
            series = new Domain.Aggregates.Series.Series(Guid.NewGuid(), file.Series, file.Volume, libraryId);
            series.SetMediaPath(ApplicationHelpers.GetMediaDirectory(series.GetType().Name, series.Id));

            _logger.LogDebug("Extracting cover image from: {FilePath}", file.FilePath);

            try
            {
                _comicBookFileSystemService.ExtractComicCoverImage(file.FilePath, series.MediaPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while extracting cover image");
            }

            _logger.LogDebug(
                "No existing Series found in database, adding new series: {@Series}",
                new
                {
                    series.Id,
                    series.Name,
                    series.Volume,
                    series.LibraryId,
                    series.MediaPath
                });

            _seriesRepository.Add(series);
            await _unitOfWork.Commit();
        }
        else
        {
            _logger.LogDebug("Matched to series: {@Series}", new {series.Id, series.Name, series.Volume});
        }

        return series;
    }
}