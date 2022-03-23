using Fayble.Domain.Aggregates.Publisher;
using Fayble.Domain.Repositories;

namespace Fayble.Infrastructure.Repositories;

public class PublisherRepository : RepositoryBase<FaybleDbContext, Publisher, Guid>, IPublisherRepository
{
    public PublisherRepository(FaybleDbContext context) : base(context)
    {
    }
}