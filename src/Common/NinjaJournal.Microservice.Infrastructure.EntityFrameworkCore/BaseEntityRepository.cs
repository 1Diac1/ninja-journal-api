using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace NinjaJournal.Microservice.Infrastructure.EntityFrameworkCore;

public class BaseEntityRepository<TKey, TEntity, TDbContext> : IEntityRepository<TKey, TEntity>
    where TKey : struct
    where TEntity : BaseEntity<TKey>
    where TDbContext : BaseDbContext<TDbContext>
{
    private readonly TDbContext _dbContext;

    public BaseEntityRepository(TDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

        if (autoSave is true)
            await SaveAsync(cancellationToken);

        return entity;
    }

    public virtual async Task UpdateAsync(TEntity entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().Entry(entity).State = EntityState.Modified;

        if (autoSave is true)
            await SaveAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync(TEntity entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().Remove(entity);

        if (autoSave is true)
            await SaveAsync(cancellationToken);
    }

    public virtual async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}