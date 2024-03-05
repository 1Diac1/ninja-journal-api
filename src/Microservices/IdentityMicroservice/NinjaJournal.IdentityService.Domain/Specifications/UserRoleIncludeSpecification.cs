using NinjaJournal.IdentityService.Domain.Entities;
using Ardalis.Specification;

namespace NinjaJournal.IdentityService.Domain.Specifications;

public class UserRoleIncludeSpecification : Specification<UserRole>
{
    public UserRoleIncludeSpecification(Guid userId)
    {
        Query.Where(ur => ur.UserId == userId)
            .Include(ur => ur.Role);
    }
}