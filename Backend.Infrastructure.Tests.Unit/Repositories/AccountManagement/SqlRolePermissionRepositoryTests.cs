using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.AccountManagement;

/// <summary>
/// Provides unit tests for the <see cref="SqlRolePermissionRepository"/> class,  ensuring its methods behave as
/// expected under various conditions.
/// </summary>
public class SqlRolePermissionRepositoryTests
{
    private readonly Mock<ThemeParkDataBaseContext> _dbContextMock;
    private readonly Mock<ILogger<SqlRolePermissionRepository>> _loggerMock;
    private readonly SqlRolePermissionRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlRolePermissionRepositoryTests"/> class.
    /// </summary>
    public SqlRolePermissionRepositoryTests()
    {
        _dbContextMock = new Mock<ThemeParkDataBaseContext>();
        _loggerMock = new Mock<ILogger<SqlRolePermissionRepository>>();
        _repository = new SqlRolePermissionRepository(_dbContextMock.Object, _loggerMock.Object);
    }

    /// <summary>
    /// Tests the <c>ViewPermissionsByRoleIdAsync</c> method to verify that it returns a list of permissions associated
    /// with a specified role ID.
    /// </summary>
    [Fact]
    public async Task ViewPermissionsByRoleIdAsync_WhenRoleHasPermissions_ReturnsPermissionList()
    {
        // Arrange
        int roleId = 1;
        var permissions = new List<Permission>
        {
            new Permission("ReadUsers") { RolePermissions = new List<RolePermission> { new(roleId, 101) } },
            new Permission("WriteUsers") { RolePermissions = new List<RolePermission> { new(roleId, 102) } },
        };

        var mockPermissionsDbSet = permissions.BuildMock().BuildMockDbSet();
        _dbContextMock.Setup(db => db.Permissions).Returns(mockPermissionsDbSet.Object);

        // Act
        var result = await _repository.ViewPermissionsByRoleIdAsync(roleId);

        // Assert
        result.Should().BeEquivalentTo(permissions,
            "because the service should return permissions linked to the specified role ID");
    }

    /// <summary>
    /// Tests the <see cref="IPermissionRepository.ViewPermissionsByRoleIdAsync(int)"/> method to ensure it returns an
    /// empty list when no permissions are assigned to the specified role ID.
    /// </summary>
    [Fact]
    public async Task ViewPermissionsByRoleIdAsync_WhenRoleHasNoPermissions_ReturnsEmptyList()
    {
        // Arrange
        int roleId = 999;
        var permissions = new List<Permission>
        {
            new Permission("Unrelated") { RolePermissions = new List<RolePermission> { new(2, 100) } }
        };

        var mockPermissionsDbSet = permissions.BuildMock().BuildMockDbSet();
        _dbContextMock.Setup(db => db.Permissions).Returns(mockPermissionsDbSet.Object);

        // Act
        var result = await _repository.ViewPermissionsByRoleIdAsync(roleId);

        // Assert
        result.Should().BeEmpty("because no permissions are assigned to the given role ID");
    }

    /// <summary>
    /// Tests the behavior of <c>ViewPermissionsByRoleIdAsync</c> when an exception is thrown during database access.
    /// Verifies that the method returns a <see cref="DomainException"/> and logs the error.
    /// </summary>
    [Fact]
    public async Task ViewPermissionsByRoleIdAsync_WhenExceptionThrown_ThrowsDomainExceptionAndLogsError()
    {
        // Arrange
        int roleId = 1;
        _dbContextMock
            .Setup(db => db.Permissions)
            .Throws(new Exception("Database error"));

        // Act
        var act = async () => await _repository.ViewPermissionsByRoleIdAsync(roleId);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("An error occurred while retrieving permissions for the role.");

        _loggerMock.Verify(
            log => log.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error retrieving permissions")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
            Times.Once);
    }

