using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

namespace NinjaJournal.IdentityService.Application.Dtos;

public class ApplicationUserDto : BaseEntityDto<Guid>
{
    public string UserName { get; set; }
}