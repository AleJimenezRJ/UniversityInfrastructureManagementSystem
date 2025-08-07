using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.AccountManagement;

/// <summary>
/// Configures the <see cref="RolePermission"/> entity for Entity Framework Core.
/// This configuration sets up the table mapping, composite primary key, and relationships
/// between <see cref="RolePermission"/>, <see cref="Role"/>, and <see cref="Permission"/> entities.
/// </summary>
internal class RolePermissionEntityConfiguration : IEntityTypeConfiguration<RolePermission>
{
    /// <summary>
    /// Configures the entity of type <see cref="RolePermission"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        // Map to the "RolePermission" table
        builder
            .ToTable("RolePermission");

        // Set composite primary key
        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        // Configure relationship to Role
        builder.HasOne(x => x.Role)
            .WithMany(x => x.RolePermissions)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure relationship to Permission
        builder.HasOne(x => x.Permission)
            .WithMany(x => x.RolePermissions)
            .HasForeignKey(x => x.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
