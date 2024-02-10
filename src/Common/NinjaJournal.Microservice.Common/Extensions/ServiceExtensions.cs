using Microsoft.AspNetCore.Builder;
using NinjaJournal.Common.Logging;
using Serilog;

namespace NinjaJournal.Microservice.Common.Extensions;

public static class ServiceExtensions
{
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        LoggingSetup.ConfigureLogging(builder.Configuration);
        builder.Host.UseSerilog();
        
        return builder;
    }
}