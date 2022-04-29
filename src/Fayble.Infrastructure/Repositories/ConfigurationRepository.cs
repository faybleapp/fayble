using Fayble.Domain.Aggregates.Configuration;
using Fayble.Domain.Repositories;

namespace Fayble.Infrastructure.Repositories;

public class ConfigurationRepository : RepositoryBase<FaybleDbContext, Configuration, ConfigurationKey>, IConfigurationRepository
{
    public ConfigurationRepository(FaybleDbContext context) : base(context)
    {
    }
}
