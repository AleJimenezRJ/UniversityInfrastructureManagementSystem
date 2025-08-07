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
/// Tests for the GetAllRolesHandler.
/// </summary>
public class GetAllRolesHandlerTests
{
    /// <summary>
    /// Mock service for role management operations.
    /// </summary>
    private readonly Mock<IRoleService> _mockService = new();

    /// <summary>
    /// Tests the HandleAsync method of the GetAllRolesHandler to ensure it returns Ok when roles exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenRolesExist()
    {
        // Arrange
        var roles = new List<Role>
    {
        new Role("Admin"),
        new Role("Editor")
    };

        _mockService
            .Setup(s => s.GetAllRolesAsync())
            .ReturnsAsync(roles);

        // Act
        var result = await GetAllRolesHandler.HandleAsync(_mockService.Object);

        // Assert
        result.Result.Should().BeOfType<Ok<GetAllRolesResponse>>();

        var value = ((Ok<GetAllRolesResponse>)result.Result!).Value;
        value.Roles.Should().HaveCount(2);
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllRolesHandler to ensure it returns NotFound when no roles exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenNoRolesExist()
    {
        _mockService
            .Setup(s => s.GetAllRolesAsync())
            .ReturnsAsync(new List<Role>());

        var result = await GetAllRolesHandler.HandleAsync(_mockService.Object);

        result.Result.Should().BeOfType<NotFound<string>>();
        ((NotFound<string>)result.Result!).Value
            .IndexOf("no registered roles", StringComparison.OrdinalIgnoreCase)
            .Should().BeGreaterThan(-1);
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllRolesHandler to ensure it returns BadRequest when the service throws a validation exception.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenDomainExceptionIsThrown()
    {
        _mockService
            .Setup(s => s.GetAllRolesAsync())
            .ThrowsAsync(new DomainException("Error retrieving roles"));

        var result = await GetAllRolesHandler.HandleAsync(_mockService.Object);

        result.Result.Should().BeOfType<Conflict<string>>();
        ((Conflict<string>)result.Result!).Value.Should().Contain("Error retrieving roles");
    }

    /// <summary>
    /// Tests the HandleAsync method of the GetAllRolesHandler to ensure it returns roles with correct names.
    /// </summary>
    /// <return> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnRolesWithCorrectNames()
    {
        var roles = new List<Role>
    {
        new Role("Admin"),
        new Role("Viewer")
    };

        _mockService
            .Setup(s => s.GetAllRolesAsync())
            .ReturnsAsync(roles);

        var result = await GetAllRolesHandler.HandleAsync(_mockService.Object);

        var value = ((Ok<GetAllRolesResponse>)result.Result!).Value;
        value.Roles.Select(r => r.Name).Should().Contain(new[] { "Admin", "Viewer" });
    }
}
