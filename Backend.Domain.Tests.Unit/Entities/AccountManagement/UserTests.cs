using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.AccountManagement;


/// The PBI that this test class is related to is: #118
/// Create New Users

/// Technical tasks to complete for the UserRole entity:
/// - Implement backend logic to store user data
/// - Add validations for required fields
/// - Write unit and integration tests

/// Participants: Elizabeth Huang & Esteban Baires


/// <summary>
/// Unit tests for the <see cref="User"/> class.
/// </summary>
public class UserTests
{
    /// <summary>
    /// A valid UserName used for testing purposes.
    /// </summary>
    private readonly UserName _validUserName;


    /// <summary>
    /// UserTests constructor initializes a valid UserName for testing, before each test.
    /// </summary>
    public UserTests()
    {
        _validUserName = UserName.Create("testuser");
    }

    /// <summary>
    /// Tests that the User constructor creates a User with a valid UserName.
    /// </summary>
    [Fact]
    public void Constructor_WithValidUserName_CreatesUser()
    {
        // Arrange
        var userName = _validUserName;
        // Act
        var user = new User(userName, 1);
        // Assert
        user.UserName.Should().Be(userName);
        user.Id.Should().Be(0); // Default ID for new users
    }

    /// <summary>
    /// Tests that the UserName constructor throws a ValidationException when given an invalid UserName.
    /// </summary>
    [Fact]
    public void Constructor_WithInvalidUserName_ShouldThrowValidationException()
    {
        // Arrange
        string invalidUserName = "Testear!";
        // Act
        Action act = () => UserName.Create(invalidUserName);
        // Assert
        act.Should().Throw<ValidationException>()
           .WithMessage("*Invalid UserName format*");
    }
}