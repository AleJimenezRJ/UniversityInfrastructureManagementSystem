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
/// Tests for the DeleteRoleHandler.
/// </summary>
public class DeleteRoleHandlerTests
{
    /// <summary>
    /// Mock service for role management operations.
    /// </summary>
    private readonly Mock<IRoleService> _mockService = new();

    /// <summary>
    /// Tests the HandleAsync method of the DeleteRoleHandler to ensure it returns Ok when a role is deleted successfully.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenRoleDeletedSuccessfully()
    {
        // Arrange
        var request = new DeleteRoleRequest { Id = 1 };

        _mockService
            .Setup(s => s.DeleteRoleAsync(request.Id))
            .ReturnsAsync(true);

        // Act
        var result = await DeleteRoleHandler.HandleAsync(request, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<Ok<DeleteRoleResponse>>();
        var response = ((Ok<DeleteRoleResponse>)result.Result!).Value;
        response.Message.Should().Contain("has been deleted successfully");
    }

    /// <summary>
    /// Tests the HandleAsync method of the DeleteRoleHandler to ensure it returns BadRequest when the role ID is invalid.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        // Arrange
        var request = new DeleteRoleRequest { Id = -10 };

        // Act
        var result = await DeleteRoleHandler.HandleAsync(request, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
    }

    /// <summary>
    /// Tests the HandleAsync method of the DeleteRoleHandler to ensure it returns NotFound when the role does not exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenRoleDoesNotExist()
    {
        // Arrange
        var request = new DeleteRoleRequest { Id = 42 };

        _mockService
            .Setup(s => s.DeleteRoleAsync(request.Id))
            .ThrowsAsync(new NotFoundException("Role not found"));

        // Act
        var result = await DeleteRoleHandler.HandleAsync(request, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
        var msg = ((NotFound<string>)result.Result!).Value;
        msg.Should().Contain("not found");
    }

    /// <summary>
    /// Tests the HandleAsync method of the DeleteRoleHandler to ensure it returns Conflict when a domain exception occurs during deletion.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenDomainExceptionOccurs()
    {
        // Arrange
        var request = new DeleteRoleRequest { Id = 5 };

        _mockService
            .Setup(s => s.DeleteRoleAsync(request.Id))
            .ThrowsAsync(new DomainException("Role is in use"));

        // Act
        var result = await DeleteRoleHandler.HandleAsync(request, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<Conflict<string>>();
        var msg = ((Conflict<string>)result.Result!).Value;
        msg.Should().Contain("in use");
    }
}
