using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.AccountManagement;

/// <summary>
/// Configures the <see cref="Permission"/> entity for Entity Framework Core.
/// Sets up table mapping, primary key, property constraints, and relationships.
/// </summary>
internal class PermissionEntityConfiguration : IEntityTypeConfiguration<Permission>
{
    /// <summary>
    /// Configures the entity type for <see cref="Permission"/>.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        // Map to the "Permission" table
        builder.ToTable("Permission");

        // Set the primary key
        builder.HasKey(x => x.Id);

        // Configure the Description property as required with a max length of 200
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(200);

        // Configure the one-to-many relationship with RolePermission
        builder.HasMany(x => x.RolePermissions)
            .WithOne(x => x.Permission)
            .HasForeignKey(x => x.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
