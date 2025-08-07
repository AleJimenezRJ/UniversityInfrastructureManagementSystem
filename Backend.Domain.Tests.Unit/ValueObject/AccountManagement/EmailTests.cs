using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.ValueObject.AccountManagement;


/// <summary>
/// Unit tests for the <see cref="Email"/> value object.
/// </summary>
public class EmailTests
{

    // TryCreate method tests

    /// <summary>
    /// Tests that TryCreate returns true when a valid email is provided.
    /// </summary>
    [Fact]
    public void TryCreate_WithValidEmail_ShouldReturnTrue()
    {
        // Arrange
        var input = "example@test.com";

        // Act
        var result = Email.TryCreate(input, out var email);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that TryCreate returns true for various valid email formats.
    /// </summary>
    /// <param name="validEmail"> A valid email string to test.</param>
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("name.surname@company.org")]
    [InlineData("a@b.cd")]
    public void TryCreate_WithValidEmails_ShouldReturnTrue(string validEmail)
    {
        // Act
        var result = Email.TryCreate(validEmail, out var email);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that TryCreate returns true for emails with special characters.
    /// </summary>
    [Fact]
    public void TryCreate_EmailWithSpecialCharacters_ShouldReturnTrue()
    {
        // Arrange
        var input = "first.last+custom-tag_123@sub.domain.com";

        // Act
        var result = Email.TryCreate(input, out var email);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that TryCreate returns false for various invalid email formats.
    /// </summary>
    /// <param name="invalidEmail"> An invalid email string to test.</param>
    [Theory]
    [InlineData("missingatsign.com")]
    [InlineData("no.domain@")]
    [InlineData("@nodomain.com")]
    [InlineData("user@.com")]
    [InlineData("user@@domain.com")]
    [InlineData("user@domain,com")]
    [InlineData("user@domain com")]
    [InlineData("user@domaincom")]
    public void TryCreate_InvalidEmails_ShouldReturnFalse(string invalidEmail)
    {
        // Act
        var result = Email.TryCreate(invalidEmail, out var email);

        // Assert
        result.Should().BeFalse(because: "validation should fail when given a invalid email");
    }

    /// <summary>
    /// Tests that TryCreate returns false when the email is null or whitespace.
    /// </summary>
    [Fact]
    public void TryCreate_WithEmptyEmail_ShouldReturnFalse()
    {
        // Arrange
        var input = "";

        // Act
        var result = Email.TryCreate(input, out var email);

        // Assert
        result.Should().BeFalse(because: "validation should fail when given a empty email");
    }

    /// <summary>
    /// Tests that TryCreate returns false when the email is null.
    /// </summary>
    /// <param name="validEmail"> A valid email string to test.</param>
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("name.surname@company.org")]
    [InlineData("valid.email+tag@sub.domain.net")]
    public void Create_WithValidEmail_ShouldReturnEmailObject(string validEmail)
    {
        // Act
        var email = Email.Create(validEmail);

        // Assert
        email.Should().NotBeNull();
        email.Value.Should().Be(validEmail);
    }

    /// <summary>
    /// Tests that Create throws a ValidationException when an invalid email is provided.
    /// </summary>
    /// <param name="invalidEmail"> An invalid email string to test.</param>
    [Theory]
    [InlineData("noatsign.com")]
    [InlineData("@no-user.com")]
    [InlineData("user@.nodomain")]
    [InlineData("user@@domain.com")]
    [InlineData("user@domain")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithInvalidEmail_ShouldThrowValidationException(string? invalidEmail)
    {
        // Act
        Action act = () => Email.Create(invalidEmail);

        // Assert
        act.Should()
           .Throw<ValidationException>()
           .WithMessage("Invalid email format. It must contain '@' and end with .domain");
    }

    /// <summary>
    /// Tests that Create throws a ValidationException when the email is null.
    /// </summary>
    [Fact]
    public void Create_EmailWithWhitespaceAround_ShouldThrow()
    {
        // Arrange
        var input = "  user@example.com ";

        // Act
        Action act = () => Email.Create(input);

        // Assert
        act.Should().Throw<ValidationException>();
    }

    /// <summary>
    /// Tests that two Email objects created with the same valid email are equal.
    /// </summary>
    [Fact]
    public void Create_ValidEmail_ShouldProduceEqualObjects()
    {
        // Arrange
        var email1 = Email.Create("same@domain.com");
        var email2 = Email.Create("same@domain.com");

        // Assert
        email1.Should().Be(email2);
    }

    /// <summary>
    /// Tests that two Email objects created with different emails are not equal.
    /// </summary>
    [Fact]
    public void Create_DifferentEmails_ShouldProduceNonEqualObjects()
    {
        // Arrange
        var email1 = Email.Create("user1@domain.com");
        var email2 = Email.Create("user2@domain.com");

        // Assert
        email1.Should().NotBe(email2);
    }

}