namespace Fayble.Models;

public class Person
{
    public Guid? Id { get; private set; }
    public string Name { get; private set; }

    public Person(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}