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
/// Tests for the DeleteUserRoleHandler.
/// </summary>
public class DeleteUserRoleHandlerTests
{
    /// <summary>
    /// Mock service for user-role operations.
    /// </summary>
    private readonly Mock<IUserRoleService> _mockService = new();

    /// <summary>
    /// Tests the HandleAsync method of the DeleteUserRoleHandler to ensure it returns Ok when a user-role association is deleted successfully.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenIdsAreInvalid()
    {
        // Arrange
        var request = new DeleteUserRoleRequest { UserId = 0, RoleId = -1 };

        // Act
        var result = await DeleteUserRoleHandler.HandleAsync(request, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
    }

    /// <summary>
    /// Tests the HandleAsync method of the DeleteUserRoleHandler to ensure it returns NotFound when the user-role association does not exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenAssociationDoesNotExist()
    {
        // Arrange
        var request = new DeleteUserRoleRequest { UserId = 1, RoleId = 2 };

        _mockService
            .Setup(s => s.RemoveRoleAsync(request.UserId, request.RoleId))
            .ThrowsAsync(new NotFoundException("Association not found"));

        // Act
        var result = await DeleteUserRoleHandler.HandleAsync(request, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
        ((NotFound<string>)result.Result!).Value.Should().ContainEquivalentOf("association not found");
    }

    /// <summary>
    /// Tests the HandleAsync method of the DeleteUserRoleHandler to ensure it returns Conflict when a domain exception occurs.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenDomainExceptionOccurs()
    {
        // Arrange
        var request = new DeleteUserRoleRequest { UserId = 1, RoleId = 2 };

        _mockService
            .Setup(s => s.RemoveRoleAsync(request.UserId, request.RoleId))
            .ThrowsAsync(new DomainException("Cannot remove role"));

        // Act
        var result = await DeleteUserRoleHandler.HandleAsync(request, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<Conflict<string>>();
        ((Conflict<string>)result.Result!).Value.Should().ContainEquivalentOf("cannot remove role");
    }

    /// <summary>
    /// Tests the HandleAsync method of the DeleteUserRoleHandler to ensure it returns Ok when the user-role association is removed successfully.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenAssociationRemovedSuccessfully()
    {
        // Arrange
        var request = new DeleteUserRoleRequest { UserId = 1, RoleId = 2 };

        _mockService
            .Setup(s => s.RemoveRoleAsync(request.UserId, request.RoleId))
            .ReturnsAsync(true);

        // Act
        var result = await DeleteUserRoleHandler.HandleAsync(request, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<Ok<DeleteUserRoleResponse>>();
        ((Ok<DeleteUserRoleResponse>)result.Result!).Value.Message
            .Should().Contain("UserId 1").And.Contain("RoleId 2");
    }
}
