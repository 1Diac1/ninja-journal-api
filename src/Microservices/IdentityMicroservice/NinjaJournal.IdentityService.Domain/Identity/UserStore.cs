using Microsoft.AspNetCore.Identity;
using NinjaJournal.IdentityService.Domain.Entities;
using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;

namespace NinjaJournal.IdentityService.Domain.Identity;

public class UserStore : IUserStore<ApplicationUser>,
    IUserEmailStore<ApplicationUser>,
    IUserPasswordStore<ApplicationUser>,
    IUserRoleStore<ApplicationUser>
{
    private readonly IReadEntityRepository<Guid, ApplicationUser> _userReadRepository;
    private readonly IEntityRepository<Guid, ApplicationUser> _userRepository;
    private readonly IRoleStore<ApplicationRole> _roleStore;
    private bool disposed;


    public UserStore(IReadEntityRepository<Guid, ApplicationUser> userReadRepository, 
        IEntityRepository<Guid, ApplicationUser> userRepository, IRoleStore<ApplicationRole> roleStore)
    {
        _userReadRepository = userReadRepository;
        _userRepository = userRepository;
        _roleStore = roleStore;
    }

    public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        cancellationToken.ThrowIfCancellationRequested();
        
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        cancellationToken.ThrowIfCancellationRequested();
        
        return Task.FromResult<string?>(user.UserName);
    }

    public Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(userName, nameof(userName));

        cancellationToken.ThrowIfCancellationRequested();

        user.UserName = userName;
        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        cancellationToken.ThrowIfCancellationRequested();
        
        return Task.FromResult<string?>(user.NormalizedUserName);
    }

    public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(normalizedName, nameof(normalizedName));

        cancellationToken.ThrowIfCancellationRequested();
        
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        cancellationToken.ThrowIfCancellationRequested();
        
        await _userRepository.AddAsync(user, true, cancellationToken);

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        
        cancellationToken.ThrowIfCancellationRequested();
        
        await _userRepository.UpdateAsync(user, true, cancellationToken);

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        cancellationToken.ThrowIfCancellationRequested();
        
        await _userRepository.DeleteAsync(user, true, cancellationToken);

        return IdentityResult.Success;
    }

    public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId, nameof(userId));

        cancellationToken.ThrowIfCancellationRequested();
        
        return await _userReadRepository.GetByIdAsync(Guid.Parse(userId), true, cancellationToken);
    }

    public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(normalizedUserName, nameof(normalizedUserName));

        cancellationToken.ThrowIfCancellationRequested();
        
        return await _userReadRepository.GetAsync(u => u.NormalizedUserName == normalizedUserName
            , true, cancellationToken);
    }

    public Task SetEmailAsync(ApplicationUser user, string? email, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(email, nameof(email));

        cancellationToken.ThrowIfCancellationRequested();
        
        user.Email = email;
        return Task.CompletedTask;
    }

    public Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        cancellationToken.ThrowIfCancellationRequested();
        
        return Task.FromResult<string?>(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        
        
        return Task.FromResult(user.EmailConfirmed);
    }

    public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(normalizedEmail, nameof(normalizedEmail));

        return await _userReadRepository.GetAsync(u => u.NormalizedEmail == normalizedEmail, true, cancellationToken);
    }

    public Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        return Task.FromResult<string?>(user.NormalizedEmail);
    }

    public Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(normalizedEmail, nameof(normalizedEmail));

        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }

    public Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(passwordHash, nameof(passwordHash));

        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        return Task.FromResult<string?>(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        return Task.FromResult(user.PasswordHash != null);
    }

    public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(roleName, nameof(roleName));

        ApplicationRole? role = await _roleStore.FindByNameAsync(roleName, cancellationToken);
        
        if (role is null)
        {
            throw new InvalidOperationException($"Role '{roleName}' doesn't exist");
        }

        user.AddRole(role.Id);
    }

    public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        List<string> roles = new();

        foreach (var userRole in user.Roles)
        {
            var role = await _roleStore.FindByIdAsync(userRole.RoleId.ToString(), cancellationToken);

            if (role is not null)
                roles.Add(role.Name);
        }

        return roles;
    }

    public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
        {
            return;
        }
        disposed = true;
    }

    ~UserStore()
    {
        Dispose(disposing: false);
    }
}