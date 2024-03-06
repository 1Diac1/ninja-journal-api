using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;
using NinjaJournal.Microservice.Application.Abstractions.Services;

namespace NinjaJournal.Microservice.Application.Services;

public class CacheKeyService : ICacheKeyService
{
    public string GenerateCacheKey<TKey, TEntity>(string action, params object[] parameters) 
        where TKey : struct 
        where TEntity : IAggregateRoot<TKey>, ICacheableEntity<TKey>
    {
        var entityType = typeof(TEntity).Name;
        var key = $"Entity:{entityType}:{action}:";

        return parameters.Aggregate(key, (current, parameter) => current + $"{parameter}:");
    }
}