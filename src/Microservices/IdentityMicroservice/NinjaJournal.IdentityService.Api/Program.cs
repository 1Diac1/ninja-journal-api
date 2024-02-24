using NinjaJournal.IdentityService.Infrastructure.Postgresql;
using NinjaJournal.Microservice.Api.AspNetCore.Extensions;
using NinjaJournal.IdentityService.Application;
using NinjaJournal.Microservice.Api.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServiceInfrastructureModule(builder.Configuration);
builder.Services.AddIdentityServiceApplicationModule();

builder.AddSerilog();
builder.Services.AddAspNetCoreServicesModule(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();