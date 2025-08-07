using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.ComponentsManagement;

/// <summary>
/// Configures the EF Core entity mapping for the <see cref="Whiteboard"/> entity.
/// </summary>
internal class WhiteboardEntityConfiguration : IEntityTypeConfiguration<Whiteboard>
{
    /// <summary>
    /// Configures the entity properties and relationships for the <see cref="Whiteboard"/> entity.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Whiteboard> builder)
    {
        builder.ToTable("Whiteboard", "Infrastructure", t => t.UseSqlOutputClause(false));

        builder.Property(wb => wb.MarkerColor)
            .HasMaxLength(20)
            .IsRequired()
            .HasConversion(
            convertToProviderExpression: markerColorValueObject => markerColorValueObject!.Value,
            convertFromProviderExpression: markerColorString => Colors.Create(markerColorString));

    }
}

