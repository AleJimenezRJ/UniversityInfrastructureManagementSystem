using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Handlers.UniversityManagement;

public class PostAreaHandlerTests
{
    private readonly Mock<IAreaServices> _areaServiceMock;
    private readonly Mock<ICampusServices> _campusServiceMock;

    private readonly AddAreaDto _validDto;
    private readonly AddAreaDto _invalidDto;

    public PostAreaHandlerTests()
    {
        _areaServiceMock = new Mock<IAreaServices>(MockBehavior.Strict);
        _campusServiceMock = new Mock<ICampusServices>(MockBehavior.Strict);

        _validDto = new AddAreaDto(
            Name: "Engineering Zone",
            Campus: new AddAreaCampusDto("Main Campus")
        );

        _invalidDto = new AddAreaDto(
            Name: "",  // Invalid name
            Campus: new AddAreaCampusDto("Main Campus")
        );
    }

    [Fact]
    public async Task HandleAsync_WhenCampusDoesNotExist_ShouldReturnBadRequest()
    {
        _campusServiceMock.Setup(x => x.GetByNameAsync(_validDto.Campus.Name))
            .ReturnsAsync((Campus?)null);

        var result = await PostAreaHandler.HandleAsync(
            _areaServiceMock.Object,
            _campusServiceMock.Object,
            _validDto);

        var badRequest = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        badRequest.Value!.ErrorMessages.Should().Contain("The specified campus does not exist.");
    }

    [Fact]
    public async Task HandleAsync_WhenDtoIsInvalid_ShouldReturnValidationError()
    {
        var university = new University(new EntityName("UCR"));
        var campus = new Campus(new EntityName("Main Campus"), new EntityLocation("Location"), university);

        _campusServiceMock.Setup(x => x.GetByNameAsync(_invalidDto.Campus.Name))
            .ReturnsAsync(campus);

        var result = await PostAreaHandler.HandleAsync(
            _areaServiceMock.Object,
            _campusServiceMock.Object,
            _invalidDto);

        var badRequest = Assert.IsType<BadRequest<ErrorResponse>>(result.Result);
        badRequest.Value!.ErrorMessages.Should().NotBeEmpty();
    }

    [Fact]
    public async Task HandleAsync_WhenAdditionFails_ShouldReturnConflict()
    {
        var university = new University(new EntityName("UCR"));
        var campus = new Campus(new EntityName("Main Campus"), new EntityLocation("Location"), university);

        _campusServiceMock.Setup(x => x.GetByNameAsync(_validDto.Campus.Name))
            .ReturnsAsync(campus);

        _areaServiceMock.Setup(x => x.AddAreaAsync(It.IsAny<Area>()))
            .ReturnsAsync(false);

        var result = await PostAreaHandler.HandleAsync(
            _areaServiceMock.Object,
            _campusServiceMock.Object,
            _validDto);

        var conflict = Assert.IsType<Conflict<ErrorResponse>>(result.Result);
        conflict.Value!.ErrorMessages.Should().Contain("Area could not be added.");
    }

    [Fact]
    public async Task HandleAsync_WhenAreaAlreadyExists_ShouldReturnConflict()
    {
        var university = new University(new EntityName("UCR"));
        var campus = new Campus(new EntityName("Main Campus"), new EntityLocation("Location"), university);

        _campusServiceMock.Setup(x => x.GetByNameAsync(_validDto.Campus.Name))
            .ReturnsAsync(campus);

        _areaServiceMock.Setup(x => x.AddAreaAsync(It.IsAny<Area>()))
            .ThrowsAsync(new DuplicatedEntityException("Area already exists."));

        var result = await PostAreaHandler.HandleAsync(
            _areaServiceMock.Object,
            _campusServiceMock.Object,
            _validDto);

        var conflict = Assert.IsType<Conflict<ErrorResponse>>(result.Result);
        conflict.Value!.ErrorMessages.Should().Contain("Area already exists.");
    }

    [Fact]
    public async Task HandleAsync_WhenSuccessful_ShouldReturnOk()
    {
        var university = new University(new EntityName("UCR"));
        var campus = new Campus(new EntityName("Main Campus"), new EntityLocation("Location"), university);

        _campusServiceMock.Setup(x => x.GetByNameAsync(_validDto.Campus.Name))
            .ReturnsAsync(campus);

        _areaServiceMock.Setup(x => x.AddAreaAsync(It.IsAny<Area>()))
            .ReturnsAsync(true);

        var result = await PostAreaHandler.HandleAsync(
            _areaServiceMock.Object,
            _campusServiceMock.Object,
            _validDto);

        var ok = Assert.IsType<Ok<PostAreaResponse>>(result.Result);
        ok.Value!.Area.Name.Should().Be("Engineering Zone");
        ok.Value.Area.Campus.Name.Should().Be("Main Campus");
    }
}
