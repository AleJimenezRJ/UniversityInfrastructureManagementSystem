using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using MockQueryable.Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;
using System.Linq.Expressions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="SqlRoleRepository"/> class, verifying role deletion and retrieval logic.
/// </summary>
public class SqlRoleRepositoryTests
{
    /// <summary>
    /// Tests that <see cref="SqlRoleRepository.DeleteRoleAsync(int)"/> deletes an existing role and returns true.
    /// </summary>
    [Fact]
    public async Task DeleteRoleAsync_WhenRoleExists_DeletesAndReturnsTrue()
    {
        // Arrange
        var roleId = 1;
        var role = new Role("Admin") { Id = roleId };
        var roles = new List<Role> { role }.AsQueryable();

        var mockRoles = roles.BuildMockDbSet();

        var dbContextMock = new Mock<ThemeParkDataBaseContext>();
        dbContextMock.Setup(x => x.Roles).Returns(mockRoles.Object);
        dbContextMock.Setup(x => x.Roles.Remove(It.IsAny<Role>()));
        dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var loggerMock = new Mock<ILogger<SqlRoleRepository>>();
        var repo = new SqlRoleRepository(dbContextMock.Object, loggerMock.Object);

        // Act
        var result = await repo.DeleteRoleAsync(roleId);

        // Assert
        result.Should().BeTrue();
        dbContextMock.Verify(x => x.Roles.Remove(It.Is<Role>(r => r.Id == roleId)), Times.Once);
        dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="SqlRoleRepository.DeleteRoleAsync(int)"/> throws <see cref="NotFoundException"/> when the role does not exist.
    /// </summary>
    [Fact]
    public async Task DeleteRoleAsync_WhenRoleDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var roleId = 42;
        var roles = new List<Role>().AsQueryable();

        var mockRoles = roles.BuildMockDbSet();

        var dbContextMock = new Mock<ThemeParkDataBaseContext>();
        dbContextMock.Setup(x => x.Roles).Returns(mockRoles.Object);

        var loggerMock = new Mock<ILogger<SqlRoleRepository>>();
        var repo = new SqlRoleRepository(dbContextMock.Object, loggerMock.Object);

        // Act & Assert
        await FluentActions.Invoking(() => repo.DeleteRoleAsync(roleId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Role with ID {roleId} not found.");
    }

    /// <summary>
    /// Tests that <see cref="SqlRoleRepository.DeleteRoleAsync(int)"/> throws <see cref="ConcurrencyConflictException"/> when a <see cref="DbUpdateConcurrencyException"/> is thrown.
    /// </summary>
    [Fact]
    public async Task DeleteRoleAsync_WhenDbUpdateConcurrencyException_ThrowsConcurrencyConflictException()
    {
        // Arrange
        var roleId = 1;
        var role = new Role("Test") { Id = roleId };
        var roles = new List<Role> { role }.AsQueryable();

        var mockRoles = roles.BuildMockDbSet();

        var dbContextMock = new Mock<ThemeParkDataBaseContext>();
        dbContextMock.Setup(x => x.Roles).Returns(mockRoles.Object);
        dbContextMock.Setup(x => x.Roles.Remove(It.IsAny<Role>()));
        dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                     .ThrowsAsync(new DbUpdateConcurrencyException());

        var loggerMock = new Mock<ILogger<SqlRoleRepository>>();
        var repo = new SqlRoleRepository(dbContextMock.Object, loggerMock.Object);

        // Act & Assert
        await FluentActions.Invoking(() => repo.DeleteRoleAsync(roleId))
            .Should().ThrowAsync<ConcurrencyConflictException>()
            .WithMessage("Concurrency error while deleting the role.");
    }

    /// <summary>
    /// Tests that <see cref="SqlRoleRepository.DeleteRoleAsync(int)"/> throws a <see cref="DomainException"/> when a <see cref="DbUpdateException"/> is thrown.
    /// </summary>
    [Fact]
    public async Task DeleteRoleAsync_WhenDbUpdateException_ThrowsDomainException()
    {
        // Arrange
        var roleId = 1;
        var role = new Role("Test") { Id = roleId };
        var roles = new List<Role> { role }.AsQueryable();

        var mockRoles = roles.BuildMockDbSet();

        var dbContextMock = new Mock<ThemeParkDataBaseContext>();
        dbContextMock.Setup(x => x.Roles).Returns(mockRoles.Object);
        dbContextMock.Setup(x => x.Roles.Remove(It.IsAny<Role>()));
        dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                     .ThrowsAsync(new DbUpdateException());

        var loggerMock = new Mock<ILogger<SqlRoleRepository>>();
        var repo = new SqlRoleRepository(dbContextMock.Object, loggerMock.Object);

        // Act & Assert
        await FluentActions.Invoking(() => repo.DeleteRoleAsync(roleId))
            .Should().ThrowAsync<DomainException>()
            .WithMessage("Database update error while deleting the role.");
    }

    /// <summary>
    /// Tests that <see cref="SqlRoleRepository.DeleteRoleAsync(int)"/> throws the original exception when an unexpected exception occurs.
    /// </summary>
    [Fact]
    public async Task DeleteRoleAsync_WhenUnexpectedException_ThrowsOriginalException()
    {
        // Arrange
        var roleId = 1;
        var role = new Role("Test") { Id = roleId };
        var roles = new List<Role> { role }.AsQueryable();

        var mockRoles = roles.BuildMockDbSet();

        var dbContextMock = new Mock<ThemeParkDataBaseContext>();
        dbContextMock.Setup(x => x.Roles).Returns(mockRoles.Object);
        dbContextMock.Setup(x => x.Roles.Remove(It.IsAny<Role>()));
        dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                     .ThrowsAsync(new Exception("Some unexpected error"));

        var loggerMock = new Mock<ILogger<SqlRoleRepository>>();
        var repo = new SqlRoleRepository(dbContextMock.Object, loggerMock.Object);

        // Act & Assert
        await FluentActions.Invoking(() => repo.DeleteRoleAsync(roleId))
            .Should().ThrowAsync<Exception>()
            .WithMessage("Some unexpected error");
    }

    /// <summary>
    /// Tests that <see cref="SqlRoleRepository.GetAllRolesAsync"/> returns a list of roles when roles exist.
    /// </summary>
    [Fact]
    public async Task GetAllRolesAsync_WhenRolesExist_ReturnsList()
    {
        // Arrange
        var roles = new List<Role>
                {
                    new("Admin") { Id = 1 },
                    new("Visitor") { Id = 2 }
                };

        var rolesDbSet = roles.AsQueryable().BuildMockDbSet();

        var dbContextMock = new Mock<ThemeParkDataBaseContext>();
        dbContextMock.Setup(x => x.Roles).Returns(rolesDbSet.Object);

        var loggerMock = new Mock<ILogger<SqlRoleRepository>>();
        var repo = new SqlRoleRepository(dbContextMock.Object, loggerMock.Object);

        // Act
        var result = await repo.GetAllRolesAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(r => r.Name == "Admin");
        result.Should().Contain(r => r.Name == "Visitor");
    }
}
