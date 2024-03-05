using NinjaJournal.Microservice.Infrastructure.Abstractions.Repositories;
using NinjaJournal.IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace NinjaJournal.IdentityService.Domain.Identity;

public class RoleStore : IRoleStore<ApplicationRole>
{
    private readonly IReadEntityRepository<Guid, ApplicationRole> _roleReadRepository;
    private readonly IEntityRepository<Guid, ApplicationRole> _roleRepository;
    private bool disposed;

    public RoleStore(IReadEntityRepository<Guid, ApplicationRole> roleReadRepository, IEntityRepository<Guid, ApplicationRole> roleRepository)
    {
        ArgumentNullException.ThrowIfNull(roleReadRepository, nameof(roleReadRepository));
        ArgumentNullException.ThrowIfNull(roleRepository, nameof(roleRepository));
        
        _roleReadRepository = roleReadRepository;
        _roleRepository = roleRepository;
    }
    
    public async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ArgumentNullException.ThrowIfNull(role, nameof(role));

        await _roleRepository.AddAsync(role, true, cancellationToken);

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ArgumentNullException.ThrowIfNull(role, nameof(role));

        role.ConcurrencyStamp = Guid.NewGuid().ToString();
        
        await _roleRepository.UpdateAsync(role, true, cancellationToken);

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        ArgumentNullException.ThrowIfNull(role, nameof(role));

        await _roleRepository.DeleteAsync(role, true, cancellationToken);
        
        return IdentityResult.Success;
    }

    public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(role, nameof(role));

        return Task.FromResult(role.Id.ToString());
    }

    public Task<string?> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(role, nameof(role));

        return Task.FromResult(role.Name);
    }

    public Task SetRoleNameAsync(ApplicationRole role, string? roleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(role, nameof(role));
        ArgumentNullException.ThrowIfNull(roleName, nameof(roleName));
        
        role.Name = roleName;
        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(role, nameof(role));

        return Task.FromResult<string?>(role.NormalizedName);
    }

    public Task SetNormalizedRoleNameAsync(ApplicationRole role, string? normalizedName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(role, nameof(role));
        ArgumentNullException.ThrowIfNull(normalizedName, nameof(normalizedName));
        
        role.NormalizedName = normalizedName;
        return Task.CompletedTask;
    }

    public async Task<ApplicationRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(roleId, nameof(roleId));
        
        return await _roleReadRepository.GetByIdAsync(Guid.Parse(roleId), true, cancellationToken);
    }

    public async Task<ApplicationRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(normalizedRoleName, nameof(normalizedRoleName));

        return await _roleReadRepository.GetAsync(u => u.NormalizedName == normalizedRoleName
            , true, cancellationToken);
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

    ~RoleStore()
    {
        Dispose(disposing: false);
    }
}