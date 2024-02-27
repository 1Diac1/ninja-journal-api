using NinjaJournal.IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace NinjaJournal.IdentityService.Domain.Interfaces;

public interface IRoleManager
{
    Task<ApplicationRole?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken);
    Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken);
    Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken);
}