using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using Microsoft.AspNetCore.Http.HttpResults;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using FluentAssertions;

public class DeleteComponentHandlerAndServiceTest
{
    [Fact]
    public async Task HandleAsync_ShouldReturnNoContent_WhenDeletionSucceeds()
    {
        // Arrange
        var mockRepository = new Mock<ILearningComponentRepository>();
        int componentId = 123;

        mockRepository.Setup(r => r.DeleteComponentAsync(componentId))
            .ReturnsAsync(true);

        var service = new LearningComponentServices(mockRepository.Object);
        var mapper = new GlobalMapper();
        // Act
        var result = await DeleteLearningComponentHandler.HandleAsync(service, componentId);
        // Assert
        result.Result.Should().BeOfType<NoContent>();
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNotFound_WhenComponentDoesNotExist()
    {
        // Arrange
        var mockRepository = new Mock<ILearningComponentRepository>();
        int componentId = 123;

        mockRepository.Setup(r => r.DeleteComponentAsync(componentId))
            .ReturnsAsync(false);
        var service = new LearningComponentServices(mockRepository.Object);
        var mapper = new GlobalMapper();
        // Act
        var result = await DeleteLearningComponentHandler.HandleAsync(service, componentId);
        // Assert
        result.Result.Should().BeOfType<NotFound<DeleteLearningComponentResponse>>();


    }
}
