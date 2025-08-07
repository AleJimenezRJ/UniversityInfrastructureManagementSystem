// Participantes: Isabella Rodriguez Sanchez y Gael Alpizar Alfaro
// User Story: SPT-RM-003-002 Delete Role #122
// User Story: SPT-RM-003-003 List All Roles #164
// Tarea técnica: Write unit and integration tests

using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Services.Implementations.AccountManagement;

/// <summary>
/// Unit tests for the <see cref="RoleService"/> class,
/// </summary>
public class RoleServiceTests
{
    /// <summary>
    /// Mock repository for role operations.
    /// </summary>
    private readonly Mock<IRoleRepository> _roleRepositoryMock;

    /// <summary>
    /// Service under test, which uses the mocked repository to manage roles.
    /// </summary>
    private readonly RoleService _serviceUnderTest;

    /// <summary>
    /// A valid role ID used for testing various methods in the service.
    /// </summary>
    private readonly int _roleId;

    /// <summary>
    /// A list of sample roles used for testing the retrieval of roles.
    /// </summary>
    private readonly List<Role> _sampleRoles;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleServiceTests"/> class.
    /// </summary>
    public RoleServiceTests()
    {
        _roleRepositoryMock = new Mock<IRoleRepository>(MockBehavior.Strict);
        _serviceUnderTest = new(_roleRepositoryMock.Object);
        _roleId = 1;
        _sampleRoles = new List<Role>
        {
            new Role("Admin"),
            new Role("Editor"),
            new Role("Viewer")
        };
    }

    /// <summary>
    /// Tests the <see cref="RoleService.GetAllRolesAsync"/> method to ensure it
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetAllRolesAsync_WhenRolesExist_ReturnsRoleList()
    {
        // Arrange
        _roleRepositoryMock
            .Setup(repo => repo.GetAllRolesAsync())
            .ReturnsAsync(_sampleRoles);

        // Act
        var result = await _serviceUnderTest.GetAllRolesAsync();

        // Assert
        result.Should().BeEquivalentTo(_sampleRoles,
            "because the service should return the list of roles from the repository");
    }

    /// <summary>
    /// Tests the <see cref="RoleService.GetAllRolesAsync"/> method to ensure it returns an empty list
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetAllRolesAsync_WhenNoRolesExist_ReturnsEmptyList()
    {
        // Arrange
        _roleRepositoryMock
            .Setup(repo => repo.GetAllRolesAsync())
            .ReturnsAsync(new List<Role>());

        // Act
        var result = await _serviceUnderTest.GetAllRolesAsync();

        // Assert
        result.Should().BeEmpty("because no roles exist in the database");
    }

    /// <summary>
    /// Tests the <see cref="RoleService.DeleteRoleAsync"/> method to ensure it
    /// </summary>
    /// <param name="expectedResult"> The expected result returned by the repository (true or false).</param>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DeleteRoleAsync_Always_ReturnsRepositoryResult(bool expectedResult)
    {
        // Arrange
        _roleRepositoryMock
            .Setup(repo => repo.DeleteRoleAsync(_roleId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _serviceUnderTest.DeleteRoleAsync(_roleId);

        // Assert
        result.Should().Be(expectedResult,
            "because the service should return the same result as the repository");
    }
}