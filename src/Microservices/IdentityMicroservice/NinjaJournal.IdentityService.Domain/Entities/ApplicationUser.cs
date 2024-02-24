using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;
using Microsoft.AspNetCore.Identity;

namespace NinjaJournal.IdentityService.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid StudentId { get; set; }
    public BaseEntity<Guid> BaseEntity { get; set; }
}