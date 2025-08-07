using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.UniversityManagement;

/// <summary>
/// Configures the EF Core mapping for the <see cref="University"/> entity.
/// This includes table name, primary key configuration, and value object conversion for the name.
/// </summary>
internal class UniversityEntityConfiguration : IEntityTypeConfiguration<University>
{
    /// <summary>
    /// Configures the database schema for the <see cref="University"/> entity.
    /// </summary>
    /// <param name="builder">The builder used to configure the <see cref="university"/> entity.</param>
    public void Configure(EntityTypeBuilder<University> builder)
    {
        // Map to the "university" table in the database
        builder.ToTable("University", "Locations");

        // Set the primary key to be the Name property (EntityName value object)
        builder.HasKey(university => university.Name);

        // Configure the Name property with a maximum length and value object conversion
        builder.Property(university => university.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(
                convertToProviderExpression: nameValueObject => nameValueObject!.Name,
                convertFromProviderExpression: nameString => EntityName.Create(nameString));

        builder.Property(university => university.Country)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(
                convertToProviderExpression: locationValueObject => locationValueObject!.Location,
                convertFromProviderExpression: locationString => EntityLocation.Create(locationString));
    }
}
