using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.AccountManagement;

/// <summary>
/// Entity configuration for the UserAudit entity.
/// </summary>
internal class UserAuditEntityConfiguration : IEntityTypeConfiguration<UserAudit>
{
    /// <summary>
    /// Configures the entity type for <see cref="UserAudit"/>.
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(EntityTypeBuilder<UserAudit> builder)
    {
        // Map to "UserAudit" table
        builder.ToTable("UserAudit");

        // Primary key
        builder.HasKey(a => a.AuditId);

        builder.Property(a => a.AuditId)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.UserName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Phone)
            .HasMaxLength(20);

        builder.Property(a => a.IdentityNumber)
            .HasMaxLength(20);

        builder.Property(a => a.BirthDate)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(a => a.ModifiedAt)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(a => a.Action)
            .IsRequired()
            .HasMaxLength(20);
    }
}
