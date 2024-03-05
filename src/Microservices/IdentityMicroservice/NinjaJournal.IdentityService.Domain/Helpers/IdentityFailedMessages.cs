using NinjaJournal.Microservice.Core.Helpers;

namespace NinjaJournal.IdentityService.Domain.Helpers;

public class IdentityFailedMessages : SuccessMessages
{
    public static string RoleNotFound(string roleName)
    {
        return $"Role '{roleName}' doesn't exist";
    }

    public static string RoleAlreadyExists(string roleName)
    {
        return $"Role '{roleName}' already exists for this user";
    }
}