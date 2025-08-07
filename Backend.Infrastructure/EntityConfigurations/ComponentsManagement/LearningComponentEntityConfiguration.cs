using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.ComponentsManagement;

/// <summary>
/// Configures the EF Core entity mapping for the <see cref="LearningComponent"/> entity.
/// </summary>
internal class LearningComponentEntityConfiguration : IEntityTypeConfiguration<LearningComponent>
{
    public void Configure(EntityTypeBuilder<LearningComponent> builder)
    {
        builder.ToTable("LearningComponent", "Infrastructure", tb => tb.UseSqlOutputClause(false));

        builder.Property<int>("ComponentId")
               .ValueGeneratedOnAdd();
        // Primary key
        builder.HasKey(lc => lc.ComponentId);

        // LearningSpaceId (FK)
        builder.Property<int>("LearningSpaceId")
             .IsRequired();

        // Dimensions
        builder.OwnsOne(lc => lc.Dimensions, dim =>
        {
            dim.Property(d => d.Width).HasColumnName("Width").HasColumnType("decimal(5,2)");
            dim.Property(d => d.Height).HasColumnName("Height").HasColumnType("decimal(5,2)");
            dim.Property(d => d.Length).HasColumnName("Depth").HasColumnType("decimal(5,2)");
        });

        // Position
        builder.OwnsOne(lc => lc.Position, pos =>
        {
            pos.Property(p => p.X).HasColumnName("X").HasColumnType("decimal(9,6)");
            pos.Property(p => p.Y).HasColumnName("Y").HasColumnType("decimal(9,6)");
            pos.Property(p => p.Z).HasColumnName("Z").HasColumnType("decimal(9,6)");
        });

        // Orientation
        builder.Property(lc => lc.Orientation)
          .HasMaxLength(20)
          .HasConversion(
            convertToProviderExpression: o => o.Value,
            convertFromProviderExpression: s => Orientation.Create(s));
        
        builder.Property(lc => lc.IsDeleted)
           .HasColumnName("IsDeleted")
           .IsRequired();

        // Foreign key relationship with LearningSpace
        builder.HasOne<LearningSpace>()
             .WithMany()
             .HasForeignKey("LearningSpaceId")
             .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.DisplayId)
            .HasColumnName("DisplayId")
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

    }
}
