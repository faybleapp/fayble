using Fayble.Domain.Aggregates.Configuration;
using Fayble.Domain.Repositories;

namespace Fayble.Infrastructure.Repositories;

public class ConfigurationRepository : RepositoryBase<FaybleDbContext, Configuration, Setting>, IConfigurationRepository
{
    public ConfigurationRepository(FaybleDbContext context) : base(context)
    {
    }
}
