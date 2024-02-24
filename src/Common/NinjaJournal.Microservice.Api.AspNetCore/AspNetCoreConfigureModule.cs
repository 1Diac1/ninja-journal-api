using Microsoft.AspNetCore.Builder;

namespace NinjaJournal.Microservice.Api.AspNetCore;

public static class AspNetCoreConfigureModule
{
    public static void ConfigureAuthentication(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors();

        app.MapControllers();
    }
}