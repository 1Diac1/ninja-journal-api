using NinjaJournal.StudentsManagement.Infrastructure.Postgresql;
using NinjaJournal.Microservice.Api.AspNetCore.Extensions;
using NinjaJournal.StudentsManagement.Application;
using NinjaJournal.Microservice.Api.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStudentsManagementInfrastructureModule(builder.Configuration);
builder.Services.AddStudentsManagementApplicationModule();

builder.AddSerilog();
builder.Services.AddAspNetCoreServicesModule(builder.Configuration);

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();