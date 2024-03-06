using AuthenticationOptions = NinjaJournal.Microservice.Api.AspNetCore.Options.AuthenticationOptions;
using NinjaJournal.Microservice.Application.Abstractions.Services;
using NinjaJournal.Microservice.Api.AspNetCore.Filters;
using NinjaJournal.Microservice.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace NinjaJournal.Microservice.Api.AspNetCore;

public static class AspNetCoreServicesModule
{
    public static void AddAspNetCoreServicesModule(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureCorsServices(services);

        ConfigureApiVersioningServices(services);

        ConfigureRedisServices(services, configuration);
        
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddScoped<IRedisCacheService, RedisCacheService>();
        services.AddScoped<ICacheKeyService, CacheKeyService>();
        
        services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilter>();
            options.ModelValidatorProviders.Clear();
        })
            .AddNewtonsoftJson();
    }

    public static void AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        AuthenticationOptions? authenticationOptions =
            configuration.GetSection("Authentication").Get<AuthenticationOptions>();

        AuthenticationBuilder authenticationBuilder = services
            .AddAuthentication(authenticationOptions?.Scheme ?? JwtBearerDefaults.AuthenticationScheme);
    
        if (authenticationOptions?.JwtBearer is not null)
        {
            authenticationBuilder
                .AddJwtBearer(authenticationOptions.Scheme ?? JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = authenticationOptions.JwtBearer.Authority;
                    options.Audience = authenticationOptions.JwtBearer.Audience;
                    options.RequireHttpsMetadata = authenticationOptions.JwtBearer.RequireHttpsMetadata ?? true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = authenticationOptions.JwtBearer.ValidIssuer ?? authenticationOptions.JwtBearer.Authority
                    };
                });
        }
    }

    public static void AddAuthorizationServices(this IServiceCollection services)
    {
        services.AddAuthorization();
    }

    private static void ConfigureAuthorizationServices(this IServiceCollection services)
    {
        services.AddAuthorization();
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