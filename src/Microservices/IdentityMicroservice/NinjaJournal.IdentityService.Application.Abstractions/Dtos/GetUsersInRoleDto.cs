namespace NinjaJournal.IdentityService.Application.Abstractions.Dtos;

public class GetUsersInRoleDto<TKey>
{
    public TKey RoleId { get; set; }
}