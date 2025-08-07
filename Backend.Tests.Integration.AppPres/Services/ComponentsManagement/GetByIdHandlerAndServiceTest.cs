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

public class GetByIdHandlerAndServiceTest
{
    [Fact]
    public async Task HandleAsync_ShouldReturnOkWithLearningComponents_WhenComponentsExist()
    {
        // Arrange
        var mockRepository = new Mock<ILearningComponentRepository>();
        var fakeComponentsList =
           new List<LearningComponent>
           {
               new Projector
               {
                   ProjectedContent = "Sample Content",
                   ProjectionArea = Area2D.Create(5, 3),
                   ComponentId = 1,
                   Orientation = Orientation.Create("North"),
                   Position = Coordinates.Create(10, 20, 0),
                   Dimensions = Dimension.Create(2, 1, 1)
               },
               new Whiteboard
               {
                   MarkerColor = Colors.Create("Blue"),
                   ComponentId = 2,
                   Orientation = Orientation.Create("East"),
                   Position = Coordinates.Create(15, 25, 0),
                   Dimensions = Dimension.Create(1, 2, 3)
               }
           };
        var fakeComponents = new PaginatedList<LearningComponent>(fakeComponentsList,
         2,
         10,
         0
      );
        int learningSpaceId = 1;

        mockRepository.Setup(r => r.GetLearningComponentsByIdAsync(1, 10, 0, ""))
                     .ReturnsAsync(fakeComponents);

        var service = new LearningComponentServices(mockRepository.Object);
        var mapper = new GlobalMapper();

        // Act
        var result = await GetLearningComponentsByIdHandler.HandleAsync(service, mapper, 1, "");

        // Assert
        Assert.IsType<Ok<GetLearningComponentsByIdResponse>>(result.Result);

        var okResult = result.Result as Ok<GetLearningComponentsByIdResponse>;
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
    public async Task HandleAsync_ShouldReturnOkWithEmptyList_WhenNoComponentsExist()
    {
        var fakeComponentsList =
           new List<LearningComponent>
           { };
        var fakeComponents = new PaginatedList<LearningComponent>(fakeComponentsList,
          2,
          10,
          0);
        // Arrange
        var mockRepository = new Mock<ILearningComponentRepository>();
        mockRepository.Setup(r => r.GetLearningComponentsByIdAsync(1, 10, 0, ""))
                     .ReturnsAsync(fakeComponents);
        var service = new LearningComponentServices(mockRepository.Object);
        var mapper = new GlobalMapper();
        // Act
        var result = await GetLearningComponentsByIdHandler.HandleAsync(service, mapper, 1, "");
        // Assert
        Assert.IsType<Ok<GetLearningComponentsByIdResponse>>(result.Result);
        var okResult = result.Result as Ok<GetLearningComponentsByIdResponse>;
        okResult.Value.LearningComponents.Should().BeEmpty();
    }
}
