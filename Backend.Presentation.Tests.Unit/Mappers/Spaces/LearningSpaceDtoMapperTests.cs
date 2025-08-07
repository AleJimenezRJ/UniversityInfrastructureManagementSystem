using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.Spaces;
/// <summary>
/// Unit tests for <see cref="LearningSpaceDtoMapper"/>.
/// </summary>
public class LearningSpaceDtoMapperTests
{
    private static readonly LearningSpace _exampleSpace;

    /// <summary>
    /// Static constructor to initialize an example <see cref="LearningSpace"/> instance
    /// used in multiple test cases.
    /// </summary>
    static LearningSpaceDtoMapperTests()
    {
        _exampleSpace = new LearningSpace(
            EntityName.Create("Physics Lab"),
            LearningSpaceType.Create("Laboratory"),
            Capacity.Create(30),
            Size.Create(90),
            Size.Create(100),
            Size.Create(80),
            Colors.Create("Blue"),
            Colors.Create("Gray"),
            Colors.Create("White")
        );
    }

    /// <summary>
    /// Ensures that <see cref="LearningSpaceDtoMapper.ToDto"/> correctly maps all properties from a domain entity to a full DTO.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapLearningSpaceToDtoCorrectly()
    {
        var dto = LearningSpaceDtoMapper.ToDto(_exampleSpace);

        dto.Name.Should().Be("Physics Lab");
        dto.Type.Should().Be("Laboratory");
        dto.MaxCapacity.Should().Be(30);
        dto.Height.Should().Be(90);
        dto.Width.Should().Be(100);
        dto.Length.Should().Be(80);
        dto.ColorFloor.Should().Be("Blue");
        dto.ColorWalls.Should().Be("Gray");
        dto.ColorCeiling.Should().Be("White");
    }

    /// <summary>
    /// Ensures that <see cref="LearningSpaceDtoMapper.ToDtoList"/> maps only the required subset of properties to a list DTO.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapLearningSpaceToListDtoCorrectly()
    {
        var dto = LearningSpaceDtoMapper.ToDtoList(_exampleSpace);

        dto.LearningSpaceId.Should().Be(_exampleSpace.LearningSpaceId);
        dto.Name.Should().Be("Physics Lab");
        dto.Type.Should().Be("Laboratory");
    }

    /// <summary>
    /// Validates that <see cref="LearningSpaceDtoMapper.ToEntity"/> creates a valid domain entity from a well-formed DTO.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldReturnValidLearningSpaceEntity_WhenDtoIsValid()
    {
        var dto = new LearningSpaceDto(
            Name: "Physics Lab",
            Type: "Laboratory",
            MaxCapacity: 30,
            Height: 90,
            Width: 100,
            Length: 80,
            ColorFloor: "Blue",
            ColorWalls: "Gray",
            ColorCeiling: "White"
        );

        var entity = LearningSpaceDtoMapper.ToEntity(dto);

        entity.Name.Name.Should().Be("Physics Lab");
        entity.Type.Value.Should().Be("Laboratory");
        entity.MaxCapacity.Value.Should().Be(30);
        entity.Height.Value.Should().Be(90);
        entity.Width.Value.Should().Be(100);
        entity.Length.Value.Should().Be(80);
        entity.ColorFloor.Value.Should().Be("Blue");
        entity.ColorWalls.Value.Should().Be("Gray");
        entity.ColorCeiling.Value.Should().Be("White");
    }

    /// <summary>
    /// Verifies that <see cref="LearningSpaceDtoMapper.ToEntity"/> throws a <see cref="ValidationException"/> 
    /// and reports all invalid fields when the DTO contains invalid input.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldThrowValidationException_WhenDtoIsInvalid()
    {
        var dto = new LearningSpaceDto(
            Name: "",
            Type: "INVALID",
            MaxCapacity: -5,
            Height: -1,
            Width: -1,
            Length: -1,
            ColorFloor: "",
            ColorWalls: "",
            ColorCeiling: ""
        );

        var act = () => LearningSpaceDtoMapper.ToEntity(dto);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain(e => e.Parameter == "Name")
                              .And.Contain(e => e.Parameter == "Type")
                              .And.Contain(e => e.Parameter == "MaxCapacity")
                              .And.Contain(e => e.Parameter == "Height")
                              .And.Contain(e => e.Parameter == "Width")
                              .And.Contain(e => e.Parameter == "Length")
                              .And.Contain(e => e.Parameter == "ColorFloor")
                              .And.Contain(e => e.Parameter == "ColorWalls")
                              .And.Contain(e => e.Parameter == "ColorCeiling");
    }

    /// <summary>
    /// Checks that <see cref="LearningSpaceDtoMapper.ToEntity"/> accepts the minimum valid values for each field.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldAcceptMinimumValidValues()
    {
        var dto = new LearningSpaceDto(
            Name: "A",
            Type: "Classroom",
            MaxCapacity: 1,
            Height: 1,
            Width: 1,
            Length: 1,
            ColorFloor: "Red",
            ColorWalls: "Green",
            ColorCeiling: "Blue"
        );

        var entity = LearningSpaceDtoMapper.ToEntity(dto);

        entity.Name.Name.Should().Be("A");
        entity.MaxCapacity.Value.Should().Be(1);
        entity.Height.Value.Should().Be(1);
    }

    /// <summary>
    /// Verifies that all validation errors are reported when multiple invalid fields exist in the DTO.
    /// </summary>
    [Fact]
    public void ToEntity_ShouldAggregateAllValidationErrors()
    {
        var dto = new LearningSpaceDto("", "INVALID", -1, -1, -1, -1, "", "", "");

        var act = () => LearningSpaceDtoMapper.ToEntity(dto);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().HaveCount(9);
    }

    /// <summary>
    /// Performs a round-trip conversion from entity to DTO and back,
    /// verifying that the resulting entity retains original values.
    /// </summary>
    [Fact]
    public void EntityDtoRoundTrip_ShouldPreserveData()
    {
        var dto = LearningSpaceDtoMapper.ToDto(_exampleSpace);
        var entity = LearningSpaceDtoMapper.ToEntity(dto);

        entity.Name.Name.Should().Be(_exampleSpace.Name.Name);
        entity.Type.Value.Should().Be(_exampleSpace.Type.Value);
        entity.MaxCapacity.Value.Should().Be(_exampleSpace.MaxCapacity.Value);
    }
}
