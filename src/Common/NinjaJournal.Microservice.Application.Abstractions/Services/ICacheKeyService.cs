using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

namespace NinjaJournal.Microservice.Application.Abstractions.Services;

public interface ICacheKeyService
{
    string GenerateCacheKey<TKey, TEntity>(string action, params object[] parameters)
        where TKey : struct
        where TEntity : IAggregateRoot<TKey>, ICacheableEntity<TKey>;
}