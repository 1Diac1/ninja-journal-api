using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;
using System.Text.Json.Serialization;

namespace NinjaJournal.IdentityService.Application.Abstractions.Dtos;

public class CreateApplicationRoleDto : BaseEntityDto<Guid>
{
    [JsonIgnore]
    public override Guid Id { get; set; }
    public string Name { get; set; }
}