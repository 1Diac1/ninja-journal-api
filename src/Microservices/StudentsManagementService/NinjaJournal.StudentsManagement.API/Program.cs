using NinjaJournal.Microservice.Common.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilog();

builder.Services.AddControllers();

var app = builder.Build(); 

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();