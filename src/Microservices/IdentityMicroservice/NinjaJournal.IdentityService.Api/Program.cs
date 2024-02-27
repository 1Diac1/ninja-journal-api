using NinjaJournal.IdentityService.Infrastructure.Postgresql;
using NinjaJournal.Microservice.Api.AspNetCore.Extensions;
using NinjaJournal.Microservice.Api.AspNetCore;
using NinjaJournal.IdentityService.Application;
using NinjaJournal.IdentityService.Domain;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServiceDomainModule(builder.Configuration);
builder.Services.AddIdentityServiceInfrastructureModule(builder.Configuration);
builder.Services.AddIdentityServiceApplicationModule();

builder.AddSerilog();
builder.Services.AddAspNetCoreServicesModule(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();