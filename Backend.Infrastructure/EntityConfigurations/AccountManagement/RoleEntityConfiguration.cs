using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.AccountManagement;

/// <summary>
/// Entity configuration for the Role entity.
/// </summary>
internal class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    /// <summary>
    /// Configures the Role entity.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Map to the "Role" table
        builder.ToTable("Role");

        // Set primary key
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
               .IsRequired()
               .ValueGeneratedOnAdd();
        
        // Name field with unique constraint
        builder.HasIndex(r => r.Name)
              .IsUnique();

        builder.Property(r => r.Name)
            .HasMaxLength(50);
      
    }
}
