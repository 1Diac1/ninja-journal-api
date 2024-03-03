using NinjaJournal.Microservice.Infrastructure.Abstractions.Models;
using Microsoft.AspNetCore.Identity;

namespace NinjaJournal.IdentityService.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>, IAggregateRoot<Guid>
{
    public Guid StudentId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; init; }

    public virtual ICollection<UserRole> Roles { get; protected set; } = new List<UserRole>();

    public void AddRole(Guid roleId)
    {
        if (IsInRole(roleId) is true)
            return;

        Roles.Add(new UserRole(Id, roleId));
    }

    public void RemoveRole(Guid roleId)
    {
        if (IsInRole(roleId) is false)
            return;

        Roles.Where(r => r.RoleId == roleId).ToList()
            .ForEach(r => Roles.Remove(r));
    }
    
    public bool IsInRole(Guid roleId)
    {
        return Roles.Any(r => r.RoleId == roleId);
    }
}