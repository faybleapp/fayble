﻿using Fayble.Core.Extensions;
using Fayble.Domain;
using Fayble.Domain.Aggregates.People;
using Fayble.Domain.Aggregates.Tag;
using Fayble.Domain.Enums;
using Fayble.Domain.Repositories;
using Fayble.Models.Book;
using Microsoft.Extensions.Logging;

namespace Fayble.Services.Book;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly ILogger _logger;
    private readonly ISeriesRepository _seriesRepository;
    private readonly IBookTagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookService(
        ILogger<BookService> logger,
        IBookRepository bookRepository,
        IUnitOfWork unitOfWork,
        IBookTagRepository tagRepository,
        ISeriesRepository seriesRepository)
    {
        _logger = logger;
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
        _tagRepository = tagRepository;
        _seriesRepository = seriesRepository;
    }

    public async Task<Models.Book.Book?> Get(Guid id)
    {
        return (await _bookRepository.Get(id))?.ToModel(Guid.NewGuid());
    }

    public async Task<Models.Book.Book> Update(Guid id, UpdateBook book)
    {
        var entity = await _bookRepository.Get(id);
        var tags = await UpdateTags(book.Tags);

        entity.Update(
            book.Title,
            book.Number,
            book.Summary,
            book.Rating,
            book.Language,
            DateOnly.TryParseExact(book.ReleaseDate, "yyyy-MM-dd", out var releaseDate) ? releaseDate : null,
            DateOnly.TryParseExact(book.CoverDate, "yyyy-MM", out var coverDate) ? coverDate : null,
            tags);

        await _unitOfWork.Commit();

        await CleanTags();

        //TODO: User Id
        return entity.ToModel();
    }

    public async Task<RelatedBooks> GetRelated(Guid id)
    {
        var book = await _bookRepository.Get(id);

        var relatedBooks = new RelatedBooks();
        if (book.SeriesId != null)
        {
            var series = await _seriesRepository.Get((Guid) book.SeriesId);
            relatedBooks.BooksInSeries = series.Books.OrderBy(b => b.Number).Where(b => b.Id != id).Take(40)
                .Select(b => b.ToModel()).OrderByAlphaNumeric(b => b.Number).ToList();
        }

        // TODO: Check Genre

        if (book.MediaType == MediaType.Book)
        {
            var authorId = book.People.FirstOrDefault(p => p.Type == PersonType.Author)?.Id;
            if (authorId != null)
                relatedBooks.BooksByAuthor = (await _bookRepository.Get(b => b.People.Any(p => p.Id == authorId)))
                    .Take(40).Select(b => b.ToModel()).ToList();

            if (book.PublisherId != null)
                relatedBooks.BooksByPublisher =
                    (await _bookRepository.Get(b => b.People.Any(p => p.Id == book.PublisherId)))
                    .Take(40).Select(b => b.ToModel()).ToList();
        }
        else
        {
            var writerId = book.People.FirstOrDefault(p => p.Type == PersonType.Author)?.Id;
            if (writerId == null)
                relatedBooks.BooksByWriter =
                    (await _bookRepository.Get(b => b.People.Any(p => p.Id == writerId)))
                    .Take(40).Select(b => b.ToModel()).ToList();
        }

        if (book.ReleaseDate != null)
        {
            relatedBooks.BooksReleasedSameMonth = (await _bookRepository.Get(
                    b => b.ReleaseDate != null && b.ReleaseDate.Value.Month == book.ReleaseDate.Value.Month))
                .Take(40)
                .Select(b => b.ToModel()).ToList();

            if (!relatedBooks.BooksReleasedSameMonth.Any())
                relatedBooks.BooksReleasedSameYear =
                    (await _bookRepository.Get(
                        b => b.ReleaseDate != null && b.ReleaseDate.Value.Year == book.ReleaseDate.Value.Year)).Take(40)
                    .Select(b => b.ToModel()).ToList();
        }

        return relatedBooks;
    }

    private async Task<ICollection<BookTag>> UpdateTags(IEnumerable<string> newTags)
    {
        var tags = new List<BookTag>();

        foreach (var tag in newTags)
        {
            var tagEntity = await _tagRepository.GetByName(tag);

            if (tagEntity != null)
            {
                tags.Add(tagEntity);
            }
            else
            {
                var newTag = new BookTag(Guid.NewGuid(), tag);
                _tagRepository.Add(newTag);
                tags.Add(newTag);
            }
        }

        return tags;
    }

    private async Task CleanTags()
    {
        var tags = await _tagRepository.Get(t => !t.Books.Any());

        foreach (var tag in tags) _tagRepository.Delete(tag);

        await _unitOfWork.Commit();
    }
}