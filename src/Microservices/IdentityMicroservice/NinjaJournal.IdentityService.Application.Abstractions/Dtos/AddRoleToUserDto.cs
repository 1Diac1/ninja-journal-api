namespace NinjaJournal.IdentityService.Application.Abstractions.Dtos;

public class AddRoleToUserDto<TKeyUser, TKeyRole> 
{
    public TKeyUser UserId { get; set; }
    public TKeyRole RoleId { get; set; }
}