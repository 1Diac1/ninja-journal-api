using NinjaJournal.Microservice.Core.Helpers;

namespace NinjaJournal.IdentityService.Domain.Helpers;

public class IdentitySuccessMessages : SuccessMessages
{
    public static string RoleRemovedFromUser<TKeyUser, TKeyRole>(TKeyUser userId, TKeyRole roleId) 
        where TKeyUser : struct
        where TKeyRole : struct
    {
        return $"Role '{roleId}' has been removed from user '{userId}' successfully at: {DateTime.Now}";
    }

    public static string AddRoleToUser<TKeyUser, TKeyRole>(TKeyUser userId, TKeyRole roleId)
    {
        return $"Role '{roleId}' has been added to the user '{userId}' at: {DateTime.Now}";
    }
    
    public static string PasswordChanged<TKeyUser>(TKeyUser userId)
    {
        return $"User '{userId}' password was successfully updated at: {DateTime.Now}";
    }
}