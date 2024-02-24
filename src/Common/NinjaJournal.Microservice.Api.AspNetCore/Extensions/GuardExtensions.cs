using Ardalis.GuardClauses;
using NinjaJournal.Microservice.Core.Helpers;

namespace NinjaJournal.Microservice.Api.AspNetCore.Extensions;

public static class GuardExtensions
{
    public static void NotFoundEntity<TKey, T>(this IGuardClause guardClause, TKey key, T? input) 
        where TKey : struct
    {
        guardClause.Null(key, nameof(key));

        if (input is null)
        {
            throw new Core.Exceptions.NotFoundException(typeof(T).Name, key);
        }
    }

    public static void NullOrEmpty<T>(this IGuardClause guardClause, T? input, string? parameterName = null, string? message = null)
    {
        if (input is null)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new ArgumentNullException(parameterName);
            }

            throw new ArgumentNullException(parameterName, message);
        }
        
        if (typeof(T) == typeof(Guid))
        {
            Guid myGuid = (Guid)(object)input;

            if (myGuid == Guid.Empty)
            {
                throw new ArgumentException(message, parameterName);
            }
        }
    }
}