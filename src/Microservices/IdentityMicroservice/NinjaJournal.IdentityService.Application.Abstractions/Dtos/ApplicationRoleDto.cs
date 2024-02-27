using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

namespace NinjaJournal.IdentityService.Application.Abstractions.Dtos;

public class ApplicationRoleDto : BaseEntityDto<Guid>
{
    public string Name { get; set; }
}