namespace Fayble.Services.Publisher;

public static class Mappings
{
    public static Models.Publisher ToModel(this Domain.Aggregates.Publisher.Publisher entity)
    {
        return new Models.Publisher(entity.Id, entity.Name, entity.Description, entity.MediaRoot);
    }

}
