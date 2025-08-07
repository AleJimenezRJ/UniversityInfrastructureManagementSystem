using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using Microsoft.Extensions.Logging;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.AccountManagement;

/// <summary>
/// Unit tests for <see cref="SqlUserRoleRepository"/> which manages user-role associations.
/// </summary>
public class SqlUserRoleRepositoryTests
{
    /// <summary>
    /// Tests that <see cref="SqlUserRoleRepository.AssignRoleAsync"/> throws DuplicatedEntityException when the user already has the role assigned.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task AssignRoleAsync_ShouldThrowDuplicatedEntityException_WhenUserAlreadyHasRole()
    {
        // Arrange
        var userRoles = new List<UserRole> { new UserRole(1, 1) };
        var mockUserRolesDbSet = userRoles.AsQueryable().BuildMockDbSet();

        var mockDbContext = new Mock<ThemeParkDataBaseContext>();
        mockDbContext.Setup(c => c.UserRoles).Returns(mockUserRolesDbSet.Object);

        var repository = new SqlUserRoleRepository(mockDbContext.Object, Mock.Of<ILogger<SqlUserRoleRepository>>());

        // Act
        Func<Task> act = async () => await repository.AssignRoleAsync(1, 1);

        // Assert
        await act.Should().ThrowAsync<DuplicatedEntityException>()
            .WithMessage("Attempt to create user role association that already exists");
    }


    /// <summary>
    /// Tests that <see cref="SqlUserRoleRepository.AssignRoleAsync"/> returns true when a new role is assigned to a user successfully.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task AssignRoleAsync_ShouldReturnTrue_WhenRoleAssignedSuccessfully()
    {
        // Arrange
        List<UserRole> userRoles = [];
        Mock<DbSet<UserRole>> mockUserRolesDbSet = userRoles.AsQueryable().BuildMockDbSet();
        Mock<ThemeParkDataBaseContext> mockDbContext = new();
        mockDbContext
            .Setup(c => c.UserRoles)
            .Returns(mockUserRolesDbSet.Object);
        mockDbContext
            .Setup(c => c.SaveChangesAsync(default))
            .ReturnsAsync(1);
        
        SqlUserRoleRepository repository = new(mockDbContext.Object, Mock.Of<ILogger<SqlUserRoleRepository>>());

        // Act
        var result = await repository.AssignRoleAsync(1, 2);

        // Assert
        result.Should().BeTrue(because: "The role was assigned successfully");
    }

    /// <summary>
    /// Tests that <see cref="SqlUserRoleRepository.GetByUserAndRoleAsync"/> returns null when the user-role association does not exist.
    /// </summary>
    /// <returns>
    /// Results a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task GetByUserAndRoleAsync_ShouldReturnUserRole_WhenUserRoleExists()
    {
        // Arrange
        List<UserRole> userRoles = [];
        Mock<DbSet<UserRole>> mockUserRolesDbSet = userRoles.AsQueryable().BuildMockDbSet();
        Mock<ThemeParkDataBaseContext> mockDbContext = new();
        mockDbContext
            .Setup(c => c.UserRoles)
            .Returns(mockUserRolesDbSet.Object);
        SqlUserRoleRepository repository = new(mockDbContext.Object, Mock.Of<ILogger<SqlUserRoleRepository>>());

        // Add an existing user-role association to the mock database
        // This simulates a user with ID 1 having a role with ID 2
        var existingUserRole = new UserRole(1, 2);
        userRoles.Add(existingUserRole);

        // Act
        var result = await repository.GetByUserAndRoleAsync(1, 2);
        
        // Assert
        result.Should().NotBeNull(because: "The user-role association exists");
        result!.UserId.Should().Be(1);
        result.RoleId.Should().Be(2);
    }

    /// <summary>
    /// Tests that <see cref="SqlUserRoleRepository.GetByUserAndRoleAsync"/> returns null when the user-role association does not exist.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task GetByUserAndRoleAsync_ShouldReturnNull_WhenUserRoleDoesNotExist()
    {
        // Arrange
        List<UserRole> userRoles = [];
        Mock<DbSet<UserRole>> mockUserRolesDbSet = userRoles.AsQueryable().BuildMockDbSet();
        Mock<ThemeParkDataBaseContext> mockDbContext = new();
        mockDbContext
            .Setup(c => c.UserRoles)
            .Returns(mockUserRolesDbSet.Object);
        SqlUserRoleRepository repository = new(mockDbContext.Object, Mock.Of<ILogger<SqlUserRoleRepository>>());

        // Act
        var result = await repository.GetByUserAndRoleAsync(1, 2);

        // Assert
        result.Should().BeNull(because: "The user-role association does not exist");
    }

    /// <summary>
    /// Tests that <see cref="SqlUserRoleRepository.AddAsync"/> throws DuplicatedEntityException when the user-role association already exists.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task AddAsync_ShouldThrowDuplicatedEntityException_WhenUserRoleAlreadyExists()
    {
        // Arrange
        var userRoles = new List<UserRole> { new UserRole(1, 2) };
        var mockUserRolesDbSet = userRoles.AsQueryable().BuildMockDbSet();

        var mockDbContext = new Mock<ThemeParkDataBaseContext>();
        mockDbContext.Setup(c => c.UserRoles).Returns(mockUserRolesDbSet.Object);

        var repository = new SqlUserRoleRepository(mockDbContext.Object, Mock.Of<ILogger<SqlUserRoleRepository>>());

        // Act
        Func<Task> act = async () => await repository.AddAsync(new UserRole(1, 2));

        // Assert
        await act.Should().ThrowAsync<DuplicatedEntityException>()
            .WithMessage("Attempt to create user role association that already exists");
    }


