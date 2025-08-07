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
/// Unit tests for the <see cref="SqlPermissionRepository"/> class.
/// </summary>
public class SqlPermissionRepositoryTests
{
    /// <summary>
    /// Tests that <see cref="SqlPermissionRepository.GetAllPermissionsAsync"/> returns a list of permissions
    /// when permissions exist in the database.
    /// </summary>
    [Fact]
    public async Task GetAllPermissionsAsync_WhenPermissionsExist_ReturnsPermissionList()
    {
        // Arrange
        var permissions = new List<Permission>
                        {
                            new("ReadUsers"),
                            new("WriteUsers"),
                            new("DeleteUsers"),
                        };

        var mockPermissionsDbSet = permissions.BuildMock().BuildMockDbSet();

        var dbContextMock = new Mock<ThemeParkDataBaseContext>();
        dbContextMock
            .Setup(db => db.Permissions)
            .Returns(mockPermissionsDbSet.Object);

        var loggerMock = new Mock<ILogger<SqlPermissionRepository>>();

        var repository = new SqlPermissionRepository(dbContextMock.Object, loggerMock.Object);

        // Act
        var result = await repository.GetAllPermissionsAsync();

        // Assert
        result.Should().BeEquivalentTo(permissions, "because the database contains this list of permissions");
    }

    /// <summary>
    /// Tests that <see cref="SqlPermissionRepository.GetAllPermissionsAsync"/> returns an empty list
    /// when no permissions exist in the database.
    /// </summary>
    [Fact]
    public async Task GetAllPermissionsAsync_WhenNoPermissionsExist_ReturnsEmptyList()
    {
        // Arrange
        var permissions = new List<Permission>();

        var mockPermissionsDbSet = permissions.BuildMock().BuildMockDbSet();

        var dbContextMock = new Mock<ThemeParkDataBaseContext>();
        dbContextMock
            .Setup(db => db.Permissions)
            .Returns(mockPermissionsDbSet.Object);

        var loggerMock = new Mock<ILogger<SqlPermissionRepository>>();

        var repository = new SqlPermissionRepository(dbContextMock.Object, loggerMock.Object);

        // Act
        var result = await repository.GetAllPermissionsAsync();

        // Assert
        result.Should().BeEmpty("because the database contains no permissions");
    }

    /// <summary>
    /// Tests that <see cref="SqlPermissionRepository.GetAllPermissionsAsync"/> returns an empty list
    /// and logs an error when an exception is thrown while accessing the database.
    /// </summary>
    [Fact]
    public async Task GetAllPermissionsAsync_WhenExceptionThrown_ThrowsDomainExceptionAndLogsError()
    {
        // Arrange
        var dbContextMock = new Mock<ThemeParkDataBaseContext>();
        dbContextMock
            .Setup(db => db.Permissions)
            .Throws(new Exception("Database error"));

        var loggerMock = new Mock<ILogger<SqlPermissionRepository>>();
        var repository = new SqlPermissionRepository(dbContextMock.Object, loggerMock.Object);

        // Act
        Func<Task> act = async () => await repository.GetAllPermissionsAsync();

        // Assert
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("An error occurred while retrieving permissions.");

        loggerMock.Verify(
            log => log.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, t) => state.ToString()!.Contains("Failed to retrieve permissions")),
                It.Is<Exception>(ex => ex.Message == "Database error"),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}

