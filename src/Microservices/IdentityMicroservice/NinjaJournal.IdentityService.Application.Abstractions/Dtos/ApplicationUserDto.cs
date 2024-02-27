using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

namespace NinjaJournal.IdentityService.Application.Abstractions.Dtos;

public class ApplicationUserDto : BaseEntityDto<Guid>
{
    public string UserName { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}