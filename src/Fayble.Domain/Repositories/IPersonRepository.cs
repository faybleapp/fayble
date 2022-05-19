using Fayble.Domain.Aggregates.Person;
using Fayble.Domain.Aggregates.Tag;

namespace Fayble.Domain.Repositories;

public interface IPersonRepository : IRepositoryBase<Person, Guid>
{
    Task<Person> GetByName(string name);
}
