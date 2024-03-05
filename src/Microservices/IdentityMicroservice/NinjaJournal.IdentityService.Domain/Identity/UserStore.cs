using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.IdentityService.Domain.Entities;
using NinjaJournal.IdentityService.Domain.Helpers;
using NinjaJournal.Microservice.Core.Exceptions;
using NinjaJournal.Microservice.Core.Helpers;
using Microsoft.AspNetCore.Identity;
using NinjaJournal.IdentityService.Domain.Specifications;

namespace NinjaJournal.IdentityService.Domain.Identity;

public class UserStore : IUserStore<ApplicationUser>,
    IUserEmailStore<ApplicationUser>,
    IUserPasswordStore<ApplicationUser>,
    IUserRoleStore<ApplicationUser>
{
    private readonly IReadEntityRepository<Guid, ApplicationUser> _userReadRepository;
    private readonly IReadEntityRepository<Guid, ApplicationRole> _roleReadRepository;
    private readonly IReadEntityRepository<Guid, UserRole> _userRoleReadRepository;
    private readonly IEntityRepository<Guid, UserRole> _userRoleRepository;
    private readonly IEntityRepository<Guid, ApplicationUser> _userRepository;
    private readonly IRoleStore<ApplicationRole> _roleStore;
    private bool disposed;
    
    public UserStore(IReadEntityRepository<Guid, ApplicationUser> userReadRepository, 
        IEntityRepository<Guid, ApplicationUser> userRepository, IRoleStore<ApplicationRole> roleStore, 
        IEntityRepository<Guid, UserRole> userRoleRepository, IReadEntityRepository<Guid, UserRole> userRoleReadRepository, 
        IReadEntityRepository<Guid, ApplicationRole> roleReadRepository)
    {
        ArgumentNullException.ThrowIfNull(userReadRepository, nameof(userReadRepository));
        ArgumentNullException.ThrowIfNull(userRepository, nameof(userRepository));
        ArgumentNullException.ThrowIfNull(roleStore, nameof(roleStore));
        ArgumentNullException.ThrowIfNull(userRoleRepository, nameof(userRoleRepository));
        ArgumentNullException.ThrowIfNull(userRoleReadRepository, nameof(userRoleReadRepository));
        ArgumentNullException.ThrowIfNull(roleReadRepository, nameof(roleReadRepository));
        
        _userReadRepository = userReadRepository;
        _userRepository = userRepository;
        _roleStore = roleStore;
        _userRoleRepository = userRoleRepository;
        _userRoleReadRepository = userRoleReadRepository;
        _roleReadRepository = roleReadRepository;
    }

    public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(user, nameof(user));
        
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(user, nameof(user));
        
        return Task.FromResult<string?>(user.UserName);
    }

    public Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(userName, nameof(userName));

        user.UserName = userName;
        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(user, nameof(user));
        
        return Task.FromResult<string?>(user.NormalizedUserName);
    }

    public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(normalizedName, nameof(normalizedName));
        
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(user, nameof(user));
        
        await _userRepository.AddAsync(user, true, cancellationToken);

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        user.ConcurrencyStamp = Guid.NewGuid().ToString();
        
        await _userRepository.UpdateAsync(user, true, cancellationToken);

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(user, nameof(user));
        
        await _userRepository.DeleteAsync(user, true, cancellationToken);

        return IdentityResult.Success;
    }

    public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(userId, nameof(userId));
        
        return await _userReadRepository.GetByIdAsync(Guid.Parse(userId), true, cancellationToken);
    }

    public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(normalizedUserName, nameof(normalizedUserName));
        
        return await _userReadRepository.GetAsync(u => u.NormalizedUserName == normalizedUserName
            , true, cancellationToken);
    }

    public Task SetEmailAsync(ApplicationUser user, string? email, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(email, nameof(email));
        
        user.Email = email;
        return Task.CompletedTask;
    }

    public Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(user, nameof(user));
        
        return Task.FromResult<string?>(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        
        return Task.FromResult(user.EmailConfirmed);
    }

    public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ArgumentNullException.ThrowIfNull(normalizedEmail, nameof(normalizedEmail));

        return await _userReadRepository.GetAsync(u => u.NormalizedEmail == normalizedEmail, true, cancellationToken);
    }

    public Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        return Task.FromResult<string?>(user.NormalizedEmail);
    }

    public Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(normalizedEmail, nameof(normalizedEmail));

        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }

    public Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        ArgumentNullException.ThrowIfNull(passwordHash, nameof(passwordHash));

        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        return Task.FromResult<string?>(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        return Task.FromResult(user.PasswordHash != null);
    }

    public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        
        if (string.IsNullOrWhiteSpace(roleName))
        {
            throw new ArgumentNullException(nameof(roleName));
        }

        ApplicationRole? role = await _roleStore.FindByNameAsync(roleName, cancellationToken);
        
        if (role is null)
        {
            throw new BadRequestException(IdentityFailedMessages.RoleNotFound(roleName));
        }

        var userRole = CreateUserRole(user.Id, role.Id);
        await _userRoleRepository.AddAsync(userRole, true, cancellationToken);
    }

    private UserRole CreateUserRole(Guid userId, Guid roleId)
    {
        return new UserRole()
        {
            UserId = userId,
            RoleId = roleId
        };
    }

    public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        
        if (string.IsNullOrWhiteSpace(roleName))
        {
            throw new ArgumentNullException(nameof(roleName));
        }

        ApplicationRole? role = await _roleStore.FindByNameAsync(roleName, cancellationToken);
        
        if (role is null)
        {
            throw new BadRequestException(IdentityFailedMessages.RoleNotFound(roleName));
        }

        var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);

        if (userRole is not null)
        {
            await _userRoleRepository.DeleteAsync(userRole, true, cancellationToken);
        }
    }

    public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        var specification = new UserRoleIncludeSpecification(user.Id);
        var roleNames = await _userRoleReadRepository.GetAllAsync(specification, true, cancellationToken);

        return roleNames.Select(ur => ur.Role.Name).ToList();
    }

    public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(user, nameof(user));

        if (string.IsNullOrWhiteSpace(roleName))
        {
            throw new ArgumentNullException(nameof(roleName));
        }

        var role = await _roleStore.FindByNameAsync(roleName, cancellationToken);

        if (role is null)
        {
            return false;
        }

        var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);

        return userRole is null ? false : true;
    }
    
    public async Task<UserRole> FindUserRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken)
    {
        var userRole = await _userRoleReadRepository
            .GetAsync(ur => ur.RoleId == roleId && ur.UserId == userId, true, cancellationToken);

        return userRole;
    }

    public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        if (string.IsNullOrWhiteSpace(roleName))
        {
            throw new ArgumentNullException(nameof(roleName));
        }

        var role = await _roleStore.FindByNameAsync(roleName, cancellationToken);

        if (role is null)
        {
            return new List<ApplicationUser>();
        }

        var specification = new UserRoleGetRolesByRoleIdSpecification(role.Id);
        var userRoles = await _userRoleReadRepository.GetAllAsync(specification, true, cancellationToken);

        return userRoles.Select(ur => ur.User).ToList();
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