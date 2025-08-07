using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.UniversityManagement;

/// <summary>
/// Unit tests for <see cref="AreaDtoMappers"/> class, verifying correct mapping behavior between
/// domain entities and data transfer objects related to university areas.
/// </summary>


/// PBI: #269 - Pruebas unitarias para backend <see href="https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/269"/>
/// Tareas implementadas:
/// Review backend components and responsibilities 	
/// Define test cases for each method and controller logic 	
/// Write unit tests for key services and API handlers 
/// Add assertion logic for all expected outcomes 
/// Run tests and ensure all pass

public class AreaDtoMappersTests
{
    private readonly Campus _testCampus;

    /// <summary>
    /// Initializes test data including a test area used across tests.
    /// </summary>
    public AreaDtoMappersTests()
    {
        var university = new University(new EntityName("UCR"), new EntityLocation("Costa Rica"));
        _testCampus = new Campus(new EntityName("Campus"), new EntityLocation("Loc"), university);
    }

    /// <summary>
    /// Tests that a <see cref="Area"/> is correctly mapped to a <see cref="AreaDto"/>.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapAreaToListAreaDto()
    {
        var area = CreateTestArea();

        var dto = AreaDtoMappers.ToDto(area);

        dto.Should().NotBeNull();
        dto.Name.Should().Be("Finca1");
        dto.Campus.Name.Should().Be("Campus");
    }

    /// <summary>
    /// Tests that a <see cref="Area"/> is correctly mapped to an <see cref="AddBuildingAreaDto"/>.
    /// </summary>
    [Fact]
    public void ToSecondDto_ShouldMapAreaToAddBuildingAreaDto()
    {
        var area = CreateTestArea();

        var dto = AreaDtoMappers.ToSecondDto(area);

        dto.Should().NotBeNull();
        dto.Name.Should().Be("Finca1");
    }

    /// <summary>
    /// Tests that a <see cref="Area"/> is correctly mapped to an <see cref="AddAreaDto"/>.
    /// </summary>
    [Fact]
    public void ToAddDto_ShouldMapAreaToAddAreaDto()
    {
        var area = CreateTestArea();

        var dto = AreaDtoMappers.ToAddDto(area);

        dto.Should().NotBeNull();
        dto.Name.Should().Be("Finca1");
        dto.Campus.Name.Should().Be("Campus");
    }

    /// <summary>
    /// Tests that a <see cref="ListAreaDto"/> is correctly mapped to a <see cref="Area"/> entity.
    /// </summary>
    /// 
    [Fact]
    public void ToEntity_FromListAreaDto_ShouldReturnAreaEntity()
    {
        var dto = new ListAreaDto(
            Name: "Finca1",
            Campus: new ListCampusDto("Campus", "Loc", new UniversityDto("UCR", "Costa Rica"))
        );

        var area = AreaDtoMappers.ToEntity(dto, _testCampus);

        area.Name!.Name.Should().Be("Finca1");
        area.Campus.Should().Be(_testCampus);
    }

    /// <summary>
    /// Tests that an <see cref="AddAreaDto"/> is correctly mapped to a <see cref="Area"/> entity.
    /// </summary>
    [Fact]
    public void ToEntity_FromAddAreaDto_ShouldReturnAreaEntity()
    {
        var dto = new AddAreaDto(
            Name: "Finca1",
            Campus: new AddAreaCampusDto("Campus")
        );

        var area = AreaDtoMappers.ToEntity(dto, _testCampus);

        area.Name!.Name.Should().Be("Finca1");
        area.Campus.Should().Be(_testCampus);
    }

    /// <summary>
    /// Tests that mapping an invalid <see cref="ListAreaDto"/> throws a <see cref="ValidationException"/>.
    /// </summary>
    [Fact]
    public void ToEntity_FromListAreaDto_ShouldThrowValidationException_WhenInvalid()
    {
        var dto = new ListAreaDto(
            Name: "", // Invalid name
            Campus: new ListCampusDto("Campus", "Loc", new UniversityDto("UCR", "Costa Rica"))
        );

        var act = () => AreaDtoMappers.ToEntity(dto, _testCampus);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain(e => e.Message == "The name must be non-empty and no more than 100 characters long.");
    }

    /// <summary>
    /// Tests that mapping an invalid <see cref="AddAreaDto"/> throws a <see cref="ValidationException"/>.
    /// </summary>
    [Fact]
    public void ToEntity_FromAddAreaDto_ShouldThrowValidationException_WhenInvalid()
    {
        var dto = new AddAreaDto(
            Name: "", // Invalid name
            Campus: new AddAreaCampusDto("Campus")
        );

        var act = () => AreaDtoMappers.ToEntity(dto, _testCampus);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain(e => e.Message == "The name must be non-empty and no more than 100 characters long.");
    }

    /// <summary>
    /// Creates a valid test instance of a <see cref="Area"/> entity.
    /// </summary>
    /// <returns>A new <see cref="Area"/> instance with predefined values.</returns>
    private static Area CreateTestArea()
    {
        var name = new EntityName("Finca1");
     
        var university = new University(new EntityName("UCR"), new EntityLocation("Costa Rica"));
        var campus = new Campus(new EntityName("Campus"), new EntityLocation("Locate"), university);

        return new Area(name, campus);
    }
}
