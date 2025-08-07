using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using Xunit.Sdk;
namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.AccountManagement;

/// The PBI that this test class is related to is: #121
/// Assign roles to user

/// Technical tasks to complete for the UserRole entity:
/// - Implement role assignment logic in backend
/// - Validate role existence before assignments
/// - Ensure updated permissions are reflected immediately
/// - Write tests for valid/invalid/updated assignments

/// Participants: Elizabeth Huang & Esteban Baires

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