using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.UniversityManagement;

/// <summary>
/// Unit tests for <see cref="DeleteBuildingHandler"/>.
/// </summary>
public class DeleteBuildingHandlerTests
{



    [Fact]
    public async Task HandleAsync_WhenBuildingDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var buildingServiceMock = new Mock<IBuildingsServices>();
        int buildingId = 999;

        buildingServiceMock.Setup(s => s.DeleteBuildingAsync(buildingId))
            .ReturnsAsync(false);

        // Act
        var result = await DeleteBuildingHandler.HandleAsync(buildingServiceMock.Object, buildingId);

        // Assert
        var notFound = Assert.IsType<NotFound<DeleteBuildingResponse>>(result.Result);
        notFound.Value!.ErrorMessage.Should().Contain($"Error deleting building with id {buildingId}");
    }

    [Fact]
    public async Task HandleAsync_WhenConcurrencyConflictOccurs_ShouldReturnConflict()
    {
        // Arrange
        var buildingServiceMock = new Mock<IBuildingsServices>();
        int buildingId = 456;

        buildingServiceMock.Setup(s => s.DeleteBuildingAsync(buildingId))
            .ThrowsAsync(new ConcurrencyConflictException("Concurrent update detected."));

        // Act
        var result = await DeleteBuildingHandler.HandleAsync(buildingServiceMock.Object, buildingId);

        // Assert
        var conflict = Assert.IsType<Conflict<string>>(result.Result);
        conflict.Value.Should().Contain("concurrency conflict");
    }

    [Fact]
    public async Task HandleAsync_ShouldCallServiceWithCorrectId()
    {
        // Arrange
        var buildingServiceMock = new Mock<IBuildingsServices>();
        int expectedId = 789;

        buildingServiceMock.Setup(s => s.DeleteBuildingAsync(expectedId))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        await DeleteBuildingHandler.HandleAsync(buildingServiceMock.Object, expectedId);

        // Assert
        buildingServiceMock.Verify(s => s.DeleteBuildingAsync(expectedId), Times.Once);
    }
}
