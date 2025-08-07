using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using Microsoft.AspNetCore.Http.HttpResults;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

public class PostProjectorHandlerAndServiceTest
{
    [Fact]
    public async Task HandleAsync_Should_ReturnOk_WhenProjectorIsAddedSuccessfully()
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

        mockRepository.Setup(r => r.AddComponentAsync(1, It.IsAny<Projector>()))
                 .ReturnsAsync(true);
        var request = new PostProjectorRequest(projector);

        // Act
        var result = await PostProjectorHandler.HandleAsync(service, mapper, request, 1);

        // Assert
        Assert.IsType<Ok<PostProjectorResponse>>(result.Result);
        var okResult = result.Result as Ok<PostProjectorResponse>;
        Assert.NotNull(okResult);
        Assert.NotNull(okResult.Value);

    }

    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenProjectorIsInvalid()
    {
        // Arrange
        var projector = new ProjectorNoIdDto(
    Orientation: "SoutWest",
    Position: new PositionDto(1, 2, 3),
    Dimensions: new DimensionsDto(2, 1, 1),
    ProjectionArea: new ProjectionAreaDto(5, 3),
    ProjectedContent: "Sample Content"
    );
        var mockRepository = new Mock<IProjectorRepository>();
        var service = new ProjectorServices(mockRepository.Object);
        var mapper = new GlobalMapper();
        mockRepository.Setup(r => r.AddComponentAsync(1, It.IsAny<Projector>()))
         .ReturnsAsync(false);
        var request = new PostProjectorRequest(projector);
        // Act
        var result = await PostProjectorHandler.HandleAsync(service, mapper, request, 1);
        // Assert
        Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);
        var badRequestResult = result.Result as BadRequest<List<ValidationError>>;
        Assert.NotNull(badRequestResult);
        Assert.NotEmpty(badRequestResult.Value);
    }
}
