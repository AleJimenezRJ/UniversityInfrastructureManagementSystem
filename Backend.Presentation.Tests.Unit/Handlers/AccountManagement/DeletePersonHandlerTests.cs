using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.AccountManagement;

/// <summary>
/// Tests for the DeletePersonHandler.
/// </summary>
public class DeletePersonHandlerTests
{
    /// <summary>
    /// Mock service for testing person deletion functionality.
    /// </summary>
    private readonly Mock<IPersonService> _mockService = new();

    /// <summary>
    /// Tests the HandleAsync method of the DeletePersonHandler to ensure it returns Ok when a person is deleted successfully.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenPersonIsDeletedSuccessfully()
    {
        // Arrange
        var identity = "1-8888-0000";
        var identityObj = IdentityNumber.Create(identity);

        _mockService.Setup(s => s.DeletePersonAsync(identityObj.Value)).ReturnsAsync(true);


        // Act
        var result = await DeletePersonHandler.HandleAsync(identity, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<Ok<string>>();
        ((Ok<string>)result.Result!).Value.Should().Contain("successfully");
    }

    /// <summary>
    /// Tests the HandleAsync method of the DeletePersonHandler to ensure it returns BadRequest when the identity number is invalid.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenIdentityNumberIsInvalid()
    {
        // Arrange
        var identity = "INVALID";

        // Act
        var result = await DeletePersonHandler.HandleAsync(identity, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<BadRequest<ErrorResponse>>();
        var response = ((BadRequest<ErrorResponse>)result.Result!).Value;
        response.Should().NotBeNull();
    }

    /// <summary>
    /// Tests the HandleAsync method of the DeletePersonHandler to ensure it returns NotFound when the person does not exist.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenPersonDoesNotExist()
    {
        // Arrange
        var identity = "1-8888-0000";
        var identityObj = IdentityNumber.Create(identity);

        _mockService
            .Setup(s => s.DeletePersonAsync(It.Is<string>(n => n == identityObj.Value)))
            .ThrowsAsync(new NotFoundException("Not found"));


        // Act
        var result = await DeletePersonHandler.HandleAsync(identity, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<NotFound<string>>();
        var value = ((NotFound<string>)result.Result!).Value;
        value.Should().Contain("was not found");
    }

    /// <summary>
    /// Tests the HandleAsync method of the DeletePersonHandler to ensure it returns BadRequest when an unexpected exception occurs.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation.</returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenUnexpectedExceptionOccurs()
    {
        // Arrange
        var identity = "1-8888-0000";

        _mockService
            .Setup(s => s.DeletePersonAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Something unexpected"));


        // Act
        var result = await DeletePersonHandler.HandleAsync(identity, _mockService.Object);

        // Assert
        result.Result.Should().BeOfType<BadRequest<ErrorResponse>>();
    }
}
