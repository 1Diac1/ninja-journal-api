namespace NinjaJournal.IdentityService.Application.Abstractions.Dtos;

public class GetUserRolesDto<TKeyUser>
{
    public TKeyUser UserId { get; set; }
}