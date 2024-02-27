using NinjaJournal.IdentityService.Domain.Interfaces;
using NinjaJournal.IdentityService.Domain.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace NinjaJournal.IdentityService.Domain;

public static class IdentityServiceDomainModule
{
    public static void AddIdentityServiceDomainModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRoleManager, RoleManager>();
    }
}