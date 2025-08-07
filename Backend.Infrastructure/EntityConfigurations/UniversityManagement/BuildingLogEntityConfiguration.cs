using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.UniversityManagement;

/// <summary>
/// Configures the entity type for <see cref="BuildingLog"/>.
/// </summary>
/// <remarks>This configuration maps the <see cref="BuildingLog"/> entity to the "BuildingLog" table in the
/// database. It defines the primary key, required properties, column constraints, and precision for numeric
/// values.</remarks>
internal class BuildingLogEntityConfiguration : IEntityTypeConfiguration<BuildingLog>
{
    /// <summary>
    /// Configures the entity type <see cref="BuildingLog"/> and its mapping to the database schema.
    /// </summary>
    /// <remarks>This method maps the <see cref="BuildingLog"/> entity to the "BuildingLog" table and
    /// configures its properties with specific constraints, such as required fields, maximum lengths, precision, and
    /// data types. It also defines the primary key for the entity.</remarks>
    /// <param name="builder">An <see cref="EntityTypeBuilder{TEntity}"/> instance used to configure the properties, keys, and relationships
    /// of the <see cref="BuildingLog"/> entity.</param>
    public void Configure(EntityTypeBuilder<BuildingLog> builder)
    {
        // Map to "BuildingLog" table
        builder.ToTable("BuildingsLog");

        builder.HasKey(bl => bl.BuildingsLogInternalId);

        builder.Property(bl => bl.BuildingsLogInternalId)
            .ValueGeneratedOnAdd();

        builder.Property(bl => bl.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(bl => bl.X)
            .IsRequired()
            .HasPrecision(9, 6);

        builder.Property(bl => bl.Y)
            .IsRequired()
            .HasPrecision(9, 6);


        builder.Property(bl => bl.Z)
            .IsRequired()
            .HasPrecision(9, 6);

        builder.Property(bl => bl.Width)
            .IsRequired()
            .HasPrecision(6, 2);

        builder.Property(bl => bl.Length)
            .IsRequired()
            .HasPrecision(6, 2);

        builder.Property(bl => bl.Height)
            .IsRequired()
            .HasPrecision(6, 2);

        builder.Property(building => building.Color)
           .IsRequired()
           .HasMaxLength(50);


        builder.Property(b => b.AreaName)
           .IsRequired()
           .HasMaxLength(100);

        builder.Property(bl => bl.ModifiedAt)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(bl => bl.Action)
            .IsRequired()
            .HasMaxLength(20);
    }
}
