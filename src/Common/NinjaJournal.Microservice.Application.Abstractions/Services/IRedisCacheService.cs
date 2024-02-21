namespace NinjaJournal.Microservice.Application.Abstractions.Services;

public interface IRedisCacheService
{
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default);
    Task<T> GetAsync<T>(string key, CancellationToken cancellationToken);
    Task InvalidateAsync(string key, CancellationToken cancellationToken);
}