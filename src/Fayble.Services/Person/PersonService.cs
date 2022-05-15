using Fayble.Domain.Repositories;
using Fayble.Models;

namespace Fayble.Services.Person;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<IEnumerable<Models.Person>> GetPeople()
    {
        return (await _personRepository.Get()).Select(p => p.ToModel());
    }
}