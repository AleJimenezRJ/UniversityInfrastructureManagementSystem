using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.ComponentsManagement;

/// <summary>
/// Entity configuration for the LearningComponentAudit entity.
/// </summary>
internal class LearningComponentAuditEntityConfiguration : IEntityTypeConfiguration<LearningComponentAudit>
{
    /// <summary>
    /// Configures the entity type for <see cref="LearningComponentAudit"/>.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<LearningComponentAudit> builder)
    {
        builder.ToTable("LearningComponentAudit", "Infrastructure", tb => tb.UseSqlOutputClause(false));

        builder.HasKey(a => a.LearningComponentAuditId);

        builder.Property(a => a.LearningComponentAuditId)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.ComponentId)
            .IsRequired();

        builder.Property(a => a.Width)
            .IsRequired()
            .HasPrecision(5,2);

        builder.Property(a => a.Height)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(a => a.Depth)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(a => a.X)
            .IsRequired()
            .HasPrecision(9, 6);

        builder.Property(a => a.Y)
            .IsRequired()
            .HasPrecision(9, 6);

        builder.Property(a => a.Z)
            .IsRequired()
            .HasPrecision(9, 6);

        builder.Property(a => a.Orientation)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.IsDeleted)
            .IsRequired();

        builder.Property(a => a.ComponentType)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(a => a.MarkerColor)
            .HasMaxLength(20);

        builder.Property(a => a.ProjectedContent)
            .HasMaxLength(255);

        builder.Property(a => a.ProjectedHeight)
            .HasPrecision(5,2);

        builder.Property(a => a.ProjectedWidth)
            .HasPrecision(5,2);

        builder.Property(a => a.Action)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(a => a.ModifiedAt)
            .HasColumnType("datetime")
            .IsRequired();

    }
}