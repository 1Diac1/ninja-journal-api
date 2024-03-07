using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

namespace NinjaJournal.Microservice.Core.Helpers;

public static class ErrorMessages
{
    public const string CantBeNullOrEmpty = "Parameter can't be null or empty";
    public const string EntityNotFound = "Entity with this Id was not found";
    
    public static string EntityPropertyCannotBeNullOrEmpty<TKey, TEntity>(string property)
        where TEntity : IAggregateRoot<TKey>
        where TKey : struct
    {
        return $"Property '{property}' for entity '{typeof(TEntity)}' can't be null";
    }
}