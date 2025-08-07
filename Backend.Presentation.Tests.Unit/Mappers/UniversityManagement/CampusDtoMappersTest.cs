using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.UniversityManagement;

/// <summary>
/// Unit tests for <see cref="CampusDtoMappers"/> class, verifying correct mapping behavior between
/// domain entities and data transfer objects related to university campuses.
/// </summary>

public class CampusDtoMappersTests
{
    private readonly University _testUniversity;
    private readonly AddCampusUniversityDto _testUniversityAddDto;
    private readonly UniversityDto _testUniversityDto;
    /// <summary>
    /// Initializes test data including a test university used across tests.
    /// </summary>
    public CampusDtoMappersTests()
    {
        _testUniversity = new University(new EntityName("UCR"), new EntityLocation("Costa Rica"));
        _testUniversityAddDto = new AddCampusUniversityDto("UCR");
        _testUniversityDto = new UniversityDto("UCR", "Costa Rica");
    }
    /// <summary>
    /// Tests that a <see cref="Campus"/> is correctly mapped to a <see cref="ListCampusDto"/>.
    /// </summary>
    [Fact]
    public void ToDto_WhenEntityIsCorrect_ShouldMapCampusToListCampusDto()
    {
        var campus = CreateTestCampus();
        var dto = CampusDtoMappers.ToDto(campus);
        dto.Should().NotBeNull();
        dto.Name.Should().Be("RF");
        dto.Location.Should().Be("Locate");
        dto.University.Name.Should().Be("UCR");
    }
    /// <summary>
    /// Tests that an <see cref="AddCampusDto"/> is correctly mapped to a <see cref="Campus"/> entity.
    /// </summary>
    [Fact]
    public void ToEntity_WhenDtoIsCorrect_ShouldMapAddCampusDtoToCampus()
    {
        var addCampusDto = new AddCampusDto("RF", "Locate", _testUniversityAddDto);
        var campus = CampusDtoMappers.ToEntity(addCampusDto, _testUniversity);
        campus.Should().NotBeNull();
        campus.Name.Name.Should().Be("RF");
        campus.Location.Location.Should().Be("Locate");
        campus.University.Should().Be(_testUniversity);
    }

    /// <summary>
    /// Tests that a <see cref="ListCampusDto"/> is correctly mapped to a <see cref="Campus"/> entity.
    /// </summary>
    [Fact]
    public void ToEntity_FromListCampusDto_ShouldReturnCampus()
    {
        // Arrange
        var dto = new ListCampusDto("Occidente", "San Ramón", _testUniversityDto);
        var university = _testUniversity;

        // Act
        var campus = CampusDtoMappers.ToEntity(dto, university);

        // Assert
        campus.Name.Name.Should().Be("Occidente");
        campus.Location.Location.Should().Be("San Ramón");
        campus.University.Should().Be(university);
    }

    /// <summary>
    /// Tests that <see cref="CampusDtoMappers.ToDto"/> throws an <see cref="NullReferenceException"/>
    /// </summary>
    [Fact]
    public void ToDto_WhenCampusIsNull_ShouldThrowNullReferenceException()
    {
        // Act
        Action act = () => CampusDtoMappers.ToDto(null!);

        // Assert
        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void ToEntity_FromAddCampusDto_WithNullName_ShouldThrowDomainValidationException()
    {
        // Arrange
        var invalidDto = new AddCampusDto(null!, "Locate", _testUniversityAddDto);

        // Act
        Action act = () => CampusDtoMappers.ToEntity(invalidDto, _testUniversity);

        // Assert
        act.Should().Throw<ValidationException>();
    }

    [Fact]
    public void ToEntity_FromListCampusDto_WithEmptyLocation_ShouldThrowDomainValidationException()
    {
        // Arrange
        var dto = new ListCampusDto("Occidente", "", _testUniversityDto);

        // Act
        Action act = () => CampusDtoMappers.ToEntity(dto, _testUniversity);

        // Assert
        act.Should().Throw<ValidationException>();
    }

    private Campus CreateTestCampus()
    {
        return new Campus(new EntityName("RF"), new EntityLocation("Locate"), _testUniversity);
    }
}