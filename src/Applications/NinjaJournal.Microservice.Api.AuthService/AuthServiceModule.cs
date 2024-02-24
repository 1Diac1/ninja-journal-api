using IdentityServer4.AspNetIdentity;
using Microsoft.AspNetCore.Identity;

namespace NinjaJournal.Microservice.Api.AuthService;

public static class AuthServiceModule
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        
        // services.AddIdentity<ApplicationUser, ApplicationRole>()
        //     .AddDefaultTokenProviders();

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
            //.AddAspNetIdentity<ApplicationUser>()
            .AddDeveloperSigningCredential();
    }
}