    /// <summary>
    /// Tests the <see cref="SqlRolePermissionRepository.AssignPermissionToRoleAsync"/> method to verify that it throws a
    /// DuplicatedEntityException when trying to assign a permission to a role that already has that permission.
    /// </summary>
    [Fact]
    public async Task AssignPermissionToRoleAsync_WhenRolePermissionAlreadyExists_ThrowsDuplicatedEntityException()
    {
        // Arrange
        int roleId = 1;
        int permId = 100;
        var existing = new List<RolePermission> { new(roleId, permId) }.AsQueryable().BuildMockDbSet();

        _dbContextMock.Setup(db => db.RolePermissions).Returns(existing.Object);

        // Act
        var act = async () => await _repository.AssignPermissionToRoleAsync(roleId, permId);

        // Assert
        await act.Should().ThrowAsync<DuplicatedEntityException>()
            .WithMessage($"Permission {permId} is already assigned to role {roleId}.");
    }


    /// <summary>
    /// Tests the <see cref="SqlRolePermissionRepository.AssignPermissionToRoleAsync"/> method to verify that it returns
    /// a NotFoundException when trying to assign a permission to a role that does not exist.
    [Fact]
    public async Task AssignPermissionToRoleAsync_WhenRoleDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        int roleId = 1;
        int permId = 100;

        _dbContextMock.Setup(db => db.RolePermissions).Returns(new List<RolePermission>().AsQueryable().BuildMockDbSet().Object);
        _dbContextMock.Setup(db => db.Roles).Returns(new List<Role>().AsQueryable().BuildMockDbSet().Object);

        // Act
        var act = async () => await _repository.AssignPermissionToRoleAsync(roleId, permId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Role with ID {roleId} not found.");
    }


    /// <summary>
    /// Tests the <c>AssignPermissionToRoleAsync</c> method to verify that it returns a NotFoundException when
    /// attempting to assign a permission to a role and the specified permission does not exist.
    /// </summary>
    [Fact]
    public async Task AssignPermissionToRoleAsync_WhenPermissionDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        int roleId = 1;
        int permId = 100;

        var role = new Role("Admin") { Id = roleId };
        var roles = new List<Role> { role }.AsQueryable().BuildMockDbSet();
        var permissions = new List<Permission>().AsQueryable().BuildMockDbSet(); // vacío
        var rolePerms = new List<RolePermission>().AsQueryable().BuildMockDbSet();

        _dbContextMock.Setup(db => db.RolePermissions).Returns(rolePerms.Object);
        _dbContextMock.Setup(db => db.Roles).Returns(roles.Object);
        _dbContextMock.Setup(db => db.Permissions).Returns(permissions.Object);

