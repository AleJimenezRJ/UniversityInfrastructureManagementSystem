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
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

public class GetSingleComponentHandlerAndServiceTest
{
    [Fact]
    public async Task HandleAsync_ShouldReturnOkWithLearningComponent_WhenComponentExist()
    {
        // Arrange
        var mockRepository = new Mock<ILearningComponentRepository>();
        var fakeComponent = 
        new Projector
        {
            ProjectedContent = "Sample Content",
            ComponentId = 1,
            ProjectionArea = Area2D.Create(5, 3),
            Orientation = Orientation.Create("North"),
            Position = Coordinates.Create(10, 20, 0),
            Dimensions = Dimension.Create(2, 1, 1)
        
    };


        mockRepository.Setup(r => r.GetSingleLearningComponentAsync(1))
             .ReturnsAsync((LearningComponent?)fakeComponent);

        var service = new LearningComponentServices(mockRepository.Object);
        var mapper = new GlobalMapper();

        // Act
        var result = await GetSingleLearningComponentByIdHandler.HandleAsync(service, mapper, fakeComponent.ComponentId);

        // Assert
        Assert.IsType<Ok<GetSingleLearningComponentByIdResponse>>(result.Result);

        var okResult = result.Result as Ok<GetSingleLearningComponentByIdResponse>;
        Assert.NotNull(okResult);
        Assert.NotNull(okResult.Value);

        var dto = okResult.Value.LearningComponent;
        var projectorDto = Assert.IsType<ProjectorDto>(dto);
        projectorDto.ProjectedContent.Should().Be("Sample Content");
        projectorDto.Dimensions.Width.Should().Be(2);

    }

    [Fact]
    public async Task HandleAsync_ShouldReturnBadRequest_WhenComponentIdIsInvalid()
    {
        // Arrange
        var mockRepository = new Mock<ILearningComponentRepository>();
        var service = new LearningComponentServices(mockRepository.Object);
        var mapper = new GlobalMapper();
        // Act
        var result = await GetSingleLearningComponentByIdHandler.HandleAsync(service, mapper, -1);
        // Assert
        Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);
        var badRequestResult = result.Result as BadRequest<List<ValidationError>>;
        Assert.NotNull(badRequestResult);
        Assert.Single(badRequestResult.Value);
        Assert.Equal("Invalid learning component id format.", badRequestResult.Value[0].Message);
    }
}
