namespace Fayble.Services.Tag;

public interface ITagService
{
    Task<IEnumerable<Models.Tag.Tag>?> GetAllBookTags();
}