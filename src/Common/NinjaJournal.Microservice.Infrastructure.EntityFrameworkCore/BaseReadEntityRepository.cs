using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Ardalis.Specification;

namespace NinjaJournal.Microservice.Infrastructure.EntityFrameworkCore;

public class BaseReadEntityRepository<TKey, TEntity, TDbContext> : IReadEntityRepository<TKey, TEntity>
    where TKey : struct
    where TEntity : class, IAggregateRoot<TKey>
    where TDbContext : BaseDbContext<TDbContext>
{
    private readonly TDbContext _dbContext;

    public BaseReadEntityRepository(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<IReadOnlyCollection<TEntity>> GetAllAsync(bool disableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        if (disableTracking is true)
            query = query.AsNoTracking();

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyCollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, bool disableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>().Where(predicate);

        if (disableTracking is true)
            query = query.AsNoTracking();

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<IReadOnlyCollection<TEntity>> GetAllAsync(ISpecification<TEntity> spec, bool disableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();
        
        query = this.ApplySpecification(query, spec);

        if (disableTracking is true)
            query = query.AsNoTracking();

        return await query.ToListAsync(cancellationToken);
    }
    
    public virtual async Task<IReadOnlyCollection<TEntity>> GetAllAsync(IList<ISpecification<TEntity>> specs, bool disableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        foreach (var specification in specs)
            query = this.ApplySpecification(query, specification);

        if (disableTracking is true)
            query = query.AsNoTracking();

        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool disableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>().Where(predicate);

        if (disableTracking is true)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity> GetByIdAsync(TKey id, bool disableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        if (disableTracking is true)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    private IQueryable<TEntity> ApplySpecification(IQueryable<TEntity> query, ISpecification<TEntity> specification)
    {
        return SpecificationEvaluator.Default.GetQuery(query.AsQueryable(), specification);
    }
}