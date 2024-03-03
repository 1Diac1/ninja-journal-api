using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;

namespace NinjaJournal.IdentityService.Domain.Entities;

public class UserRole : BaseEntity<Guid>
{
    public Guid UserId { get; protected set; }
    public Guid RoleId { get; protected set; }
    
    protected internal UserRole(Guid userId, Guid roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }
}