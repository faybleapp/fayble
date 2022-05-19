namespace Fayble.Services.Person;

public static class Mappings
{
    public static Models.Person ToModel(this Domain.Aggregates.Person.Person entity)
    {
        return new Models.Person(entity.Id, entity.Name);
    }
}