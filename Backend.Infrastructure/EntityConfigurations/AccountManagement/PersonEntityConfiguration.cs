using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.AccountManagement;

/// <summary>
/// Entity configuration for the Person entity.
/// </summary>
internal class PersonEntityConfiguration : IEntityTypeConfiguration<Person>
{
    /// <summary>
    /// Configures the entity of type <see cref="Person"/> using the specified <see cref="EntityTypeBuilder{TEntity}"/>.
    /// Converts the properties of the Person entity to their respective database types.
    /// </summary>
    /// <param name="builder"></param>
    /// <exception cref="NotImplementedException"></exception>
    /// The conversion was taken from  https://aka.ms/efcore-docs-value-converters
    public void Configure(EntityTypeBuilder<Person> builder)
    {
       // Map to "Person" table
        builder.ToTable("Person");

        // Primary key
        builder.HasKey(person => person.Id);
        builder.Property(person => person.Id)
               .ValueGeneratedOnAdd();

		// Email (Value Object)
        builder.Property(person => person.Email)
               .IsRequired()
               .HasMaxLength(100)
               .HasConversion(
                  convertToProviderExpression: EmailValueObject => EmailValueObject.Value,
                  convertFromProviderExpression: EmailString => Email.Create(EmailString));

        builder.HasIndex(person => person.Email)
               .IsUnique();

		// IdentityNumber (Value Object)
        builder.Property(person => person.IdentityNumber)
               .IsRequired()
               .HasMaxLength(20)
               .HasConversion(
                 convertToProviderExpression: IdentityNumberValueObject => IdentityNumberValueObject.Value,
                 convertFromProviderExpression: IdentityNumberString => IdentityNumber.Create(IdentityNumberString)
               );

        builder.HasIndex(person => person.IdentityNumber)
               .IsUnique();

		// Name fields
        builder.Property(person => person.FirstName)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(person => person.LastName)
               .IsRequired()
               .HasMaxLength(50);

		// BirthDate (Value Object)
        builder.Property(person => person.BirthDate)
               .IsRequired()
               .HasColumnType("date")
               .HasConversion(
                 convertToProviderExpression: BirthDateValueObject => BirthDateValueObject.Value.ToDateTime(TimeOnly.MinValue),
                 convertFromProviderExpression: birthDate => BirthDate.Create(DateOnly.FromDateTime(birthDate))
               );

		// Phone (Value Object, Optional)
        builder.Property(person => person.Phone)
               .HasMaxLength(20)
               .IsRequired(false)
               .HasConversion(
                convertToProviderExpression: personPhoneValueObject => personPhoneValueObject.Value,
                convertFromProviderExpression: personPhoneString => Phone.Create(personPhoneString)
                );
    }
}
