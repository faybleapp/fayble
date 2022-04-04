using Fayble.Domain;
using Fayble.Domain.Repositories;

namespace Fayble.Services.Tag;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TagService(ITagRepository tagRepository, IUnitOfWork unitOfWork)
    {
        _tagRepository = tagRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Models.Tag.Tag>?> GetAll()
    {
        return (await _tagRepository.Get())?.Select(t => t.ToModel());
    }

}