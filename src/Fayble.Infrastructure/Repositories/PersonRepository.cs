using Fayble.Domain.Aggregates.Person;
using Fayble.Domain.Aggregates.Tag;
using Fayble.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fayble.Infrastructure.Repositories;

public class PersonRepository : RepositoryBase<FaybleDbContext, Person, Guid>, IPersonRepository
{
    public PersonRepository(FaybleDbContext context) : base(context)
    {
    }

    public async Task<Person?> GetByName(string name)
    {
        return await GetWithIncludes().FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
    }
}