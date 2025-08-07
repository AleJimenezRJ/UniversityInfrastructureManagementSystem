using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.UniversityManagement;

/// <summary>
/// Configures the EF Core entity mapping for the <see cref="Campus"/> entity.
/// Includes table mapping, primary key definition, and value object conversions for properties.
/// </summary>
internal class CampusEntityConfiguration : IEntityTypeConfiguration<Campus>
{
    /// <summary>
    /// Configures the schema details for the <see cref="Campus"/> entity.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Campus> builder)
    {
        // Map to the "Campus" table
        builder.ToTable("Campus", "Locations");

        // Use the Name (EntityName value object) as the primary key
        builder.HasKey(campus => campus.Name);

        // Configure the Name property with max length and value object conversion
        builder.Property(campus => campus.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(
                convertToProviderExpression: nameValueObject => nameValueObject!.Name,
                convertFromProviderExpression: nameString => EntityName.Create(nameString));

        // Configure the Location property with max length and value object conversion
        builder.Property(campus => campus.Location)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(
                convertToProviderExpression: locationValueObject => locationValueObject!.Location,
                convertFromProviderExpression: locateString => EntityLocation.Create(locateString));

        // Configures the relationship between campus and university (foreign key)
        builder.HasOne(campus => campus.University)
            .WithMany()
            .HasForeignKey(campus => campus.UniversityName)
            .IsRequired();

        // Configures the university property with a max length and conversion from value object
        builder.Property(campus => campus.UniversityName)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(
                convertToProviderExpression: nameValueObject => nameValueObject.Name,
                convertFromProviderExpression: nameString => EntityName.Create(nameString));
    }
}
