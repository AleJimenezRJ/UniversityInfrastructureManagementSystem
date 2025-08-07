using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;


namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.ComponentsManagement;


/// <summary>
/// Unit tests for <see cref="DeleteLearningComponentHandler"/>.
/// </summary>
public class DeleteLearningComponentHandlerTests
{
    /// <summary>
    /// Tests that the handler returns NoContent when the deletion is successful.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenDeletionSucceeds_ShouldReturnNoContent()
    {
        // Arrange
        var componentServiceMock = new Mock<ILearningComponentServices>();
        int componentId = 123;
        componentServiceMock.Setup(s => s.DeleteLearningComponentAsync(componentId))
            .ReturnsAsync(true);

        // Act
        var result = await DeleteLearningComponentHandler.HandleAsync(componentServiceMock.Object, componentId);

        // Assert
        result.Result.Should().BeOfType<NoContent>();
    }

    /// <summary>
    /// Tests that the handler returns NotFound when the component does not exist.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenComponentDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var componentServiceMock = new Mock<ILearningComponentServices>();
        int componentId = 999;
        componentServiceMock.Setup(s => s.DeleteLearningComponentAsync(componentId))
            .ReturnsAsync(false);

        // Act
        var result = await DeleteLearningComponentHandler.HandleAsync(componentServiceMock.Object, componentId);

        // Assert
        var notFound = Assert.IsType<NotFound<DeleteLearningComponentResponse>>(result.Result);
        notFound.Value!.ErrorMessage.Should().Contain($"Error deleting whiteboard with id {componentId}");
    }

    /// <summary>
    /// Tests that the service is called with the correct ID.
    /// </summary>
    [Fact]
    public async Task HandleAsync_ShouldCallServiceWithCorrectId()
    {
        // Arrange
        var componentServiceMock = new Mock<ILearningComponentServices>();
        int expectedId = 456;
        componentServiceMock.Setup(s => s.DeleteLearningComponentAsync(expectedId))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        await DeleteLearningComponentHandler.HandleAsync(componentServiceMock.Object, expectedId);

        // Assert
        componentServiceMock.Verify(s => s.DeleteLearningComponentAsync(expectedId), Times.Once);
    }
}
