using System.Linq.Expressions;
using Fayble.Core.Exceptions;
using Fayble.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fayble.Infrastructure.Repositories;

public abstract class RepositoryBase<TContext, TEntity, TKey>
    where TContext : DbContext
    where TEntity : IdentifiableEntity<TKey>
    where TKey : struct
{
    protected TContext Context;
    private static readonly string EntityName = typeof(TEntity).Name;

    protected RepositoryBase(TContext context)
    {
        Context = context;
    }

    public virtual TEntity Add(TEntity entity)
    {
        return Context.Set<TEntity>().Add(entity).Entity;
    }

    public virtual TEntity Update(TEntity entity)
    {
        return Context.Set<TEntity>().Update(entity).Entity;
    }

    public virtual void Delete(TEntity entity)
    {
        Context.Remove(entity);
    }

    public virtual async Task Delete(TKey id)
    {
        var entity = await Get(id);
        Context.Remove(entity);
    }

    public virtual async Task<TEntity> Get(TKey id)
    {
        var entity = await GetWithIncludes().FirstOrDefaultAsync(x => x.Id.Equals(id));

        if (entity == null)
            throw new NotFoundException($"{EntityName} not found with id: {id}");

        return entity;
    }

    public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
    {
        return await GetWithIncludes().Where(predicate).ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> Get()
    {
        return await GetWithIncludes().ToListAsync();
    }

    protected virtual IQueryable<TEntity> GetWithIncludes()
    {
        return Context.Set<TEntity>();
    }
}