        // Act & Assert
        await FluentActions.Awaiting(() =>
            _repository.AssignPermissionToRoleAsync(roleId, permId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Permission with ID {permId} not found.");
    }


    /// <summary>
    /// Tests that the <c>AssignPermissionToRoleAsync</c> method throws a <see cref="DomainException"/>  when saving
    /// changes to the database fails.
    /// </summary>s
    [Fact]
    public async Task AssignPermissionToRoleAsync_WhenSaveFails_ThrowsDomainException()
    {
        // Arrange
        int roleId = 1;
        int permId = 100;
        var role = new Role("Admin") { Id = roleId };
        var perm = new Permission("ManageUsers") { Id = permId };

        _dbContextMock.Setup(db => db.RolePermissions)
            .Returns(new List<RolePermission>().AsQueryable().BuildMockDbSet().Object);
        _dbContextMock.Setup(db => db.Roles)
            .Returns(new List<Role> { role }.AsQueryable().BuildMockDbSet().Object);
        _dbContextMock.Setup(db => db.Permissions)
            .Returns(new List<Permission> { perm }.AsQueryable().BuildMockDbSet().Object);

        _dbContextMock.Setup(db => db.SaveChangesAsync(default)).ReturnsAsync(0);

        // Act
        var act = async () => await _repository.AssignPermissionToRoleAsync(roleId, permId);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("Failed to assign permission to role.");
    }

    /// <summary>
    /// Tests the <c>AssignPermissionToRoleAsync</c> method to ensure that it successfully adds a permission to a role
    /// and returns <see langword="true"/> when valid inputs are provided.
    /// </summary>
    [Fact]
    public async Task AssignPermissionToRoleAsync_WhenValid_AddsPermissionAndReturnsTrue()
    {
        // Arrange
        int roleId = 1;
        int permId = 100;

        var role = new Role("Admin") { Id = roleId };
        var permission = new Permission("ManageUsers") { Id = permId };

        var rolePerms = new List<RolePermission>().AsQueryable().BuildMockDbSet();
        var roles = new List<Role> { role }.AsQueryable().BuildMockDbSet();
        var permissions = new List<Permission> { permission }.AsQueryable().BuildMockDbSet();

        _dbContextMock.Setup(db => db.RolePermissions).Returns(rolePerms.Object);
        _dbContextMock.Setup(db => db.Roles).Returns(roles.Object);
        _dbContextMock.Setup(db => db.Permissions).Returns(permissions.Object);
        _dbContextMock.Setup(db => db.RolePermissions.Add(It.IsAny<RolePermission>()));
        _dbContextMock.Setup(db => db.SaveChangesAsync(default)).ReturnsAsync(1);

        var repo = new SqlRolePermissionRepository(_dbContextMock.Object, Mock.Of<ILogger<SqlRolePermissionRepository>>());

        // Act
        var result = await repo.AssignPermissionToRoleAsync(roleId, permId);

        // Assert
        result.Should().BeTrue("because the role and permission exist and the association was saved");
    }

    /// <summary>
    /// Tests the <c>RemovePermissionFromRoleAsync</c> method to verify that it returns NotFoundException when
    /// the specified role-permission association does not exist.
    /// </summary>
    [Fact]
    public async Task RemovePermissionFromRoleAsync_WhenAssociationDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        int roleId = 1;
        int permId = 100;

        var rolePermissions = new List<RolePermission>().AsQueryable().BuildMockDbSet();
        _dbContextMock.Setup(db => db.RolePermissions).Returns(rolePermissions.Object);

        var roles = new List<Role> { new Role("Admin") { Id = roleId } }.AsQueryable().BuildMockDbSet();
        var permissions = new List<Permission> { new Permission("DeleteUsers") { Id = permId } }.AsQueryable().BuildMockDbSet();
        _dbContextMock.Setup(db => db.Roles).Returns(roles.Object);
        _dbContextMock.Setup(db => db.Permissions).Returns(permissions.Object);

        // Act & Assert
        await FluentActions.Awaiting(() =>
            _repository.RemovePermissionFromRoleAsync(roleId, permId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Role with ID {roleId} not found.");
    }


    /// <summary>
    /// Tests the <c>RemovePermissionFromRoleAsync</c> method to verify that it returns NotFoundException when
    /// attempting to remove a permission from a role that does not exist.
    [Fact]
    public async Task RemovePermissionFromRoleAsync_WhenRoleDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        int roleId = 1;
        int permId = 100;

        var rolePermissions = new List<RolePermission> { new(roleId, permId) }.AsQueryable().BuildMockDbSet();
        _dbContextMock.Setup(db => db.RolePermissions).Returns(rolePermissions.Object);

        var roles = new List<Role>().AsQueryable().BuildMockDbSet(); // No roles
        var permissions = new List<Permission> { new Permission("DeleteUsers") { Id = permId } }.AsQueryable().BuildMockDbSet();
        _dbContextMock.Setup(db => db.Roles).Returns(roles.Object);
        _dbContextMock.Setup(db => db.Permissions).Returns(permissions.Object);

        // Act & Assert
        await FluentActions.Awaiting(() => _repository.RemovePermissionFromRoleAsync(roleId, permId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Permission with ID {permId} not found.");
    }


    /// <summary>
    /// Tests the <c>RemovePermissionFromRoleAsync</c> method to verify that it returns NotFoundException when
    /// attempting to remove a permission from a role and the specified permission does not exist.
    [Fact]
    public async Task RemovePermissionFromRoleAsync_WhenPermissionDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        int roleId = 1;
        int permId = 100;

        var rolePermissions = new List<RolePermission> { new(roleId, permId) }.AsQueryable().BuildMockDbSet();
        _dbContextMock.Setup(db => db.RolePermissions).Returns(rolePermissions.Object);

        var roles = new List<Role> { new Role("Admin") { Id = roleId } }.AsQueryable().BuildMockDbSet();
        var permissions = new List<Permission>().AsQueryable().BuildMockDbSet(); // No permission
        _dbContextMock.Setup(db => db.Roles).Returns(roles.Object);
        _dbContextMock.Setup(db => db.Permissions).Returns(permissions.Object);

        // Act & Assert
        await FluentActions.Awaiting(() => _repository.RemovePermissionFromRoleAsync(roleId, permId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage($"The permission {permId} is not assigned to role {roleId}.");
    }


    /// <summary>
    /// Tests the <c>RemovePermissionFromRoleAsync</c> method to verify that it removes an existing  permission from a
    /// role and returns <see langword="true"/> when the association exists.
    /// </summary>
    [Fact]
    public async Task RemovePermissionFromRoleAsync_WhenEntryExists_RemovesAndReturnsTrue()
    {
        // Arrange
        int roleId = 1;
        int permId = 100;
        var existing = new RolePermission(roleId, permId);

        var rolePermissions = new List<RolePermission> { existing }.AsQueryable().BuildMockDbSet();
        _dbContextMock.Setup(db => db.RolePermissions).Returns(rolePermissions.Object);
        _dbContextMock.Setup(db => db.RolePermissions.Remove(It.IsAny<RolePermission>()));

        var roles = new List<Role> { new Role("Admin") { Id = roleId } }.AsQueryable().BuildMockDbSet();
        var permissions = new List<Permission> { new Permission("DeleteUsers") { Id = permId } }.AsQueryable().BuildMockDbSet();
        _dbContextMock.Setup(db => db.Roles).Returns(roles.Object);
        _dbContextMock.Setup(db => db.Permissions).Returns(permissions.Object);
        _dbContextMock.Setup(db => db.SaveChangesAsync(default)).ReturnsAsync(1);

        // Act
        var result = await _repository.RemovePermissionFromRoleAsync(roleId, permId);

        // Assert
        result.Should().BeTrue("because the association existed and was successfully removed");
    }

    /// <summary>
    /// Tests the <c>RemovePermissionFromRoleAsync</c> method to verify that it returns <see langword="false"/>  when no
    /// matching <c>RolePermission</c> entry exists after performing necessary checks.
    /// </summary>
    [Fact]
    public async Task RemovePermissionFromRoleAsync_WhenAssociationEntryDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        int roleId = 1;
        int permId = 100;

        var rolePermissions = new List<RolePermission>().AsQueryable().BuildMockDbSet(); // no match
        _dbContextMock.Setup(db => db.RolePermissions).Returns(rolePermissions.Object);

        var roles = new List<Role> { new Role("Admin") { Id = roleId } }.AsQueryable().BuildMockDbSet();
        var permissions = new List<Permission> { new Permission("DeleteUsers") { Id = permId } }.AsQueryable().BuildMockDbSet();
        _dbContextMock.Setup(db => db.Roles).Returns(roles.Object);
        _dbContextMock.Setup(db => db.Permissions).Returns(permissions.Object);

        // Act & Assert
        await FluentActions.Awaiting(() => _repository.RemovePermissionFromRoleAsync(roleId, permId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Role with ID {roleId} not found.");
    }
}

