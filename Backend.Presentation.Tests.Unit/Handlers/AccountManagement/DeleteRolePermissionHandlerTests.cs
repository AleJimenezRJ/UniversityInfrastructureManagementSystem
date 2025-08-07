using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.AccountManagement;

/// <summary>
///  Tests for the DeleteRolePermissionHandler.
/// </summary>
public class DeleteRolePermissionHandlerTests
{
    /// <summary>
    /// Mock service for role permission operations.
    /// </summary>
    private readonly Mock<IRolePermissionService> _mockService = new();

    /// <summary>
    /// Tests the HandleAsync method of the DeleteRolePermissionHandler to ensure it returns Ok when a role permission is deleted successfully.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenDeletionIsSuccessful()
    {
        // Arrange
        var request = new DeleteRolePermissionRequest { RoleId = 1, PermId = 2 };

        _mockService
            .Setup(s => s.RemovePermissionFromRoleAsync(request.RoleId, request.PermId))
            .ReturnsAsync(true);

        // Act
        var result = await DeleteRolePermissionHandler.HandleAsync(request, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<Ok<DeleteRolePermissionResponse>>();
    }

    /// <summary>
    /// Tests the HandleAsync method of the DeleteRolePermissionHandler to ensure it returns BadRequest when the role ID or permission ID is invalid.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenRoleIdOrPermIdIsInvalid()
    {
        // Arrange
        var request = new DeleteRolePermissionRequest { RoleId = 0, PermId = -1 };

        // Act
        var result = await DeleteRolePermissionHandler.HandleAsync(request, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
    }

    /// <summary>
    /// Tests the HandleAsync method of the DeleteRolePermissionHandler to ensure it returns NotFound when the association does not exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenAssociationDoesNotExist()
    {
        // Arrange
        var request = new DeleteRolePermissionRequest { RoleId = 1, PermId = 1 };

        _mockService
            .Setup(s => s.RemovePermissionFromRoleAsync(request.RoleId, request.PermId))
            .ThrowsAsync(new NotFoundException("Not found"));

        // Act
        var result = await DeleteRolePermissionHandler.HandleAsync(request, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
        ((NotFound<string>)result.Result!).Value.Should().ContainEquivalentOf("not found");

    }

    /// <summary>
    /// Tests the HandleAsync method of the DeleteRolePermissionHandler to ensure it returns Conflict when a domain exception occurs.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenDomainExceptionOccurs()
    {
        // Arrange
        var request = new DeleteRolePermissionRequest { RoleId = 1, PermId = 2 };

        _mockService
            .Setup(s => s.RemovePermissionFromRoleAsync(request.RoleId, request.PermId))
            .ThrowsAsync(new DomainException("conflict error"));

        // Act
        var result = await DeleteRolePermissionHandler.HandleAsync(request, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<Conflict<string>>();
        ((Conflict<string>)result.Result!).Value.Should().Contain("conflict");
    }
}
