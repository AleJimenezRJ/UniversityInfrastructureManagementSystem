using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;
using Moq;
using Xunit;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using Microsoft.AspNetCore.Http.HttpResults;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using Microsoft.OpenApi.Any;

public class PutProjectorHandlerAndServiceTest
{
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WhenProjectorIsUpdatedSuccessfully()
    {
        // Arrange
        var mockRepository = new Mock<IProjectorRepository>();

        var projector = new ProjectorNoIdDto(
            Orientation: "North",
            Position: new PositionDto(1, 2, 3),
            Dimensions: new DimensionsDto(2, 1, 1),
            ProjectionArea: new ProjectionAreaDto(5, 3),
            ProjectedContent: "Sample Content"
        );
        var service = new ProjectorServices(mockRepository.Object);
        var mapper = new GlobalMapper();

        mockRepository.Setup(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Projector>()))
         .ReturnsAsync(true);
        var request = new PutProjectorRequest(projector);
        // Act
        var result = await PutProjectorHandler.HandleAsync(service, mapper, request, 1,2);

        // Assert
        Assert.IsType<Ok<PutProjectorResponse>>(result.Result);
        var okResult = result.Result as Ok<PutProjectorResponse>;
        Assert.NotNull(okResult);
        Assert.NotNull(okResult.Value);

    }

    [Fact]
    public async Task HandleAsync_ShouldReturnConflict_WhenProjectorUpdateFail()
    {
        // Arrange
        var projector = new ProjectorNoIdDto(
            Orientation: "South",
            Position: new PositionDto(1, 2, 3),
            Dimensions: new DimensionsDto(2, 1, 1),
            ProjectionArea: new ProjectionAreaDto(5, 3),
            ProjectedContent: "Sample Content"
        );
        var mockRepository = new Mock<IProjectorRepository>();
        mockRepository.Setup(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Projector>()))
        .ReturnsAsync(false);
        var service = new ProjectorServices(mockRepository.Object);
        var mapper = new GlobalMapper();
        mockRepository.Setup(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Projector>()))
         .ReturnsAsync(false);
        var request = new PutProjectorRequest(projector);
        // Act
        var result = await PutProjectorHandler.HandleAsync(service, mapper, request, 1, 2);
        // Assert
        Assert.IsType<Conflict>(result.Result);
    }
}
