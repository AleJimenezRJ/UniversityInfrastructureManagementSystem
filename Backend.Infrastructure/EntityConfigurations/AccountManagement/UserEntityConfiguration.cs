using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.EntityConfigurations.AccountManagement;

/// <summary>
/// Entity configuration for the User entity.
/// </summary>
internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    /// Configures the entity mapping for <see cref="User"/> using Fluent API.
    /// </summary>
    /// <param name="builder">The builder used to construct the entity model.</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Map to "UserAccount" table
        builder.ToTable("UserAccount");

        // Primary key
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id).ValueGeneratedOnAdd();

        // UserName (Value Object)
        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion(
                  convertToProviderExpression: UserNameValueObject => UserNameValueObject.Value,
                  convertFromProviderExpression: UserNameString => UserName.Create(UserNameString));

        builder.HasIndex(user => user.UserName)
            .IsUnique();

        // Person relationship
        builder.Property(User => User.PersonId)
            .IsRequired();

        builder.HasOne(user => user.Person)
            .WithMany()
            .HasForeignKey(user => user.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
