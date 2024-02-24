namespace NinjaJournal.Microservice.Api.AspNetCore.Options;

public class AuthenticationOptions
{
    public string? Scheme { get; set; }
    public JwtBearerOptions? JwtBearer { get; set; }
}