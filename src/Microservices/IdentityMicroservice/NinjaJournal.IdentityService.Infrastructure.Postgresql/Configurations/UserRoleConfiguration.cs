using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NinjaJournal.IdentityService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace NinjaJournal.IdentityService.Infrastructure.Postgresql.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(e => new { e.UserId, e.RoleId });
        
        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // Настрой связь в соответствии с твоими требованиями

        builder.HasOne(d => d.Role)
            .WithMany()
            .HasForeignKey(d => d.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}