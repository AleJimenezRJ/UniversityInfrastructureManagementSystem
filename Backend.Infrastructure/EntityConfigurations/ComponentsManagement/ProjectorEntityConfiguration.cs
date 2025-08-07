using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.ComponentsManagement;

/// <summary>
/// Configures the EF Core entity mapping for the <see cref="Projector"/> entity.
/// </summary>
internal class ProjectorEntityConfiguration : IEntityTypeConfiguration<Projector>
{
    /// <summary>
    /// Configures the entity properties and relationships for the <see cref="Projector"/> entity.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<Projector> builder)
    {
        builder.ToTable("Projector", "Infrastructure", t => t.UseSqlOutputClause(false));

        builder.Property(p => p.ProjectedContent)
            .HasColumnName("ProjectedContent")
            .HasMaxLength(255);

        builder.OwnsOne(p => p.ProjectionArea, area =>
        {
            area.Property(a => a.Height)
                .HasColumnName("ProjectedHeight")
                .HasColumnType("decimal(5,2)");

            area.Property(a => a.Length)
                .HasColumnName("ProjectedWidth")
                .HasColumnType("decimal(5,2)");
        });

    }
}