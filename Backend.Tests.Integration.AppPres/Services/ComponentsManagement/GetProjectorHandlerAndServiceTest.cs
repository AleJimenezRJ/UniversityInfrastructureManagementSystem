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

public class GetProjectorHandlerAndServiceTest
{
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WithProjectorDtos_WhenServiceReturnsProjectors()
    {
        // Arrange
        var mockRepository = new Mock<IProjectorRepository>();
        var fakeComponents = new List<Projector>
    {
        new Projector
        {
            ProjectedContent = "Sample Content",
            ProjectionArea = Area2D.Create(5, 3),
            Orientation = Orientation.Create("North"),
            Position = Coordinates.Create(10, 20, 0),
            Dimensions = Dimension.Create(2, 1, 1)
        }
    };

        mockRepository.Setup(r => r.GetAllAsync())
                      .ReturnsAsync(fakeComponents.AsEnumerable());

        var service = new ProjectorServices(mockRepository.Object);
        var mapper = new GlobalMapper();

        // Act
        var result = await GetProjectorHandler.HandleAsync(service, mapper);

        // Assert
        Assert.IsType<Ok<GetProjectorResponse>>(result.Result);

        var okResult = result.Result as Ok<GetProjectorResponse>;
        Assert.NotNull(okResult);
        Assert.NotNull(okResult.Value);

        var dto = okResult.Value.Projectors.First();
        var projectorDto = Assert.IsType<ProjectorDto>(dto);
        projectorDto.ProjectedContent.Should().Be("Sample Content");
        projectorDto.Orientation.Should().Be("North");

    }
    [Fact]
    public async Task HandleAsync_ShouldReturnOk_WithEmptyProjectorDtos_WhenServiceReturnsNoProjectors()
    {
        // Arrange
        var mockRepository = new Mock<IProjectorRepository>();
        mockRepository.Setup(r => r.GetAllAsync())
                      .ReturnsAsync(Enumerable.Empty<Projector>());
        var service = new ProjectorServices(mockRepository.Object);
        var mapper = new GlobalMapper();
        // Act
        var result = await GetProjectorHandler.HandleAsync(service, mapper);
        // Assert
        Assert.IsType<Ok<GetProjectorResponse>>(result.Result);
        var okResult = Assert.IsType<Ok<GetProjectorResponse>>(result.Result);
        okResult.Value!.Projectors.Should().BeEmpty();
    }
}
