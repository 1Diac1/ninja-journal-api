using AuthenticationOptions = NinjaJournal.Microservice.Api.AspNetCore.Options.AuthenticationOptions;
using NinjaJournal.Microservice.Application.Abstractions.Services;
using NinjaJournal.Microservice.Api.AspNetCore.Filters;
using NinjaJournal.Microservice.Api.AspNetCore.Options;
using NinjaJournal.Microservice.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

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

        services.Configure<CacheOptions>(configuration.GetSection("CacheOptions"));
        
        services.AddScoped<IRedisCacheService, RedisCacheService>();
        services.AddScoped<ICacheKeyService, CacheKeyService>();

        ConfigureAuthorizationServices(services);
        ConfigureAuthenticationServices(services, configuration);
        
        services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilter>();
            options.ModelValidatorProviders.Clear();
        })
            .AddNewtonsoftJson();

        ConfigureSwaggerModule(services, configuration);
    }

    private static void ConfigureAuthenticationServices(IServiceCollection services, IConfiguration configuration)
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

    private static void ConfigureAuthorizationServices(IServiceCollection services)
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

    private static void ConfigureSwaggerModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<SwaggerOptions>()
            .Bind(configuration.GetSection(SwaggerOptions.Swagger))
            .ValidateDataAnnotations();

        var swaggerOptions = services.BuildServiceProvider().GetRequiredService<IOptions<SwaggerOptions>>().Value;

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                swaggerOptions.Version,
                new OpenApiInfo()
                {
                    Title = swaggerOptions.Title,
                    Version = swaggerOptions.Version
                });

            options.DocInclusionPredicate((docName, description) => true);
            options.TagActionsBy(x => new[] { x.GroupName });

            if (swaggerOptions.Security?.Flow is not null)
            {
                options.AddSecurityDefinition(swaggerOptions.Security.Name,
                    new OpenApiSecurityScheme()
                    {
                        Type = swaggerOptions.Security.Type.GetValueOrDefault(),
                        Scheme = swaggerOptions.Security.Scheme,
                        Flows = GetOpenApiOAuthFlows(swaggerOptions.Security.Flow)
                    });
                
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = swaggerOptions.Security.Name,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        swaggerOptions.Security.Flow.Scopes?.Keys.ToArray() ?? Array.Empty<string>()
                    }
                });
            }
        });
    }
    
    private static OpenApiOAuthFlows GetOpenApiOAuthFlows(SwaggerSecurityFlow flow)
    {
        OpenApiOAuthFlows result = new();
        switch (flow.GrantType)
        {
            case GrantTypes.AuthorizationCode:
                result.AuthorizationCode = GetOpenApiOAuthFlow(flow);
                break;
            case GrantTypes.ClientCredentials:
                result.ClientCredentials = GetOpenApiOAuthFlow(flow);
                break;
            case GrantTypes.Implicit:
                result.Implicit = GetOpenApiOAuthFlow(flow);
                break;
            case GrantTypes.Password:
                result.Password = GetOpenApiOAuthFlow(flow);
                break;
        };
        return result;
    }
    
    private static OpenApiOAuthFlow GetOpenApiOAuthFlow(SwaggerSecurityFlow flow)
    {
        return new OpenApiOAuthFlow
        {
            AuthorizationUrl = GetUri(flow.AuthorityUrl, flow.AuthorizationUrl),
            TokenUrl = GetUri(flow.AuthorityUrl, flow.TokenUrl),
            RefreshUrl = GetUri(flow.AuthorityUrl, flow.RefreshUrl),
            Scopes = flow.Scopes
        };
    }

    private static Uri? GetUri(string? baseUrl, string? segment)
    {
        if (string.IsNullOrEmpty(segment))
        {
            return null;
        }
        if (string.IsNullOrEmpty(baseUrl) || segment.StartsWith(baseUrl))
        {
            return new Uri(segment);
        }
        string url = string.Join("/", new[] { baseUrl.TrimEnd('/'), segment.Trim('/') });
        return new Uri(url);
    }
}






