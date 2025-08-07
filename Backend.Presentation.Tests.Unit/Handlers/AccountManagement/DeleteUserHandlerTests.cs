using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.AccountManagement;

/// <summary>
/// Tests for the DeleteUserHandler.
/// </summary>
public class DeleteUserHandlerTests
{
    /// <summary>
    /// Mock service for user operations.
    /// </summary>
    private readonly Mock<IUserService> _mockService = new();

    /// <summary>
    /// Tests the HandleAsync method of the DeleteUserHandler to ensure it returns Ok when a user is deleted successfully.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        // Arrange
        int invalidId = -1;

        // Act
        var result = await DeleteUserHandler.HandleAsync(invalidId, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
    }

    /// <summary>
    /// Tests the HandleAsync method of the DeleteUserHandler to ensure it returns NotFound when the user does not exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        int id = 1;
        _mockService
            .Setup(s => s.DeleteUserAsync(id))
            .ThrowsAsync(new NotFoundException("User not found"));

        // Act
        var result = await DeleteUserHandler.HandleAsync(id, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
        ((NotFound<string>)result.Result!).Value.Should().ContainEquivalentOf("user not found");
    }

    /// <summary>
    /// Tests the HandleAsync method of the DeleteUserHandler to ensure it returns Conflict when a domain exception occurs.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenDomainExceptionOccurs()
    {
        // Arrange
        int id = 1;
        _mockService
            .Setup(s => s.DeleteUserAsync(id))
            .ThrowsAsync(new DomainException("User cannot be deleted"));

        // Act
        var result = await DeleteUserHandler.HandleAsync(id, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<Conflict<string>>();
        ((Conflict<string>)result.Result!).Value.Should().ContainEquivalentOf("cannot be deleted");
    }

    /// <summary>
    /// Tests the HandleAsync method of the DeleteUserHandler to ensure it returns Ok when a user is deleted successfully.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenUserDeletedSuccessfully()
    {
        // Arrange
        int id = 1;
        _mockService
            .Setup(s => s.DeleteUserAsync(id))
            .ReturnsAsync(true);

        // Act
        var result = await DeleteUserHandler.HandleAsync(id, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<Ok<string>>();
        ((Ok<string>)result.Result!).Value.Should().Be("User deleted successfully.");
    }
}
