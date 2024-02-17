using Ardalis.GuardClauses;

namespace NinjaJournal.Microservice.Api.AspNetCore.Extensions;

public static class GuardExtensions
{
    public static void NotFoundEntity<TKey, T>(this IGuardClause guardClause, TKey key, T? input) 
        where TKey : struct
        where T : class
    {
        guardClause.Null(key, nameof(key));

        if (input is null)
            throw new Core.Exceptions.NotFoundException(nameof(input), key);
    }
}