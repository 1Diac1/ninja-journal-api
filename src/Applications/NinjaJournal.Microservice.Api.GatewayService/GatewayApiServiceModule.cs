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
    }

    public static async Task ConfigureGatewayApiService(this WebApplication app)
    {
        app.Configure();
        
        await app.UseOcelot();
    }
}