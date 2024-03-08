using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NinjaJournal.Microservice.Api.AspNetCore.Options;

namespace NinjaJournal.Microservice.Api.AspNetCore;

public static class AspNetCoreConfigureModule
{
    public static void Configure(this WebApplication app)
    {
        ConfigureSwagger(app);
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors();

        app.MapControllers();
    }

    private static void ConfigureSwagger(WebApplication app)
    {
        var services = app.Services;
        var swaggerOptions = services.GetRequiredService<IOptions<SwaggerOptions>>().Value;

        app.UseSwagger(options =>
        {
            options.RouteTemplate = "{documentName}/swagger.json";
        });

        app.UseSwaggerUI(options =>
        {
            options.RoutePrefix = string.Empty;
            options.SwaggerEndpoint("v1/swagger.json", swaggerOptions.Version);
            options.EnableTryItOutByDefault();
            options.DefaultModelExpandDepth(-1);
            options.DisplayRequestDuration();

            if (swaggerOptions.Security?.Flow is not null)
            {
                options.OAuthClientId(swaggerOptions.Security.Flow.ClientId);
                options.OAuthAppName(swaggerOptions.Title);
                options.OAuthScopes(swaggerOptions.Security.Flow.Scopes?.Keys.ToArray() ?? Array.Empty<string>());

                if (swaggerOptions.Security.Flow.UsePkce ?? false)
                {
                    options.OAuthUsePkce();
                }
            }
        });
    }
}