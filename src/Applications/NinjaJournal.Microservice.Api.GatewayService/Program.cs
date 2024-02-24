using NinjaJournal.Microservice.Api.GatewayService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGatewayApiServiceModule(builder.Configuration);

var app = builder.Build();

app.MapControllers();

await app.ConfigureGatewayApiService();

app.Run();