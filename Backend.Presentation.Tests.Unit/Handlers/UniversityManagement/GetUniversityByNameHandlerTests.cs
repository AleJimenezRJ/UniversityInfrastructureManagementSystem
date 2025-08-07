using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.UniversityManagement;
/// <summary>
/// Unit tests for <see cref="GetUniversityByNameHandler"/>, which handles HTTP GET requests
/// </summary>
public class GetUniversityByNameHandlerTests
{
    /// <summary>
    /// Mock service for <see cref="IUniversityServices"/> to simulate database operations
    /// </summary>
    private readonly Mock<IUniversityServices> _mockService;

    /// <summary>
    /// Initializes the mock service used to simulate <see cref="IUniversityServices"/> behavior.
    /// </summary>
    public GetUniversityByNameHandlerTests()
    {
        _mockService = new Mock<IUniversityServices>(MockBehavior.Strict);
    }
    /// <summary>
    /// Tests that <see cref="GetUniversityByNameHandler.HandleAsync"/> returns a successful
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenUniversityExists()
    {
        // Arrange
        var testUniversity = TestUniversity("UCR", "Costa Rica");

        _mockService.Setup(s => s.GetByNameAsync("UCR"))
            .ReturnsAsync(testUniversity);

        // Act
        var result = await GetUniversityByNameHandler.HandleAsync(_mockService.Object, "UCR");

        // Assert
        var okResult = Assert.IsType<Ok<GetUniversityByNameResponse>>(result.Result);
        okResult.Value.University.Name.Should().Be("UCR");
        okResult.Value.University.Country.Should().Be("Costa Rica");
    }
    /// <summary>
    /// Tests that <see cref="GetUniversityByNameHandler.HandleAsync"/> returns a NotFound result
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenUniversityDoesNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.GetByNameAsync("UTN"))
            .ReturnsAsync((University?)null);

        // Act
        var result = await GetUniversityByNameHandler.HandleAsync(_mockService.Object, "UTN");

        // Assert
        var notFoundResult = Assert.IsType<NotFound<string>>(result.Result);
        notFoundResult.Value.Should().Be("The requested university was not found.");
    }
    /// <summary>
    /// Tests that <see cref="GetUniversityByNameHandler.HandleAsync"/> returns a NotFound result
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenExceptionThrown()
    {
        // Arrange
        _mockService.Setup(s => s.GetByNameAsync("UCR"))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await GetUniversityByNameHandler.HandleAsync(_mockService.Object, "UCR");

        // Assert
        var notFoundResult = Assert.IsType<NotFound<string>>(result.Result);
        notFoundResult.Value.Should().Contain("Database error");
    }
    /// <summary>
    /// Tests that <see cref="GetUniversityByNameHandler.HandleAsync"/> correctly maps the university data
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldMapUniversityCorrectly()
    {
        // Arrange
        var university = TestUniversity("UNA", "Heredia");

        _mockService.Setup(s => s.GetByNameAsync("UNA"))
            .ReturnsAsync(university);

        // Act
        var result = await GetUniversityByNameHandler.HandleAsync(_mockService.Object, "UNA");

        // Assert
        var okResult = Assert.IsType<Ok<GetUniversityByNameResponse>>(result.Result);
        var dto = okResult.Value.University;

        dto.Name.Should().Be("UNA");
        dto.Country.Should().Be("Heredia");
    }
    /// <summary>
    /// Tests that <see cref="GetUniversityByNameHandler.HandleAsync"/> calls the service method with the correct name
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task HandleAsync_ShouldCallGetByNameOnce_WithCorrectName()
    {
        // Arrange
        var university = TestUniversity("UCR", "SJ");

        _mockService.Setup(s => s.GetByNameAsync("UCR"))
            .ReturnsAsync(university);

        // Act
        await GetUniversityByNameHandler.HandleAsync(_mockService.Object, "UCR");

        // Assert
        _mockService.Verify(s => s.GetByNameAsync("UCR"), Times.Once);
    }

    /// <summary>
    /// Helper static method to create a standardized <see cref="University"/> object for testing.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="country"></param>
    /// <returns></returns>
    private static University TestUniversity(string name, string country)
    {
        return new University(
            new EntityName(name),
            new EntityLocation(country)
        );
    }
}
