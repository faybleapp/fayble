using Fayble.Domain;
using Fayble.Domain.Repositories;

namespace Fayble.Services.Tag;

public class TagService : ITagService
{
    private readonly IBookTagRepository _bookTagRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TagService(IBookTagRepository bookTagRepository, IUnitOfWork unitOfWork)
    {
        _bookTagRepository = bookTagRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Models.Tag>?> GetAllBookTags()
    {
        return (await _bookTagRepository.Get())?.Select(t => t.ToModel());
    }

}