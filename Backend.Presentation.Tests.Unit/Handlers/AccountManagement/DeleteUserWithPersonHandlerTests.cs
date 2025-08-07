using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.AccountManagement;

/// <summary>
/// Tests for the DeleteUserWithPersonHandler.
/// </summary>
public class DeleteUserWithPersonHandlerTests
{
    /// <summary>
    /// Mock service for user-person operations.
    /// </summary>
    private readonly Mock<IUserWithPersonService> _mockService = new();

    /// <summary>
    /// Tests the HandleAsync method of the DeleteUserWithPersonHandler to ensure it returns BadRequest when user or person IDs are invalid.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenIdsAreInvalid()
    {
        // Arrange
        var request = new DeleteUserWithPersonRequest { UserId = -5, PersonId = 0 };

        // Act
        var result = await DeleteUserWithPersonHandler.HandleAsync(request, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<BadRequest<List<ValidationError>>>();
    }

    /// <summary>
    /// Tests the HandleAsync method of the DeleteUserWithPersonHandler to ensure it returns NotFound when the user does not exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new DeleteUserWithPersonRequest { UserId = 1, PersonId = 2 };

        _mockService
            .Setup(s => s.DeleteUserWithPersonAsync(request.UserId, request.PersonId))
            .ThrowsAsync(new NotFoundException("User not found"));

        // Act
        var result = await DeleteUserWithPersonHandler.HandleAsync(request, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
        ((NotFound<string>)result.Result!).Value.Should().ContainEquivalentOf("user not found");
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenDomainExceptionOccurs()
    {
        // Arrange
        var request = new DeleteUserWithPersonRequest { UserId = 3, PersonId = 4 };

        _mockService
            .Setup(s => s.DeleteUserWithPersonAsync(request.UserId, request.PersonId))
            .ThrowsAsync(new DomainException("Deletion not allowed"));

        // Act
        var result = await DeleteUserWithPersonHandler.HandleAsync(request, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<Conflict<string>>();
        ((Conflict<string>)result.Result!).Value.Should().ContainEquivalentOf("deletion not allowed");
    }

    /// <summary>
    /// Tests the HandleAsync method of the DeleteUserWithPersonHandler to ensure it returns Ok when the user is deleted successfully.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenUserDeletedSuccessfully()
    {
        // Arrange
        var request = new DeleteUserWithPersonRequest { UserId = 7, PersonId = 8 };

        _mockService
            .Setup(s => s.DeleteUserWithPersonAsync(request.UserId, request.PersonId))
            .ReturnsAsync(true);

        // Act
        var result = await DeleteUserWithPersonHandler.HandleAsync(request, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<Ok<string>>();
        ((Ok<string>)result.Result!).Value.Should().Contain("User with ID 7");
    }
}
