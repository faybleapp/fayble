using Fayble.Domain.Aggregates.Series;

namespace Fayble.Domain.Repositories;

public interface ISeriesRepository : IRepositoryBase<Series, Guid>
{
}
