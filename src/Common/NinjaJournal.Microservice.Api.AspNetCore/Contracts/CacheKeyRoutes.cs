namespace NinjaJournal.Microservice.Api.AspNetCore.Contracts;

public static class CacheKeyRoutes
{
    private const string Version = "v1";
    private const string Base = $"{Version}";

    public const string GetAll = Base + ":GetAll";
    public const string Get = Base + ":Get";

    public static class IdentityService
    { }
}