    /// <summary>
    /// Tests that <see cref="SqlUserRoleRepository.AddAsync"/> returns true when a new user-role association is added successfully.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task AddAsync_ShouldReturnTrue_WhenUserRoleAddedSuccessfully()
    {
        // Arrange
        List<UserRole> userRoles = [];
        Mock<DbSet<UserRole>> mockUserRolesDbSet = userRoles.AsQueryable().BuildMockDbSet();
        Mock<ThemeParkDataBaseContext> mockDbContext = new();
        mockDbContext
            .Setup(c => c.UserRoles)
            .Returns(mockUserRolesDbSet.Object);
        mockDbContext
            .Setup(c => c.SaveChangesAsync(default))
            .ReturnsAsync(1);
        
        SqlUserRoleRepository repository = new(mockDbContext.Object, Mock.Of<ILogger<SqlUserRoleRepository>>());
        
        // Act
        var result = await repository.AddAsync(new UserRole(1, 2));
        
        // Assert
        result.Should().BeTrue(because: "The user-role association was added successfully");
    }

    /// <summary>
    /// Tests that <see cref="SqlUserRoleRepository.RemoveAsync"/> throws NotFoundException when the user-role association does not exist.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task RemoveAsync_ShouldThrowNotFoundException_WhenUserRoleDoesNotExist()
    {
        // Arrange
        List<UserRole> userRoles = []; // empty
        var mockUserRolesDbSet = userRoles.AsQueryable().BuildMockDbSet();

        var mockDbContext = new Mock<ThemeParkDataBaseContext>();
        mockDbContext.Setup(c => c.UserRoles).Returns(mockUserRolesDbSet.Object);

        var repository = new SqlUserRoleRepository(mockDbContext.Object, Mock.Of<ILogger<SqlUserRoleRepository>>());

        var userRoleToRemove = new UserRole(1, 2);

        // Act + Assert
        Func<Task> act = async () => await repository.RemoveAsync(userRoleToRemove);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("User Role with roleId 2 and userId 1 and was not found.");
    }

    /// <summary>
    /// Tests that <see cref="SqlUserRoleRepository.RemoveAsync"/> returns true when a user-role association is removed successfully.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task RemoveAsync_ShouldReturnTrue_WhenUserRoleRemovedSuccessfully()
    {
        // Arrange
        List<UserRole> userRoles = [];
        Mock<DbSet<UserRole>> mockUserRolesDbSet = userRoles.AsQueryable().BuildMockDbSet();
        Mock<ThemeParkDataBaseContext> mockDbContext = new();
        mockDbContext
            .Setup(c => c.UserRoles)
            .Returns(mockUserRolesDbSet.Object);
        mockDbContext
            .Setup(c => c.SaveChangesAsync(default))
            .ReturnsAsync(1);
        
        SqlUserRoleRepository repository = new(mockDbContext.Object, Mock.Of<ILogger<SqlUserRoleRepository>>());
        
        // Add an existing user-role association to the mock database
        var existingUserRole = new UserRole(1, 2);
        userRoles.Add(existingUserRole);
        
        // Act
        var result = await repository.RemoveAsync(existingUserRole);
        
        // Assert
        result.Should().BeTrue(because: "The user-role association was removed successfully");
    }

    /// <summary>
    /// Tests that <see cref="SqlUserRoleRepository.GetRolesByUserIdAsync"/> returns an empty list when no roles are assigned to the user.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task GetRolesByUserIdAsync_ShouldReturnListOfRoles_WhenUserHasRolesAssigned()
    {
        // Arrange
        var existingRole = new Role("Admin") { Id = 2 };
        var existingUserRole = new UserRole(1, 2)
        {
            Role = existingRole
        };

        List<UserRole> userRoles = [existingUserRole];
        Mock<DbSet<UserRole>> mockUserRolesDbSet = userRoles.AsQueryable().BuildMockDbSet();

        Mock<ThemeParkDataBaseContext> mockDbContext = new();
        mockDbContext
            .Setup(c => c.UserRoles)
            .Returns(mockUserRolesDbSet.Object);

        var repository = new SqlUserRoleRepository(mockDbContext.Object, Mock.Of<ILogger<SqlUserRoleRepository>>());

        // Act
        var result = await repository.GetRolesByUserIdAsync(1);

        // Assert
        result.Should()
            .NotBeNull(because: "a user-role relationship exists")
            .And.HaveCount(1, because: "one role was linked to user ID 1")
            .And.ContainSingle(r => r.Id == 2 && r.Name == "Admin", because: "the role was assigned to the user");
    }

    /// <summary>
    /// Tests that <see cref="SqlUserRoleRepository.GetRolesByUserIdAsync"/> returns an empty list when no roles are assigned to the user.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task GetRolesByUserIdAsync_ShouldReturnEmptyList_WhenNoRolesAssignedToUser()
    {
        // Arrange
        List<UserRole> userRoles = [];
        var mockUserRolesDbSet = userRoles.AsQueryable().BuildMockDbSet();
        var mockDbContext = new Mock<ThemeParkDataBaseContext>();
        mockDbContext
            .Setup(c => c.UserRoles)
            .Returns(mockUserRolesDbSet.Object);

        var repository = new SqlUserRoleRepository(mockDbContext.Object, Mock.Of<ILogger<SqlUserRoleRepository>>());

        // Act
        var result = await repository.GetRolesByUserIdAsync(1);

        // Assert
        result.Should()
            .NotBeNull(because: "it should return an empty list instead of null")
            .And.BeEmpty(because: "there are no roles assigned to the user");
    }

}