using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using Xunit.Sdk;
namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Tests.Unit.Entities.AccountManagement;

/// <summary>
/// Unit tests for the <see cref="UserRole"/> class.
/// </summary>
public class UserRoleTests
{
    /// <summary>
    /// Tests the constructor of the <see cref="UserRole"/> class with valid arguments.
    /// </summary>
    [Fact] 
    public void Constructor_WithValidArguments_IsCreatedCorrectly()
    {
        // Arrange
        int userId = 1;
        int roleId = 2;
        // Act
        var userRole = new UserRole(userId, roleId);
        // Assert
        userRole.UserId.Should().Be(userId);
        userRole.RoleId.Should().Be(roleId);
    }
}