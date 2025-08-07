using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.UniversityManagement;

/// <summary>
/// Unit tests for <see cref="DeleteUniversityHandler"/>.
/// </summary>
public class DeleteUniversityHandlerTests
{
    private readonly Mock<IUniversityServices> _mockService;
    private readonly string _universityName = "TestUniversity";

    public DeleteUniversityHandlerTests()
    {
        _mockService = new Mock<IUniversityServices>();
    }

    /// <summary>
    /// Tests that the handler returns NotFound when the university does not exist.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenUniversityDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var universityName = "NonExistentUniversity";
        _mockService.Setup(x => x.DeleteUniversityAsync(universityName))
            .ReturnsAsync(false);

        // Act
        var result = await DeleteUniversityHandler.HandleAsync(_mockService.Object, universityName);

        // Assert
        var notFound = Assert.IsType<NotFound<DeleteUniversityResponse>>(result.Result);
        notFound.Value!.ErrorMessage.Should().Contain($"Error deleting university with name {universityName}");
    }

    /// <summary>
    /// Tests that <see cref="DeleteUniversityHandler.HandleAsync(IUniversityServices, string)"/> returns a
    /// <see cref="NotFound{T}"/> result when the provided university name is null or empty.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task HandleAsync_WithNullOrEmptyUniversityName_ShouldReturnNotFound(string? invalidName)
    {
        // Arrange
        _mockService.Setup(x => x.DeleteUniversityAsync(It.IsAny<string>()))
            .ReturnsAsync(false); // Assuming the service handles validation

        // Act
        var result = await DeleteUniversityHandler.HandleAsync(_mockService.Object, invalidName);

        // Assert
        var notFound = Assert.IsType<NotFound<DeleteUniversityResponse>>(result.Result);

        notFound.Value!.ErrorMessage.Should().Contain("University name cannot be null or empty.");
    }

    /// <summary>
    /// Tests that the handler returns Ok when the university is successfully deleted.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenUniversityExists_ShouldReturnOk()
    {
        // Arrange
        _mockService.Setup(x => x.DeleteUniversityAsync(_universityName))
            .ReturnsAsync(true);

        // Act
        var result = await DeleteUniversityHandler.HandleAsync(_mockService.Object, _universityName);

        // Assert
        var okResult = Assert.IsType<Ok<string>>(result.Result);
        okResult.Value.Should().Be($"The university {_universityName} has been deleted from the system successfully.");
    }

    /// <summary>
    /// Tests that <see cref="DeleteUniversityHandler.HandleAsync(IUniversityServices, string)"/> throws an exception
    /// when the underlying service throws an exception during the delete operation.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenServiceThrowsException_ShouldThrow()
    {
        // Arrange
        _mockService.Setup(x => x.DeleteUniversityAsync(_universityName))
            .ThrowsAsync(new Exception("Service failure"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() =>
            DeleteUniversityHandler.HandleAsync(_mockService.Object, _universityName));
    }

    /// <summary>
    /// Tests that the handler calls the service with the correct university name.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldCallServiceWithCorrectUniversityName()
    {
        // Arrange
        _mockService.Setup(x => x.DeleteUniversityAsync(_universityName))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        await DeleteUniversityHandler.HandleAsync(_mockService.Object, _universityName);

        // Assert
        _mockService.Verify(x => x.DeleteUniversityAsync(_universityName), Times.Once);
    }
}
