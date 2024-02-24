using NinjaJournal.Microservice.Application.Abstractions.Services;
using NinjaJournal.Microservice.Api.AspNetCore.Filters;
using NinjaJournal.Microservice.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace NinjaJournal.Microservice.Api.AspNetCore;

public static class AspNetCoreModule
{
    public static void AddAspNetCoreModule(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureCorsServices(services);

        ConfigureApiVersioningServices(services);

        ConfigureRedisServices(services, configuration);
        
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddScoped<IRedisCacheService, RedisCacheService>();
        
        services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilter>();
            options.ModelValidatorProviders.Clear();
        })
            .AddNewtonsoftJson();
    }

    private static void ConfigureAuthenticationServices(this IServiceCollection services)
    {
        
    }
    
    private static void ConfigureRedisServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });
    }
    
    private static void ConfigureCorsServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(x =>
            {
                x
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    private static void ConfigureApiVersioningServices(IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });
    }
}