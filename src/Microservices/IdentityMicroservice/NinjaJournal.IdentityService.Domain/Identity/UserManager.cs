using NinjaJournal.IdentityService.Domain.Interfaces;
using NinjaJournal.IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NinjaJournal.IdentityService.Domain.Identity;

public class UserManager : UserManager<ApplicationUser>, IUserManager
{
    private CancellationToken _cancellationToken = CancellationToken.None;
    
    public UserManager(IUserStore<ApplicationUser> store, 
        IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, 
        IEnumerable<IUserValidator<ApplicationUser>> userValidators, 
        IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, 
        ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, 
        IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) 
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    { }

    protected override CancellationToken CancellationToken => _cancellationToken;

    public async Task<ApplicationUser?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        return await FindByIdAsync(id.ToString());
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password, CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        return await CreateAsync(user, password);
    }

    public async Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string password, CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        return await AddPasswordAsync(user, password);
    }

    public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        return await UpdateAsync(user);
    }

    public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string oldPassword, string password,
        CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        return await ChangePasswordAsync(user, oldPassword, password);
    }

    public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        return await AddToRoleAsync(user, roleName);
    }

    public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        return await RemoveFromRoleAsync(user, roleName);
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
        return await DeleteAsync(user);
    }
}