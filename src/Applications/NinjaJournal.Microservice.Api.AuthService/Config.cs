using IdentityServer4.Models;
using IdentityModel;

namespace NinjaJournal.Microservice.Api.AuthService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResources.Address(),
            new IdentityResources.Phone(),
            new IdentityResource("role", new []{ JwtClaimTypes.Role })
        };
}