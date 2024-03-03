using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NinjaJournal.IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace NinjaJournal.IdentityService.Infrastructure.Postgresql.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });
        builder.HasOne<ApplicationUser>().WithMany().HasForeignKey(ur => ur.UserId).IsRequired();
        builder.HasOne<ApplicationRole>().WithMany().HasForeignKey(ur => ur.RoleId).IsRequired();
    }
}