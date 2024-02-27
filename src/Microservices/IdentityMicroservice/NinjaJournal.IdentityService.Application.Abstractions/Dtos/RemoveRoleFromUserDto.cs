namespace NinjaJournal.IdentityService.Application.Abstractions.Dtos;

public class RemoveRoleFromUserDto<TKeyUser, TKeyRole>
{
    public TKeyUser UserId { get; set; }
    public TKeyRole RoleId { get; set; }
}