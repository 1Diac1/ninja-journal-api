using NinjaJournal.Microservice.Api.AspNetCore;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace NinjaJournal.Microservice.Api.GatewayService;

public static class GatewayApiServiceModule
{
    public static void AddGatewayApiServiceModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAspNetCoreServicesModule(configuration);
        
        services.AddControllers();

        services.AddOcelot();
    }

    public static async Task ConfigureGatewayApiService(this WebApplication app)
    {
        await app.UseOcelot();
    }
}