using Ardalis.Specification;

namespace NinjaJournal.Microservice.Infrastructure.EntityFrameworkCore.Specifications;

public sealed class EntityWithLimitSpecification<TEntity> : Specification<TEntity>
{
    public EntityWithLimitSpecification(int limit)
    {
        Query.Take(limit);
    }
}