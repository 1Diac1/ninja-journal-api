using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using NinjaJournal.IdentityService.Domain.Entities;

namespace NinjaJournal.Microservice.Api.AuthService.Controllers;

internal class ProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileService(UserManager<ApplicationUser> userManager)
    {
        ArgumentNullException.ThrowIfNull(userManager, nameof(userManager));

        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        ApplicationUser user = await _userManager.GetUserAsync(context.Subject);

        if (user == null)
        {
            return;
        }

        List<Claim> userClaims = new()
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.GivenName, user.FirstName),
            new Claim(ClaimTypes.Surname, user.LastName),
            new Claim(ClaimTypes.Email, user.Email!),
        };

        context.IssuedClaims = userClaims;
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }
}