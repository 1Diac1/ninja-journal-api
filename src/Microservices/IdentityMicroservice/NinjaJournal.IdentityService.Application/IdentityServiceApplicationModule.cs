using Microsoft.Extensions.DependencyInjection;
using NinjaJournal.IdentityService.Domain.Entities;
using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;

namespace NinjaJournal.IdentityService.Application;

public static class IdentityServiceApplicationModule
{
    public static void AddIdentityServiceApplicationModule(this IServiceCollection services)
    {
        services.AddScoped<IEntityRepository<Guid, new ApplicationUser().BaseEntity>();
    }
}