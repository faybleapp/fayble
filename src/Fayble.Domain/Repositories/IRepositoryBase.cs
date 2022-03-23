using System.Linq.Expressions;

namespace Fayble.Domain.Repositories;

public interface IRepositoryBase<TAggregateRoot, TId>
    where TAggregateRoot : class, IAggregateRoot
    where TId : struct
{
    Task<IEnumerable<TAggregateRoot>> Get();

    Task<TAggregateRoot> Get(TId id);

    Task<IEnumerable<TAggregateRoot>> Get(Expression<Func<TAggregateRoot, bool>> predicate);

    TAggregateRoot Add(TAggregateRoot entity);

    TAggregateRoot Update(TAggregateRoot entity);

    void Delete(TAggregateRoot entity);

    Task Delete(TId id);
}