using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

namespace NinjaJournal.IdentityService.Domain.Entities;

public class UserRole : IAggregateRoot<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    public ApplicationUser User { get; set; }
    public ApplicationRole Role { get; set; }
}