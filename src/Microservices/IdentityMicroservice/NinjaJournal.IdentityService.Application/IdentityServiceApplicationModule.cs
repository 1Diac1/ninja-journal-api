using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.Microservice.Infrastructure.EntityFrameworkCore;
using NinjaJournal.IdentityService.Infrastructure.Postgresql;
using NinjaJournal.IdentityService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace NinjaJournal.IdentityService.Application;

public static class IdentityServiceApplicationModule
{
    public static void AddIdentityServiceApplicationModule(this IServiceCollection services)
    {
        services.AddScoped<IEntityRepository<Guid, ApplicationUser>, BaseEntityRepository<Guid, ApplicationUser, IdentityServiceDbContext>>();
        services.AddScoped<IReadEntityRepository<Guid, ApplicationUser>, BaseReadEntityRepository<Guid, ApplicationUser, IdentityServiceDbContext>>();
        
        services.AddScoped<IEntityRepository<Guid, ApplicationRole>, BaseEntityRepository<Guid, ApplicationRole, IdentityServiceDbContext>>();
        services.AddScoped<IReadEntityRepository<Guid, ApplicationRole>, BaseReadEntityRepository<Guid, ApplicationRole, IdentityServiceDbContext>>();

        services.AddScoped<IEntityRepository<Guid, UserRole>, BaseEntityRepository<Guid, UserRole, IdentityServiceDbContext>>();
        services.AddScoped<IReadEntityRepository<Guid, UserRole>, BaseReadEntityRepository<Guid, UserRole, IdentityServiceDbContext>>();
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}