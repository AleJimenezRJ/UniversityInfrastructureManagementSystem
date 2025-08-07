using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.AccountManagement;
/// <summary>
/// Entity configuration for the Staff entity.
/// </summary>
internal class StaffEntityConfiguration : IEntityTypeConfiguration<Staff>
{
    /// <summary>
    /// Configures the entity of type <see cref="Staff"/>.
    /// </summary>
    /// <param name="builder"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        // Mapea a la tabla "Staff"
        builder.ToTable("Staff");

        // Define clave primaria
        builder.HasKey(s => s.PersonId);

        builder.Property(s => s.PersonId)
               .HasColumnName("PersonId");

        // Relationship with Person - composition with delete cascade
        builder.HasOne(s => s.Person)
               .WithMany()
               .HasForeignKey(s => s.PersonId)
               .OnDelete(DeleteBehavior.Cascade);

        // StaffType (restricción tipo CHECK: 'ADM', 'DOC', 'ADMDOC')
        builder.Property(s => s.Type)
               .IsRequired()
               .HasMaxLength(10)
               .HasColumnName("StaffType");

        // references: https://learn.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.relationalentitytypebuilderextensions.hascheckconstraint?view=efcore-9.0
        builder.HasCheckConstraint("CK_Staff_StaffType",
            "StaffType IN ('ADM', 'DOC', 'ADMDOC')");

        // InstitutionalEmail
        builder.Property(s => s.InstitutionalEmail)
               .IsRequired()
               .HasMaxLength(100)
               .HasConversion(
                    email => email.Value,
                    str => Email.Create(str)
               );

        builder.HasIndex(s => s.InstitutionalEmail)
               .IsUnique();
    }
}