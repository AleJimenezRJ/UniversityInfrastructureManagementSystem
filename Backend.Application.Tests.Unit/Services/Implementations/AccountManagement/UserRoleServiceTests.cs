using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Services.Implementations.AccountManagement;

/// The PBI that this test class is related to is: #121

/// Technical tasks to complete for the UserRole entity:
/// - Implement role assignment logic in backend 
/// - Validate role existence before assignments
/// - Ensure updated permissions are reflected immediately
/// - Write tests for valid/invalid/updated assignments

/// Participants: Elizabeth Huang & Esteban Baires

/// <summary>
/// Unit tests for the <see cref="UserRoleService"/> class.
/// </summary>
public class UserRoleServiceTests
{
    private readonly Mock<IUserRoleRepository> _userRoleRepositoryMock;
    private readonly UserRoleService _serviceUnderTest;
    private readonly UserRole _userRoleWithUserAndRole;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRoleServiceTests"/> class.
    /// </summary>
    public UserRoleServiceTests()
    {
        _userRoleRepositoryMock = new Mock<IUserRoleRepository>();
        _serviceUnderTest = new UserRoleService(_userRoleRepositoryMock.Object);
        // userId = 1, roleId = 2
        _userRoleWithUserAndRole = new UserRole(1, 2);
    }

    /// <summary>
    /// Tests the AssignRoleAsync method when the user does not have the specified role.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact] 
    public async Task AssignRoleAsync_WhenUserDoesNotHaveRole_ShouldAddRoleAndReturnTrue()
    {
        // Arrange
        _userRoleRepositoryMock // Task<bool> AssignRoleAsync(int userId, int roleId);
            .Setup(repo => repo.AssignRoleAsync(1, 2))
            .ReturnsAsync(true);

        // Act
        var result = await _serviceUnderTest.AssignRoleAsync(_userRoleWithUserAndRole.UserId, _userRoleWithUserAndRole.RoleId);
        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests the AssignRoleAsync method when the user already has the specified role.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task AssignRoleAsync_WhenUserAlreadyHasRole_ShouldNotAddRoleAndReturnFalse()
    {
        // Arrange
        _userRoleRepositoryMock
            .Setup(repo => repo.GetByUserAndRoleAsync(1, 2))
            .ReturnsAsync(_userRoleWithUserAndRole);
        // Act
        var result = await _serviceUnderTest.AssignRoleAsync(_userRoleWithUserAndRole.UserId, _userRoleWithUserAndRole.RoleId);
        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests the RemoveRoleAsync method when the user has the specified role.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task RemoveRoleAsync_WhenUserHasRole_ShouldRemoveRoleAndReturnTrue()
    {
        // Arrange
        _userRoleRepositoryMock
            .Setup(repo => repo.GetByUserAndRoleAsync(1, 2))
            .ReturnsAsync(_userRoleWithUserAndRole);

        _userRoleRepositoryMock
            .Setup(repo => repo.RemoveAsync(_userRoleWithUserAndRole))
            .ReturnsAsync(true);

        // Act
        var result = await _serviceUnderTest.RemoveRoleAsync(_userRoleWithUserAndRole.UserId, _userRoleWithUserAndRole.RoleId);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests the RemoveRoleAsync method when the user does not have the specified role.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task RemoveRoleAsync_WhenUserDoesNotHaveRole_ShouldNotRemoveRoleAndReturnFalse()
    {
        // Arrange
        _userRoleRepositoryMock
            .Setup(repo => repo.GetByUserAndRoleAsync(1, 2))
            .ReturnsAsync((UserRole?)null);

        // Act
        var result = await _serviceUnderTest.RemoveRoleAsync(_userRoleWithUserAndRole.UserId, _userRoleWithUserAndRole.RoleId);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests the GetRolesByUserIdAsync method when the user has roles assigned.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task GetRolesByUserIdAsync_WhenUserHasRoles_ShouldReturnUserRoles()
    {
        // Arrange
        var roles = new List<Role> { new Role("Test") };

        _userRoleRepositoryMock
            .Setup(repo => repo.GetRolesByUserIdAsync(1))
            .ReturnsAsync(roles);

        // Act
        var result = await _serviceUnderTest.GetRolesByUserIdAsync(_userRoleWithUserAndRole.UserId);

        // Assert
        result.Should().BeEquivalentTo(roles);
    }

    /// <summary>
    /// Tests the GetRolesByUserIdAsync method when the user has no roles assigned.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task GetRolesByUserIdAsync_WhenUserHasNoRoles_ShouldReturnEmptyList()
    {
        // Arrange
        _userRoleRepositoryMock
            .Setup(repo => repo.GetRolesByUserIdAsync(1))
            .ReturnsAsync(new List<Role>());

        // Act
        var result = await _serviceUnderTest.GetRolesByUserIdAsync(_userRoleWithUserAndRole.UserId);

        // Assert
        result.Should().BeEmpty();
    }
}