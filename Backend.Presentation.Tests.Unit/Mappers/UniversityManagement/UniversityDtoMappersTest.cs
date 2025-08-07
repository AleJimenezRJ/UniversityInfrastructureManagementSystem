using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.UniversityManagement;

/// <summary>
/// Unit tests for the <see cref="UniversityDtoMappers"/> class,
/// which handles transformations between <see cref="University"/> domain entities and DTOs.
/// </summary>
public class UniversityDtoMappersTests
{
    /// <summary>
    /// Verifies that a <see cref="University"/> is correctly mapped to a <see cref="UniversityDto"/>.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapUniversityToUniversityDto()
    {
        // Arrange
        var university = new University(
            new EntityName("UCR"),
            new EntityLocation("Costa Rica")
        );

        // Act
        var dto = UniversityDtoMappers.ToDto(university);

        // Assert
        dto.Name.Should().Be("UCR");
        dto.Country.Should().Be("Costa Rica");
    }

    /// <summary>
    /// Verifies that a <see cref="University"/> is correctly mapped to an <see cref="AddCampusUniversityDto"/>.
    /// </summary>
    [Fact]
    public void ToSecondDto_ShouldMapUniversityToAddCampusUniversityDto()
    {
        // Arrange
        var university = new University(
            new EntityName("UCR"),
            new EntityLocation("Costa Rica")
        );

        // Act
        var dto = UniversityDtoMappers.ToSecondDto(university);

        // Assert
        dto.Name.Should().Be("UCR");
    }

    /// <summary>
    /// Verifies that an <see cref="AddCampusUniversityDto"/> is correctly mapped to a <see cref="University"/> entity.
    /// </summary>
    [Fact]
    public void ToEntity_FromAddCampusUniversityDto_ShouldMapToUniversityEntity()
    {
        // Arrange
        var dto = new AddCampusUniversityDto("UCR");

        // Act
        var entity = UniversityDtoMappers.ToEntity(dto);

        // Assert
        entity.Name.Name.Should().Be("UCR");
        entity.Country.Should().BeNull(); // Country is not included in AddCampusUniversityDto
    }

    /// <summary>
    /// Verifies that a <see cref="UniversityDto"/> is correctly mapped to a <see cref="University"/> entity.
    /// </summary>
    [Fact]
    public void ToEntity_FromUniversityDto_ShouldMapToUniversityEntity()
    {
        // Arrange
        var dto = new UniversityDto("UCR", "Costa Rica");

        // Act
        var entity = UniversityDtoMappers.ToEntity(dto);

        // Assert
        entity.Name.Name.Should().Be("UCR");
        entity.Country.Location.Should().Be("Costa Rica");
    }
}
