using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using Microsoft.AspNetCore.Http.HttpResults;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using FluentAssertions;

public class GetAllHandlerAndServiceTest
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnOkWithLearningComponents_WhenComponentsExist()
    {
        // Arrange
        var mockRepository = new Mock<ILearningComponentRepository>();
        var fakeComponents = new List<LearningComponent>
    {
        new Projector
        {
            ProjectedContent = "Sample Content",
            ProjectionArea = Area2D.Create(5, 3),
            Orientation = Orientation.Create("North"),
            Position = Coordinates.Create(10, 20, 0),
            Dimensions = Dimension.Create(2, 1, 1)
        },
        new Whiteboard
        {
            MarkerColor = Colors.Create("Blue"),
            Orientation = Orientation.Create("East"),
            Position = Coordinates.Create(15, 25, 0),
            Dimensions = Dimension.Create(1, 2, 3)
        }
    };

        mockRepository.Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                      .ReturnsAsync(fakeComponents.AsEnumerable());

        var service = new LearningComponentServices(mockRepository.Object);
        var mapper = new GlobalMapper();

        // Act
        var result = await GetLearningComponentHandler.HandleAsync(service, mapper, 10, 0);

        // Assert
        Assert.IsType<Ok<GetLearningComponentResponse>>(result.Result);

        var okResult = result.Result as Ok<GetLearningComponentResponse>;
        Assert.NotNull(okResult);
        Assert.NotNull(okResult.Value);

        var dto = okResult.Value.LearningComponents.First();
        var projectorDto = Assert.IsType<ProjectorDto>(dto);
        projectorDto.ProjectedContent.Should().Be("Sample Content");
        projectorDto.Dimensions.Width.Should().Be(2);

        var whiteboardDto = Assert.IsType<WhiteboardDto>(okResult.Value.LearningComponents.Last());
        whiteboardDto.MarkerColor.Should().Be("Blue");
        whiteboardDto.Dimensions.Height.Should().Be(3);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOkWithEmptyList_WhenNoComponentsExist()
    {
        // Arrange
        var mockRepository = new Mock<ILearningComponentRepository>();
        mockRepository.Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                      .ReturnsAsync(Enumerable.Empty<LearningComponent>());
        var service = new LearningComponentServices(mockRepository.Object);
        var mapper = new GlobalMapper();
        // Act
        var result = await GetLearningComponentHandler.HandleAsync(service, mapper, 10, 0);
        // Assert
        Assert.IsType<Ok<GetLearningComponentResponse>>(result.Result);
        var okResult = result.Result as Ok<GetLearningComponentResponse>;
        okResult.Value!.LearningComponents.Should().BeEmpty();
    }
}
