using NinjaJournal.IdentityService.Domain.Entities;
using Ardalis.Specification;

namespace NinjaJournal.IdentityService.Domain.Specifications;

public class UserRoleGetRolesByRoleIdSpecification : Specification<UserRole>
{
    public UserRoleGetRolesByRoleIdSpecification(Guid roleId)
    {
        Query.Include(ur => ur.User)
            .Where(ur => ur.RoleId == roleId);
    }
}