namespace Fayble.Services.Tag;

public static class Mappings
{
    public static Models.Tag.Tag ToModel(this Domain.Aggregates.Tag.BookTag entity)
    {
        return new Models.Tag.Tag(entity.Id, entity.Name);
    }
}