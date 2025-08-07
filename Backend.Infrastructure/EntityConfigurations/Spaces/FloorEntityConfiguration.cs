using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.Spaces;

/// <summary>
/// Configuration class for the <see cref="Floor"/> entity.
/// Maps the entity properties to the corresponding table and columns in the database.
/// </summary>
internal class FloorEntityConfiguration : IEntityTypeConfiguration<Floor>
{
    /// <summary>
    /// Configures the entity framework mapping for the <see cref="Floor"/> entity.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Floor> builder)
    {
        builder.ToTable("Floor", "Infrastructure");

        builder.Property<int>("FloorId")
          .ValueGeneratedOnAdd();

        builder.HasKey("FloorId");

        // Shadow property for BuildingInternalId (foreign key)
        builder.Property<int>("BuildingId")
          .IsRequired();

        // Unique index (shadow + VO)
        builder.HasIndex("BuildingId", "Number")
          .IsUnique();

        // ValueObject mapping: FloorNumber → int
        builder.Property(f => f.Number)
          .HasConversion(
            convertToProviderExpression: floorNumberVO => floorNumberVO.Value, // From FloorNumber to int
            convertFromProviderExpression: floorNumberInt => FloorNumber.Create(floorNumberInt)  // From int to FloorNumber
          )
          .IsRequired();

        // Foreign key relationship via shadow property
        builder.HasOne<Building>()
          .WithMany()
          .HasForeignKey("BuildingId")
          .OnDelete(DeleteBehavior.Cascade);
    }
}
