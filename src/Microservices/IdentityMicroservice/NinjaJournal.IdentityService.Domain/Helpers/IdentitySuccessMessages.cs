using NinjaJournal.Microservice.Core.Helpers;

namespace NinjaJournal.IdentityService.Domain.Helpers;

public class IdentitySuccessMessages : SuccessMessages
{
    public static string RoleRemovedFromUser<TKeyUser, TKeyRole>(TKeyUser userId, TKeyRole roleId) 
        where TKeyUser : struct
        where TKeyRole : struct
    {
        return $"Role with id: ({roleId}) has been removed from user with id: ({userId}) successfully at: {DateTime.Now}";
    }
}