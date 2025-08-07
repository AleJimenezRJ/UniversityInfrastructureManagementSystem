using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.UniversityManagement;

/// <summary>
/// Configures the EF Core entity mapping for the <see cref="Area"/> entity.
/// This includes table mapping, primary key setup, and value object conversions.
/// </summary>
internal class AreaEntityConfiguration : IEntityTypeConfiguration<Area>
{
    /// <summary>
    /// Configures the entity type mapping for the <see cref="Area"/> class.
    /// </summary>
    /// <param name="builder">The entity type builder used for configuration.</param>
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        // Map to the "Area" table
        builder.ToTable("Area", "Locations");

        // Use the Name (EntityName value object) as the primary key
        builder.HasKey(area => area.Name);

        // Configure the Name property with max length and value object conversion
        builder.Property(area => area.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(
                convertToProviderExpression: nameValueObject => nameValueObject!.Name,
                convertFromProviderExpression: nameString => EntityName.Create(nameString));

        // Configure the foreign key relationship with Campus via CampusName
        builder.HasOne(area => area.Campus)
            .WithMany()
            .HasForeignKey(area => area.CampusName)
            .IsRequired();

        // Configure CampusName property with max length and value object conversion
        builder.Property(area => area.CampusName)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(
                convertToProviderExpression: nameValueObject => nameValueObject.Name,
                convertFromProviderExpression: nameString => EntityName.Create(nameString));
    }
}
