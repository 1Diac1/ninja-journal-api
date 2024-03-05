using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

namespace NinjaJournal.IdentityService.Application.Abstractions.Dtos;

public class ChangeUserPasswordDto<TKeyUser>
{
    public TKeyUser UserId { get; set; }
    public string OldPassword { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}