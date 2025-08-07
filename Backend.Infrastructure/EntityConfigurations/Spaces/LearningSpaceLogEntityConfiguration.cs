using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.Spaces;

/// <summary>
/// Configures the entity type for <see cref="LearningSpaceLog"/>.
/// </summary>
internal class LearningSpaceLogEntityConfiguration : IEntityTypeConfiguration<LearningSpaceLog>
{
    public void Configure(EntityTypeBuilder<LearningSpaceLog> builder)
    {
        builder.ToTable("LearningSpaceLog", "Infrastructure", tb => tb.UseSqlOutputClause(false));

        builder.HasKey(log => log.LearningSpaceLogInternalId);

        builder.Property(log => log.LearningSpaceLogInternalId)
            .ValueGeneratedOnAdd();

        builder.Property(log => log.Name)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(log => log.Type)
            .IsRequired()
            .HasMaxLength(50)
            .IsUnicode(false);

        builder.Property(log => log.MaxCapacity)
            .IsRequired();

        builder.Property(log => log.Width)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(log => log.Height)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(log => log.Length)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(log => log.ColorFloor)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(log => log.ColorWalls)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(log => log.ColorCeiling)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(log => log.ModifiedAt)
            .IsRequired()
            .HasColumnType("datetime");

        builder.Property(log => log.Action)
            .IsRequired()
            .HasMaxLength(20);
    }
}
