using IdentityServer4.AspNetIdentity;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using NinjaJournal.IdentityService.Application;
using NinjaJournal.IdentityService.Domain;
using NinjaJournal.IdentityService.Domain.Entities;
using NinjaJournal.IdentityService.Domain.Identity;
using NinjaJournal.IdentityService.Infrastructure.Postgresql;
using NinjaJournal.Microservice.Api.AuthService.Controllers;
using UserStore = Microsoft.AspNetCore.Identity.EntityFrameworkCore.UserStore;

namespace NinjaJournal.Microservice.Api.AuthService;

public static class AuthServiceModule
{
    public static void ConfigureServicesAuthService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityServiceDomainModule(configuration);
        services.AddIdentityServiceApplicationModule();
        services.AddIdentityServiceInfrastructureModule(configuration);
        
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddDefaultTokenProviders();

        services.AddIdentityServer(options =>
            {
                options.IssuerUri = configuration.GetValue<string>("IdentityServer:IssuerUri");
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiResources(configuration.GetSection("IdentityServer:ApiResources"))
            .AddInMemoryApiScopes(configuration.GetSection("IdentityServer:ApiScopes"))
            .AddInMemoryClients(configuration.GetSection("IdentityServer:Clients"))
            .AddAspNetIdentity<ApplicationUser>()
            .AddProfileService<ProfileService>()
            .AddDeveloperSigningCredential();
    }

    public static void PreConfigureAuthService(this IApplicationBuilder app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions()
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto
        });

        app.UseStaticFiles();
    }

    public static void ConfigureAuthService(this IApplicationBuilder app)
    {
        app.UseDeveloperExceptionPage();
        
        app.UseIdentityServer();
    }
}