using NinjaJournal.Microservice.Infrastructure.EntityFrameworkCore;
using NinjaJournal.IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace NinjaJournal.IdentityService.Infrastructure.Postgresql;

public class IdentityServiceDbContext : BaseDbContext<IdentityServiceDbContext>
{
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ApplicationRole> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    
    public IdentityServiceDbContext(DbContextOptions<IdentityServiceDbContext> options) 
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityServiceDbContext).Assembly);
    }
}