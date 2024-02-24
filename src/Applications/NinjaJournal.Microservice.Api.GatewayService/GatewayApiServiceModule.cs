using NinjaJournal.Microservice.Api.AspNetCore;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace NinjaJournal.Microservice.Api.GatewayService;

public static class GatewayApiServiceModule
{
    public static void AddGatewayApiServiceModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthenticationServices(configuration);
        
        services.AddControllers();

        services.AddOcelot();
        
        services.AddAuthenticationServices(configuration);
    }

    public static async Task ConfigureGatewayApiService(this WebApplication app)
    {
        app.ConfigureAuthentication();
        
        await app.UseOcelot();
    }
}