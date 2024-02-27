using NinjaJournal.Microservice.Core.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace NinjaJournal.IdentityService.Domain.Extensions;

public static class IdentityResultExtensions
{
    public static void Check(this IdentityResult result)
    {
        if (result.Succeeded is true)
            return;

        var errors = result.Errors
            .Select(e => e.Description);

        throw new BadRequestException(errors);
    }
}