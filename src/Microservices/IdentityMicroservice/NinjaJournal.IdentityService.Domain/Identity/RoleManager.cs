using NinjaJournal.IdentityService.Domain.Interfaces;
using NinjaJournal.IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace NinjaJournal.IdentityService.Domain.Identity;

public class RoleManager : RoleManager<ApplicationRole>, IRoleManager
{
    private CancellationToken _cancellationToken = CancellationToken.None;

    public RoleManager(IRoleStore<ApplicationRole> store,
        IEnumerable<IRoleValidator<ApplicationRole>> roleValidators,
        ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
        ILogger<RoleManager<ApplicationRole>> logger)
        : base(store, roleValidators, keyNormalizer, errors, logger)
    { }

    protected override CancellationToken CancellationToken => _cancellationToken;
    
    public async Task<ApplicationRole?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        return await FindByIdAsync(id.ToString());
    }

    public async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        return await CreateAsync(role);
    }

    public async Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        return await UpdateAsync(role);
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        return await DeleteAsync(role);
    }
}