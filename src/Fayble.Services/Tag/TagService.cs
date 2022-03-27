using Fayble.Domain.Repositories;

namespace Fayble.Services.Tag;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;

    public TagService(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<IEnumerable<Models.Tag.Tag>?> GetAll()
    {
        return (await _tagRepository.Get())?.Select(t => t.ToModel());
    }
}