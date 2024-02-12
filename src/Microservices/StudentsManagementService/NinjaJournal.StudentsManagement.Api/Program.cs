using NinjaJournal.Microservice.Core.Extensions;
using Serilog;

var builder = WebApplicatio n.CreateBuilder(args);

builder.AddSerilog();

builder.Services.AddControllers();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();