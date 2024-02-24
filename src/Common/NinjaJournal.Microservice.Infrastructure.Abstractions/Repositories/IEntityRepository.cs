using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

namespace NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;

public interface IEntityRepository<TKey, TEntity> where TEntity : IAggregateRoot<TKey>
    where TKey : struct
{
    Task<TEntity> AddAsync(TEntity entity, bool autoSave = true, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, bool autoSave = true, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, bool autoSave = true, CancellationToken cancellationToken = default);
    Task SaveAsync(CancellationToken cancellationToken = default);
}