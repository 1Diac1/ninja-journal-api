namespace NinjaJournal.IdentityService.Application.Abstractions.Dtos;

public class GetUsersInRoleDto<TKeyRole>
{
    public TKeyRole RoleId { get; set; }
}