using Fayble.Domain.Aggregates.Configuration;

namespace Fayble.Domain.Repositories;

public interface IConfigurationRepository : IRepositoryBase<Configuration, Setting>
{
}