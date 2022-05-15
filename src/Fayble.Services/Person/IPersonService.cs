namespace Fayble.Services.Person;

public interface IPersonService
{
    Task<IEnumerable<Models.Person>> GetPeople();
}