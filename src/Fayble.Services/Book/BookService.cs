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

    public BookService(ILogger<BookService> logger, IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Models.Book.Book?> Get(Guid id)
    {
        return (await _bookRepository.Get(id))?.ToModel(Guid.NewGuid());
    }

    public async Task<Models.Book.Book> Update(Guid id, UpdateBook book)
    {
        var entity = await _bookRepository.Get(id);

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
            DateOnly.TryParseExact(book.StoreDate, "yyyy-MM-dd", out var storeDate) ? storeDate : null);

        await _unitOfWork.Commit();

        //TODO: User Id
        return entity.ToModel();
    }
}