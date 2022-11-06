using Fayble.Core.Helpers;
using Fayble.Domain;
using Fayble.Domain.Aggregates.BackgroundTask;
using Fayble.Domain.Aggregates.Book;
using Fayble.Domain.Enums;
using Fayble.Domain.Repositories;
using Fayble.Models.Import;
using Fayble.Services.FileSystem;
using Microsoft.Extensions.Logging;

namespace Fayble.Services.BackgroundServices.Services;

public class BackgroundImportService : IBackgroundImportService
{
    private readonly ISeriesRepository _seriesRepository;
    private readonly IComicBookFileSystemService _comicBookFileSystemService;
    private readonly IBackgroundTaskRepository _backgroundTaskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public BackgroundImportService(
        ISeriesRepository seriesRepository,
        IComicBookFileSystemService comicBookFileSystemService,
        IUnitOfWork unitOfWork,
        IBackgroundTaskRepository backgroundTaskRepository,
        ILogger<BackgroundImportService> logger)
    {
        _seriesRepository = seriesRepository;
        _comicBookFileSystemService = comicBookFileSystemService;
        _unitOfWork = unitOfWork;
        _backgroundTaskRepository = backgroundTaskRepository;
        _logger = logger;
    }

    public async Task Import(ImportFileRequest importFile, Guid backgroundTaskId)
    {
        try
        {
            await UpdateTaskStatus(backgroundTaskId, BackgroundTaskStatus.Running);
            _logger.LogInformation("Importing book: {FilePath}", importFile.FilePath);
            var series = await _seriesRepository.Get(importFile.SeriesId);
            var file = _comicBookFileSystemService.GetFile(importFile.FilePath);

            var newFilePath = Path.Combine(
                series.Library.FolderPath,
                series.FolderPath,
                $"{importFile.DestinationFileName}.{file.FileExtension}");

            var bookFile = new BookFile(
                Guid.NewGuid(),
                importFile.DestinationFileName,
                PathHelpers.GetRelativePath(newFilePath, series.Library.FolderPath),
                file.FileSize,
                file.FileExtension,
                file.FileLastModifiedDate,
                file.PageCount,
                _comicBookFileSystemService.GetHash(file.FilePath));

            var book = new Domain.Aggregates.Book.Book(
                Guid.NewGuid(),
                series.LibraryId,
                MediaType.ComicBook,
                importFile.Number,
                bookFile,
                series.Id);

            book.SetMediaRoot(ApplicationHelpers.GetMediaDirectoryRoot(book.Id));

            series.AddBook(book);

            _comicBookFileSystemService.ExtractComicCoverImage(file.FilePath, book.MediaRoot, book.Id);

            File.Move(importFile.FilePath, newFilePath);

            await _unitOfWork.Commit();

            await UpdateTaskStatus(backgroundTaskId, BackgroundTaskStatus.Complete);
            _logger.LogInformation("Imported book: {FilePath}", importFile.FilePath);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "An error occurred while importing book: {FilePath}", importFile.FilePath);
            await UpdateTaskStatus(backgroundTaskId, BackgroundTaskStatus.Failed);
        }
    }

    private async Task UpdateTaskStatus(Guid id, BackgroundTaskStatus status)
    {
        var task = await _backgroundTaskRepository.Get(id);
        task.UpdateStatus(status);
        await _unitOfWork.Commit();
    }
}