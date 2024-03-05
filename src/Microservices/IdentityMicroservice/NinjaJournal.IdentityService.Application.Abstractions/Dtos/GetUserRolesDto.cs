namespace NinjaJournal.IdentityService.Application.Abstractions.Dtos;

public class GetUserRolesDto<TKey>
{
    public TKey UserId { get; set; }
}