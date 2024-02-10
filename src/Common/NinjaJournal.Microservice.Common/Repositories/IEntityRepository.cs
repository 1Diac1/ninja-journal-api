using NinjaJournal.Microservice.Common.Models;
using System.Linq.Expressions;
using Ardalis.Specification;

namespace NinjaJournal.Microservice.Common.Repositories;

public interface IEntityRepository<TEntity> where TEntity : BaseEntity
{
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, bool disableTracking = true, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TEntity>> GetAllAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, bool disableTracking = true, CancellationToken  cancellationToken = default);

    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, bool autoSave = true, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, bool autoSave = true, CancellationToken cancellationToken = default);
    Task SaveAsync(CancellationToken cancellationToken = default);
}