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

/// <summary>
/// Unit tests for <see cref="PostBuildingHandler"/>.
/// These tests validate the behavior of the building creation handler with various input scenarios,
/// including valid and invalid data, conflict situations, and successful creation.
/// </summary>
public class PostBuildingHandlerTests
{
    private readonly Mock<IBuildingsServices> _buildingServiceMock;
    private readonly Mock<IAreaServices> _areaServiceMock;

    private readonly AddBuildingDto _validDto;
    private readonly AddBuildingDto _invalidDto;

    /// <summary>
    /// Initializes mock services and test data for valid and invalid building creation DTOs.
    /// </summary>
    public PostBuildingHandlerTests()
    {
        _buildingServiceMock = new Mock<IBuildingsServices>(MockBehavior.Strict);
        _areaServiceMock = new Mock<IAreaServices>(MockBehavior.Strict);

        _validDto = new AddBuildingDto(
            Name: "Engineering Building",
            X: 10, Y: 20, Z: 0,
            Width: 30, Length: 40, Height: 15,
            Color: "Red",
            Area: new AddBuildingAreaDto("Main Campus")
        );

        _invalidDto = new AddBuildingDto(
            Name: "",
            X: -10, Y: -20, Z: 0,
            Width: -30, Length: -40, Height: -15,
            Color: "",
            Area: new AddBuildingAreaDto("InvalidArea")
        );
    }

    /// <summary>
    /// Tests that the handler returns validation errors when the input DTO is invalid.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenDtoIsInvalid_ShouldReturnValidationErrors()
    {
        var university = new University(new EntityName("U"));
        var campus = new Campus(new EntityName("C"), new EntityLocation("Loc"), university);
        var validArea = new Area(new EntityName("A"), campus);

        _areaServiceMock.Setup(a => a.GetByNameAsync(_invalidDto.Area.Name))
            .ReturnsAsync(validArea);

        var result = await PostBuildingHandler.HandleAsync(
            _buildingServiceMock.Object, _areaServiceMock.Object, _invalidDto);

        var badRequest = Assert.IsType<BadRequest<List<ValidationError>>>(result.Result);
        badRequest.Value.Should().Contain(e => e.Parameter == "Name" || e.Parameter == "Dimensions");
    }

    /// <summary>
    /// Tests that the handler returns a conflict result when the building already exists.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenBuildingAlreadyExists_ShouldReturnConflict()
    {
        var university = new University(new EntityName("U"));
        var campus = new Campus(new EntityName("C"), new EntityLocation("Loc"), university);
        var validArea = new Area(new EntityName("A"), campus);

        _areaServiceMock.Setup(a => a.GetByNameAsync(_validDto.Area.Name))
            .ReturnsAsync(validArea);

        _buildingServiceMock.Setup(b => b.AddBuildingAsync(It.IsAny<Building>()))
            .ThrowsAsync(new DuplicatedEntityException("Building already exists."));

        var result = await PostBuildingHandler.HandleAsync(
            _buildingServiceMock.Object, _areaServiceMock.Object, _validDto);

        var conflict = Assert.IsType<Conflict<ErrorResponse>>(result.Result);
        conflict.Value!.ErrorMessages.Should().Contain("Building already exists.");
    }

    /// <summary>
    /// Tests that the handler returns a conflict result when adding the building fails unexpectedly.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenAddFails_ShouldReturnConflict()
    {
        var university = new University(new EntityName("U"));
        var campus = new Campus(new EntityName("C"), new EntityLocation("Loc"), university);
        var validArea = new Area(new EntityName("A"), campus);

        _areaServiceMock.Setup(a => a.GetByNameAsync(_validDto.Area.Name))
            .ReturnsAsync(validArea);

        _buildingServiceMock.Setup(b => b.AddBuildingAsync(It.IsAny<Building>()))
            .ReturnsAsync(false);

        var result = await PostBuildingHandler.HandleAsync(
            _buildingServiceMock.Object, _areaServiceMock.Object, _validDto);

        var conflict = Assert.IsType<Conflict<ErrorResponse>>(result.Result);
        conflict.Value!.ErrorMessages.Should().Contain("Building could not be added.");
    }

    /// <summary>
    /// Tests that the handler returns an OK result when the building is successfully created.
    /// </summary>
    [Fact]
    public async Task HandleAsync_WhenSuccessful_ShouldReturnOkWithResponse()
    {
        var university = new University(new EntityName("U"));
        var campus = new Campus(new EntityName("C"), new EntityLocation("Loc"), university);
        var validArea = new Area(new EntityName("A"), campus);

        _areaServiceMock.Setup(a => a.GetByNameAsync(_validDto.Area.Name))
            .ReturnsAsync(validArea);

        _buildingServiceMock.Setup(b => b.AddBuildingAsync(It.IsAny<Building>()))
            .ReturnsAsync(true);

        var result = await PostBuildingHandler.HandleAsync(
            _buildingServiceMock.Object, _areaServiceMock.Object, _validDto);

        var ok = Assert.IsType<Ok<PostBuildingResponse>>(result.Result);
        ok.Value.Should().NotBeNull();
        ok.Value.Building.Name.Should().Be(_validDto.Name);
    }
}
