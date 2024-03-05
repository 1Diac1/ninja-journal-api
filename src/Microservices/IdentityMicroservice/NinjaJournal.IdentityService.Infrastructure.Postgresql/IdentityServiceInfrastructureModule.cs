using NinjaJournal.IdentityService.Domain.Entities;
using NinjaJournal.IdentityService.Domain.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace NinjaJournal.IdentityService.Infrastructure.Postgresql;

public static class IdentityServiceInfrastructureModule
{
    public static void AddIdentityServiceInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityServiceDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString(nameof(IdentityServiceDbContext))));

        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 8;
            })
            .AddRoles<ApplicationRole>()
            .AddUserStore<UserStore>()
            .AddRoleStore<RoleStore>()
            .AddUserManager<UserManager>()
            .AddRoleManager<RoleManager>();
    }
}