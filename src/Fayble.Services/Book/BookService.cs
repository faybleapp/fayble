using System.Runtime.CompilerServices;
using Fayble.Domain;
using Fayble.Domain.Aggregates.Tag;
using Fayble.Domain.Repositories;
using Fayble.Models.Book;
using Fayble.Services.Tag;
using Microsoft.Extensions.Logging;

namespace Fayble.Services.Book;

public class BookService : IBookService
{
    private readonly ILogger _logger;
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookTagRepository _tagRepository;

    public BookService(
        ILogger<BookService> logger,
        IBookRepository bookRepository,
        IUnitOfWork unitOfWork,
        IBookTagRepository tagRepository)
    {
        _logger = logger;
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
        _tagRepository = tagRepository;
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
            DateOnly.TryParseExact(book.ReleaseDate, "yyyy-MM-dd", out var releaseDate) ? releaseDate: null,
            DateOnly.TryParseExact(book.CoverDate, "yyyy-MM", out var coverDate) ? coverDate : null,
            tags);

        await _unitOfWork.Commit();

        await CleanTags();

        //TODO: User Id
        return entity.ToModel();
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

        foreach (var tag in tags)
        {
            _tagRepository.Delete(tag);
        }

        await _unitOfWork.Commit();
    }
}