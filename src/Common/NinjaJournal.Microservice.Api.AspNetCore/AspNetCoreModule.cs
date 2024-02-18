using NinjaJournal.Microservice.Api.AspNetCore.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace NinjaJournal.Microservice.Api.AspNetCore;

public static class AspNetCoreModule
{
    public static void AddAspNetCoreModule(this IServiceCollection services)
    {
        ConfigureCors(services);

        ConfigureApiVersioning(services);

        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);
        
        services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilter>();
            options.ModelValidatorProviders.Clear();
        })
            .AddNewtonsoftJson();
    }

    private static void ConfigureCors(IServiceCollection services)
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

    private static void ConfigureApiVersioning(IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });
    }
}