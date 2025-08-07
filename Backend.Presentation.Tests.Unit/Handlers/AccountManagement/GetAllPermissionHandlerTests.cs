using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.AccountManagement;

/// <summary>
/// Tests for the GetAllPermissionHandler.
/// </summary>
public class GetAllPermissionHandlerTests
{
    /// <summary>
    /// Mock service for permissions.
    /// </summary>
    private readonly Mock<IPermissionService> _mockService = new();

    /// <summary>
    /// Tests the HandleAsync method of the GetAllPermissionsHandler to ensure it returns Ok when permissions exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenPermissionsExist()
    {
        // Arrange
        var permissions = new List<Permission>
        {
            new Permission("Manage users", 1),
            new Permission("Delete roles", 2)
        };

        _mockService
            .Setup(s => s.GetAllPermissionsAsync())
            .ReturnsAsync(permissions);

        // Act
        var result = await GetAllPermissionsHandler.HandleAsync(_mockService.Object);

        // Assert
        result.Result.Should().BeOfType<Ok<GetAllPermissionsResponse>>();

        var value = ((Ok<GetAllPermissionsResponse>)result.Result!).Value;
        value.Permissions.Should().HaveCount(2);
        value.Permissions[0].Id.Should().Be(1);
        value.Permissions[0].Description.Should().Be("Manage users");
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllPermissionsHandler to ensure it returns NotFound when no permissions exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenNoPermissionsExist()
    {
        // Arrange
        _mockService
            .Setup(s => s.GetAllPermissionsAsync())
            .ReturnsAsync(new List<Permission>());

        // Act
        var result = await GetAllPermissionsHandler.HandleAsync(_mockService.Object);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
        ((NotFound<string>)result.Result!).Value
         .IndexOf("no registered permissions", StringComparison.OrdinalIgnoreCase)
         .Should().BeGreaterThan(-1);

    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllPermissionsHandler to ensure it returns BadRequest when the service throws a validation exception.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenDomainExceptionOccurs()
    {
        // Arrange
        _mockService
            .Setup(s => s.GetAllPermissionsAsync())
            .ThrowsAsync(new DomainException("Error fetching permissions"));

        // Act
        var result = await GetAllPermissionsHandler.HandleAsync(_mockService.Object);

        // Assert
        result.Result.Should().BeOfType<Conflict<string>>();
        ((Conflict<string>)result.Result!).Value.Should().Contain("Error fetching permissions");
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllPermissionsHandler to ensure it returns expected descriptions when permissions vary.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnExpectedDescriptions_WhenDescriptionsVary()
    {
        // Arrange
        var permissions = new List<Permission>
    {
        new Permission("View reports", 1),
        new Permission("Export data", 2)
    };

        _mockService
            .Setup(s => s.GetAllPermissionsAsync())
            .ReturnsAsync(permissions);

        // Act
        var result = await GetAllPermissionsHandler.HandleAsync(_mockService.Object);

        // Assert
        var response = ((Ok<GetAllPermissionsResponse>)result.Result!).Value;
        response.Permissions.Should().ContainSingle(p => p.Description == "View reports");
        response.Permissions.Should().ContainSingle(p => p.Description == "Export data");
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllPermissionsHandler to ensure it maps permission IDs correctly.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldMapCorrectIds()
    {
        // Arrange
        var permissions = new List<Permission>
    {
        new Permission("Access dashboard", 10),
        new Permission("Configure system", 20)
    };

        _mockService
            .Setup(s => s.GetAllPermissionsAsync())
            .ReturnsAsync(permissions);

        // Act
        var result = await GetAllPermissionsHandler.HandleAsync(_mockService.Object);

        // Assert
        var value = ((Ok<GetAllPermissionsResponse>)result.Result!).Value;
        value.Permissions.Should().Contain(p => p.Id == 10);
        value.Permissions.Should().Contain(p => p.Id == 20);
    }
}
