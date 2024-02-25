using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NinjaJournal.IdentityService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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
            .AddEntityFrameworkStores<IdentityServiceDbContext>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddRoleManager<RoleManager<ApplicationRole>>();
    }
}