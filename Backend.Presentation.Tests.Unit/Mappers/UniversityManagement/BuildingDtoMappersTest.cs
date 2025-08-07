using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.UniversityManagement;

/// <summary>
/// Unit tests for <see cref="BuildingDtoMappers"/> class, verifying correct mapping behavior between
/// domain entities and data transfer objects related to university buildings.
/// </summary>
public class BuildingDtoMappersTests
{
    private readonly Area _testArea;

    /// <summary>
    /// Initializes test data including a test area used across tests.
    /// </summary>
    public BuildingDtoMappersTests()
    {
        var university = new University(new EntityName("UCR"), new EntityLocation("Costa Rica"));
        var campus = new Campus(new EntityName("RF"), new EntityLocation("Locate"), university);
        _testArea = new Area(new EntityName("F1"), campus);
    }

    /// <summary>
    /// Tests that a <see cref="Building"/> is correctly mapped to a <see cref="ListBuildingDto"/>.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapBuildingToListBuildingDto()
    {
        var building = CreateTestBuilding();

        var dto = BuildingDtoMappers.ToDto(building);

        dto.Should().NotBeNull();
        dto.Name.Should().Be("Engineering");
        dto.Color.Should().Be("Blue");
        dto.X.Should().Be(1.1);
        dto.Width.Should().Be(10);
        dto.Area.Name.Should().Be("F1");
    }

    /// <summary>
    /// Tests that a <see cref="Building"/> is correctly mapped to an <see cref="AddBuildingDto"/>.
    /// </summary>
    [Fact]
    public void ToSecondDto_ShouldMapBuildingToAddBuildingDto()
    {
        var building = CreateTestBuilding();

        var dto = BuildingDtoMappers.ToSecondDto(building);

        dto.Should().NotBeNull();
        dto.Name.Should().Be("Engineering");
        dto.Color.Should().Be("Blue");
        dto.Width.Should().Be(10);
        dto.Area.Name.Should().Be("F1");
    }

    /// <summary>
    /// Tests that a <see cref="ListBuildingDto"/> is correctly mapped to a <see cref="Building"/> entity.
    /// </summary>
    [Fact]
    public void ToEntity_FromListBuildingDto_ShouldReturnBuildingEntity()
    {
        var dto = new ListBuildingDto(
            Id: 1,
            Name: "Engineering",
            X: 1.1, Y: 2.2, Z: 3.3,
            Width: 10, Length: 20, Height: 30,
            Color: "Blue",
            Area: new ListAreaDto("F1", new ListCampusDto("RF", "Locate", new UniversityDto("UCR", "Costa Rica")))
        );

        var building = BuildingDtoMappers.ToEntity(dto, _testArea);

        building.Name!.Name.Should().Be("Engineering");
        building.Color!.Value.Should().Be("Blue");
    }

    /// <summary>
    /// Tests that an <see cref="AddBuildingDto"/> is correctly mapped to a <see cref="Building"/> entity.
    /// </summary>
    [Fact]
    public void ToEntity_FromAddBuildingDto_ShouldReturnBuildingEntity()
    {
        var dto = new AddBuildingDto(
            Name: "Engineering",
            X: 1.1, Y: 2.2, Z: 3.3,
            Width: 10, Length: 20, Height: 30,
            Color: "Blue",
            Area: new AddBuildingAreaDto("F1")
        );

        var building = BuildingDtoMappers.ToEntity(dto, _testArea);

        building.Name!.Name.Should().Be("Engineering");
        building.Color!.Value.Should().Be("Blue");
    }

    /// <summary>
    /// Tests that mapping an invalid <see cref="ListBuildingDto"/> throws a <see cref="ValidationException"/>.
    /// </summary>
    [Fact]
    public void ToEntity_FromListBuildingDto_ShouldThrowValidationException_WhenInvalid()
    {
        var dto = new ListBuildingDto(
            Id: 1,
            Name: "", // Invalid name
            X: -1, Y: -1, Z: -1, // Valid coordinates
            Width: -10, Length: -20, Height: -30, // Invalid dimensions
            Color: "", // Invalid color
            Area: new ListAreaDto("F1", new ListCampusDto("RF", "Locate", new UniversityDto("UCR", "Costa Rica")))
        );

        var act = () => BuildingDtoMappers.ToEntity(dto, _testArea);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain(e => e.Parameter == "Name")
                              .And.Contain(e => e.Parameter == "Dimensions")
                              .And.Contain(e => e.Parameter == "Color");
    }

    /// <summary>
    /// Tests that mapping an invalid <see cref="AddBuildingDto"/> throws a <see cref="ValidationException"/>.
    /// </summary>
    [Fact]
    public void ToEntity_FromAddBuildingDto_ShouldThrowValidationException_WhenInvalid()
    {
        var dto = new AddBuildingDto(
            Name: "", // Invalid name
            X: -1, Y: -1, Z: -1, // Valid coordinates
            Width: -10, Length: -20, Height: -30, // Invalid dimensions
            Color: "", // Invalid color
            Area: new AddBuildingAreaDto("F1")
        );

        var act = () => BuildingDtoMappers.ToEntity(dto, _testArea);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain(e => e.Parameter == "Name")
                              .And.Contain(e => e.Parameter == "Dimensions")
                              .And.Contain(e => e.Parameter == "Color");
    }

    /// <summary>
    /// Creates a valid test instance of a <see cref="Building"/> entity.
    /// </summary>
    /// <returns>A new <see cref="Building"/> instance with predefined values.</returns>
    private static Building CreateTestBuilding()
    {
        var name = new EntityName("Engineering");
        var coords = new Coordinates(1.1, 2.2, 3.3);
        var dimensions = new Dimension(10, 20, 30);
        var color = new Colors("Blue");

        var university = new University(new EntityName("UCR"), new EntityLocation("Costa Rica"));
        var campus = new Campus(new EntityName("RF"), new EntityLocation("Locate"), university);
        var area = new Area(new EntityName("F1"), campus);

        return new Building(1, name, coords, dimensions, color, area);
    }
}
