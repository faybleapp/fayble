using Fayble.Domain;
using Fayble.Domain.Repositories;
using Fayble.Models.Book;
using Microsoft.Extensions.Logging;

namespace Fayble.Services.Book;

public class BookService : IBookService
{
    private readonly ILogger _logger;
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITagRepository _tagRepository;

    public BookService(
        ILogger<BookService> logger,
        IBookRepository bookRepository,
        IUnitOfWork unitOfWork,
        ITagRepository tagRepository)
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

        var tags = new List<Domain.Aggregates.Tag.Tag>();

        foreach (var tag in book.Tags)
        {
            var tagEntity = (await _tagRepository.Get(t => t.Name.ToLower() == tag.ToLower())).FirstOrDefault();

            if (tagEntity != null)
            {
                tags.Add(tagEntity);
            }
            else
            {
                var newTag = new Domain.Aggregates.Tag.Tag(Guid.NewGuid(), tag);
                _tagRepository.Add(newTag);
                tags.Add(newTag);
            }
        }

        entity.Update(
            book.Title,
            book.Number,
            book.Summary,
            book.Notes,
            book.Rating,
            book.Locked,
            book.Language,
            book.Review,
            DateOnly.TryParseExact(book.CoverDate, "yyyy-MM-dd", out var coverDate) ? coverDate : null,
            DateOnly.TryParseExact(book.StoreDate, "yyyy-MM-dd", out var storeDate) ? storeDate : null,
            tags);

        await _unitOfWork.Commit();

        //TODO: User Id
        return entity.ToModel();
    }
}