﻿using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.Microservice.Infrastructure.EntityFrameworkCore;
using NinjaJournal.StudentsManagement.Infrastructure.Postgresql;
using NinjaJournal.StudentsManagement.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace NinjaJournal.StudentsManagement.Application;

public static class StudentsManagementApplicationModule
{
    public static void AddStudentsManagementApplicationModule(this IServiceCollection services)
    {
        services.AddScoped<IEntityRepository<Guid, Student>, BaseEntityRepository<Guid, Student, StudentsManagementDbContext>>();
        services.AddScoped<IReadEntityRepository<Guid, Student>, BaseReadEntityRepository<Guid, Student, StudentsManagementDbContext>>();
        
        services.AddScoped<IEntityRepository<Guid, Group>, BaseEntityRepository<Guid, Group, StudentsManagementDbContext>>();
        services.AddScoped<IReadEntityRepository<Guid, Group>, BaseReadEntityRepository<Guid, Group, StudentsManagementDbContext>>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}