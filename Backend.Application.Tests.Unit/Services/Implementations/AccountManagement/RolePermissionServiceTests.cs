// Participantes: Isabella Rodriguez Sanchez y Gael Alpizar Alfaro
// User Story: SPT-PA-004-001 View Permissions Assigned to Role #124
// Tarea técnica: Write unit and integration tests

using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Services.Implementations.AccountManagement;

/// <summary>
/// Unit tests for the <see cref="RolePermissionService"/> class,
/// </summary>
public class RolePermissionServiceTests
{
    /// <summary>
    /// Mock repository for role-permission operations.
    /// </summary>
    private readonly Mock<IRolePermissionRepository> _rolePermissionRepositoryMock;

    /// <summary>
    /// Service under test, which uses the mocked repository to manage role-permission associations.
    /// </summary>
    private readonly RolePermissionService _serviceUnderTest;

    /// <summary>
    /// A valid role ID and permission ID used for testing various methods in the service.
    /// </summary>
    private readonly int _roleId;

    /// <summary>
    /// A valid permission ID used for testing various methods in the service.
    /// </summary>
    private readonly int _permId;

    /// <summary>
    /// A list of sample permissions used for testing the retrieval of permissions by role ID.
    /// </summary>
    private readonly List<Permission> _samplePermissions;

    /// <summary>
    /// A non-existent role ID used to test the behavior of the service when a role does not exist.
    /// </summary>
    private readonly int _nonExistentRoleId;


    /// <summary>
    /// Initializes a new instance of the <see cref="RolePermissionServiceTests"/> class.
    /// </summary>
    public RolePermissionServiceTests()
    {
        _rolePermissionRepositoryMock = new Mock<IRolePermissionRepository>(MockBehavior.Strict);
        _serviceUnderTest = new (_rolePermissionRepositoryMock.Object);
        _roleId = 1;
        _permId = 100;
        _nonExistentRoleId = 999;
        _samplePermissions = new List<Permission>
        {
            new Permission("ReadUsers"),
            new Permission("WriteUsers"),
            new Permission("DeleteUsers")
        };
    }

    /// <summary>
    /// Tests the <see cref="RolePermissionService.ViewPermissionsByRoleIdAsync"/> method to ensure it
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task ViewPermissionsByRoleIdAsync_WhenRoleExists_ReturnsPermissions()
    {
        // Arrange

        _rolePermissionRepositoryMock
            .Setup(repo => repo.ViewPermissionsByRoleIdAsync(_roleId))
            .ReturnsAsync(_samplePermissions);

        // Act
        var result = await _serviceUnderTest.ViewPermissionsByRoleIdAsync(_roleId);

        // Assert
        result.Should().BeEquivalentTo(_samplePermissions,
            "because the service should return the list of permissions for a valid role ID");
    }

    /// <summary>
    /// Tests the <see cref="RolePermissionService.ViewPermissionsByRoleIdAsync"/> method when the role does not exist.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task ViewPermissionsByRoleIdAsync_WhenRoleDoesNotExist_ReturnsNull()
    {
        // Arrange
        _rolePermissionRepositoryMock
            .Setup(repo => repo.ViewPermissionsByRoleIdAsync(_nonExistentRoleId))
            .ReturnsAsync((List<Permission>?)null);

        // Act
        var result = await _serviceUnderTest.ViewPermissionsByRoleIdAsync(_nonExistentRoleId);

        // Assert
        result.Should().BeNull("because the role ID does not exist or has no permissions");
    }

    /// <summary>
    /// Tests the <see cref="RolePermissionService.AssignPermissionToRoleAsync"/> method to ensure it
    /// </summary>
    /// <param name="expectedResult"> The expected result returned by the repository (true or false).</param>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task AssignPermissionToRoleAsync_Always_ReturnsResultFromRepository(bool expectedResult)
    {
        // Arrange
        _rolePermissionRepositoryMock
            .Setup(repo => repo.AssignPermissionToRoleAsync(_roleId, _permId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _serviceUnderTest.AssignPermissionToRoleAsync(_roleId, _permId);

        // Assert
        result.Should().Be(expectedResult, "because the result should match the repository's result");
    }

    /// <summary>
    /// Tests the <see cref="RolePermissionService.RemovePermissionFromRoleAsync"/> method to ensure it
    /// </summary>
    /// <param name="expectedResult"> The expected result returned by the repository (true or false).</param>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task RemovePermissionFromRoleAsync_Always_ReturnsResultFromRepository(bool expectedResult)
    {
        // Arrange
        _rolePermissionRepositoryMock
            .Setup(repo => repo.RemovePermissionFromRoleAsync(_roleId, _permId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _serviceUnderTest.RemovePermissionFromRoleAsync(_roleId, _permId);

        // Assert
        result.Should().Be(expectedResult, "because the result should match the repository's result");
    }
}
