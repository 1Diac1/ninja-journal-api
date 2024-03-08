using NinjaJournal.Microservice.Api.AuthService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServicesAuthService(builder.Configuration);

var app = builder.Build();

app.PreConfigureAuthService();
app.ConfigureAuthService();

app.Run();