using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using System.Drawing;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.Spaces;

/// <summary>
/// Configuration class for the <see cref="LearningSpace"/> entity.
/// Maps the entity properties to the corresponding table and columns in the database.
/// </summary>
internal class LearningSpaceEntityConfiguration : IEntityTypeConfiguration<LearningSpace>
{
    /// <summary>
    /// Configures the entity framework mapping for the <see cref="LearningSpace"/> entity.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<LearningSpace> builder)
    {
        builder.ToTable("LearningSpace", "Infrastructure", tb => tb.UseSqlOutputClause(false));

        builder.Property<int>("LearningSpaceId")
               .ValueGeneratedOnAdd();

        builder.HasKey("LearningSpaceId");

        // Shadow property: FloorId (FK to Floor table)
        builder.Property<int>("FloorId")
               .IsRequired();

        // Unique constraint: FloorId + Name
        builder.HasIndex("FloorId", "Name")
               .IsUnique();

        // Property: Name
        builder.Property(ls => ls.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion(
                convertToProviderExpression: nameVO => nameVO.Name,
                convertFromProviderExpression: nameStr => EntityName.Create(nameStr)
            );

        // Property: Type
        builder.Property(ls => ls.Type)
            .HasMaxLength(50)
            .HasConversion(
                convertToProviderExpression: typeVO => typeVO.Value,
                convertFromProviderExpression: typeStr => LearningSpaceType.Create(typeStr)
            );

        // Property: MaxCapacity
        builder.Property(ls => ls.MaxCapacity)
            .IsRequired()
            .HasConversion(
                convertToProviderExpression: capacityVO => capacityVO.Value,
                convertFromProviderExpression: capacityInt => Capacity.Create(capacityInt)
            );

        // Property: Height
        builder.Property(ls => ls.Height)
            .IsRequired()
            .HasPrecision(5, 2)
            .HasConversion(
                convertToProviderExpression: heightVO => (decimal)heightVO.Value,
                convertFromProviderExpression: heightDecimal => Domain.ValueObjects.Spaces.Size.Create((double)heightDecimal)
            );

        // Property: Width
        builder.Property(ls => ls.Width)
            .IsRequired()
            .HasPrecision(5, 2)
            .HasConversion(
                convertToProviderExpression: widthVO => (decimal)widthVO.Value,
                convertFromProviderExpression: widthDecimal => Domain.ValueObjects.Spaces.Size.Create((double)widthDecimal)
            );

        // Property: Length
        builder.Property(ls => ls.Length)
            .IsRequired()
            .HasPrecision(5, 2)
            .HasConversion(
                convertToProviderExpression: lengthVO => (decimal)lengthVO.Value,
                convertFromProviderExpression: lengthDecimal => Domain.ValueObjects.Spaces.Size.Create((double)lengthDecimal)
            );

        // Property: ColorFloor
        builder.Property(ls => ls.ColorFloor)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion(
                convertToProviderExpression: colorVO => colorVO.Value,
                convertFromProviderExpression: colorStr => Colors.Create(colorStr)
            );

        // Property: ColorWalls
        builder.Property(ls => ls.ColorWalls)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion(
                convertToProviderExpression: colorVO => colorVO.Value,
                convertFromProviderExpression: colorStr => Colors.Create(colorStr)
            );


        // Property: ColorCeiling
        builder.Property(ls => ls.ColorCeiling)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion(
                convertToProviderExpression: colorVO => colorVO.Value,
                convertFromProviderExpression: colorStr => Colors.Create(colorStr)
            );

        // Foreign key to Floor (only FloorId)
        builder.HasOne<Floor>()
            .WithMany()
            .HasForeignKey("FloorId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
