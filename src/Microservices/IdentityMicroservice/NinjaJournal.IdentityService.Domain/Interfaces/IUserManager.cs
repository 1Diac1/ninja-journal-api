using NinjaJournal.IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace NinjaJournal.IdentityService.Domain.Interfaces;

public interface IUserManager
{
    Task<ApplicationUser?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IdentityResult> CreateAsync(ApplicationUser user, string password, CancellationToken cancellationToken);
    Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string password, CancellationToken cancellationToken);
    Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken);
    Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string oldPassword, string password, CancellationToken cancellationToken);
    Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken);
    Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken);
    Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken);
}