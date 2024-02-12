using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

namespace NinjaJournal.Microservice.Core.Helpers;

public static class SuccessMessages
{
    public static string EntityCreated<TKey, TEntity>(TKey id) 
        where TEntity : BaseEntity<TKey>
        where TKey : struct
    {
        return $"Entity ({typeof(TEntity)}) with id: {id} has been created successfully at: {DateTime.Now}";
    }

    public static string EntityUpdated<TKey, TEntity>(TKey id)
        where TEntity : BaseEntity<TKey>
        where TKey : struct
    {
        return $"Entity ({typeof(TEntity)} with id: {id} has been updated successfully at: {DateTime.Now}";
    }
    
    public static string EntityDeleted<TKey, TEntity>(TKey id)
        where TEntity : BaseEntity<TKey>
        where TKey : struct
    {
        return $"Entity ({typeof(TEntity)} with id: {id} has been deleted successfully at: {DateTime.Now}";
    }
}