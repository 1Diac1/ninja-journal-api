using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

namespace NinjaJournal.Microservice.Core.Helpers;

public class SuccessMessages
{
    public static string EntityCreated<TKey, TEntity>(TKey id) 
        where TEntity : IAggregateRoot<TKey>
        where TKey : struct
    {
        return $"Entity '{typeof(TEntity)}' with id '{id}' has been created successfully at: {DateTime.Now}";
    }

    public static string EntityUpdated<TKey, TEntity>(TKey id)
        where TEntity : IAggregateRoot<TKey>
        where TKey : struct
    {
        return $"Entity '{typeof(TEntity)}' with id '{id}' has been updated successfully at: {DateTime.Now}";
    }
    
    public static string EntityDeleted<TKey, TEntity>(TKey id)
        where TEntity : IAggregateRoot<TKey>
        where TKey : struct
    {
        return $"Entity '{typeof(TEntity)}' with id '{id}' has been deleted successfully at: {DateTime.Now}";
    }

    public static string DataRetrievedFromCache<TKey, TEntity>(TKey id, string cacheKey)
        where TEntity : IAggregateRoot<TKey>, ICacheableEntity<TKey>
        where TKey : struct
    {
        return $"Data '{typeof(TEntity)}' was retrieved from cache '{cacheKey}'";
    }

    public static string DataRetrievedFromCache<TEntity>(string cacheKey)
    {
        return $"Data '{typeof(TEntity)}' was retrieved from cache '{cacheKey}'";
    }

    public static string DataDeletedFromCache<TKey, TEntity>(TKey id, string cacheKey)
        where TEntity : IAggregateRoot<TKey>, ICacheableEntity<TKey>
        where TKey : struct
    {
        return $"Data '{typeof(TEntity)}' has been removed from the cache '{cacheKey}' at: {DateTime.Now}";
    }
}