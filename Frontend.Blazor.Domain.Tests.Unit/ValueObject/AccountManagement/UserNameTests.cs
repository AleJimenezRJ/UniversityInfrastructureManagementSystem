using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Tests.Unit.ValueObject.AccountManagement;

/// <summary>
/// Unit tests for the <see cref="UserName"/> value object.
/// </summary>
public class UserNameTests
{
    /// <summary>
    /// Tests that TryCreate returns false when the input is null or whitespace.
    /// </summary>
    /// <param name="input"> The input string to test.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    public void TryCreate_WithNullOrWhitespace_ReturnsFalse(string? input)
    {
        var result = UserName.TryCreate(input, out var userName);

        result.Should().BeFalse();
        userName.Should().BeNull();
    }

    /// <summary>
    /// Tests that TryCreate returns false when the input does not match the expected format.
    /// </summary>
    /// <param name="input"> The input string to test.</param>
    [Theory]
    [InlineData("UserName")]
    [InlineData("user name")]
    [InlineData("user$name")]
    [InlineData("user-name")]
    [InlineData("123456789012345678901234567890123456789012345678901")]
    public void TryCreate_WithInvalidFormat_ReturnsFalse(string input)
    {
        var result = UserName.TryCreate(input, out var userName);

        result.Should().BeFalse();
        userName.Should().BeNull();
    }

    /// <summary>
    /// Tests that TryCreate returns true when the input matches the expected format.
    /// </summary>
    /// <param name="input"> The input string to test.</param>
    [Theory]
    [InlineData("username")]
    [InlineData("user.name")]
    [InlineData("user_name")]
    [InlineData("abc123")]
    [InlineData("a")]
    [InlineData("user1234567890123456789012345678901234567890")]
    public void TryCreate_WithValidFormat_ReturnsTrue(string input)
    {
        var result = UserName.TryCreate(input, out var userName);

        result.Should().BeTrue();
        userName.Should().NotBeNull();
        userName!.Value.Should().Be(input);
    }

    /// <summary>
    /// Tests that Create throws a ValidationException when the input is null, empty, or does not match the expected format.
    /// </summary>
    /// <param name="input"> The input string to test.</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("User Name")]
    public void Create_WithInvalidInput_ThrowsValidationException(string? input)
    {
        Action act = () => UserName.Create(input);

        act.Should()
           .ThrowExactly<ValidationException>()
           .WithMessage("*Invalid UserName format*");
    }

    /// <summary>
    /// Tests that Create returns a UserName instance when the input is valid.
    /// </summary>
    /// <param name="input"> The input string to test.</param>
    [Theory]
    [InlineData("valid.username")]
    [InlineData("valid_user")]
    public void Create_WithValidInput_ReturnsUserNameInstance(string input)
    {
        var result = UserName.Create(input);

        result.Should().NotBeNull();
        result.Value.Should().Be(input);
    }

    /// <summary>
    /// Tests that two UserName instances with the same value are considered equal.
    /// </summary>
    [Fact]
    public void UserNames_WithSameValue_AreEqual()
    {
        var input = "user123";
        var u1 = UserName.Create(input);
        var u2 = UserName.Create(input);

        u1.Should().Be(u2);
    }

    /// <summary>
    /// Tests that two UserName instances with different values are not considered equal.
    /// </summary>
    [Fact]
    public void UserNames_WithDifferentValues_AreNotEqual()
    {
        var u1 = UserName.Create("user1");
        var u2 = UserName.Create("user2");

        u1.Should().NotBe(u2);
    }

}
