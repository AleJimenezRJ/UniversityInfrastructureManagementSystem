using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.AccountManagement;

/// <summary>
/// Entity configuration for the Student entity.
/// </summary>
internal class StudentEntityConfiguration : IEntityTypeConfiguration<Student>
{
    /// <summary>
    /// Configures the entity mapping for <see cref="Student"/> using Fluent API.
    /// </summary>
    /// <param name="builder">The builder used to construct the entity model.</param>
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        // Maps to the "Student" table
        builder.ToTable("Student");

        // Defines the primary key (Foreign key to Person)
        builder.HasKey(s => s.PersonId);

        builder.Property(s => s.PersonId)
               .HasColumnName("PersonId");

        // Configure StudentId
        builder.Property(s => s.StudentId)
               .IsRequired()
               .HasMaxLength(30);

        builder.HasIndex(s => s.StudentId)
               .IsUnique();

        // Configure InstitutionalEmail conversion
        builder.Property(s => s.InstitutionalEmail)
               .IsRequired()
               .HasMaxLength(100)
               .HasConversion(
                   email => email.Value,
                   str => Email.Create(str)
               );

        builder.HasIndex(s => s.InstitutionalEmail)
               .IsUnique();

        // Defines the relation with Person
        builder.HasOne(s => s.Person)
               .WithMany()
               .HasForeignKey(s => s.PersonId)
               .OnDelete(DeleteBehavior.Cascade);
    }


}
