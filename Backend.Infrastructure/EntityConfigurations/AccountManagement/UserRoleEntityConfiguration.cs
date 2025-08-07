using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.AccountManagement;

/// <summary>
/// Configures the UserRole entity for Entity Framework Core.
/// Sets up the composite primary key and relationships with User and Role entities.
/// </summary>
internal class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
{
    /// <summary>
    /// Configures the UserRole entity.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        // Map to the "UserRole" table
        builder.ToTable("UserRole");

        // Set composite primary key
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });

        // Configure relationship with User entity
        builder
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        // Configure relationship with Role entity
        builder
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
