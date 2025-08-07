using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.Spaces;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.Spaces;

/// <summary>
/// Unit tests for <see cref="LearningSpaceLogDtoMappers"/>.
/// </summary>
public class LearningSpaceLogDtoMapperTests
{
    private static readonly LearningSpaceLog _exampleLog;
    private static readonly LearningSpaceLog _exampleLog2;

    static LearningSpaceLogDtoMapperTests()
    {
        _exampleLog = new LearningSpaceLog
        {
            LearningSpaceLogInternalId = 1,
            Name = "Aula Magna",
            Type = "Auditorium",
            MaxCapacity = 100,
            Width = 10.5m,
            Height = 4.2m,
            Length = 15.0m,
            ColorFloor = "Red",
            ColorWalls = "White",
            ColorCeiling = "Gray",
            ModifiedAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
            Action = "Created"
        };
        _exampleLog2 = new LearningSpaceLog
        {
            LearningSpaceLogInternalId = 2,
            Name = "Lab 202",
            Type = "Laboratory",
            MaxCapacity = 25,
            Width = 7.5m,
            Height = 3.5m,
            Length = 9.0m,
            ColorFloor = "Blue",
            ColorWalls = "Gray",
            ColorCeiling = "White",
            ModifiedAt = new DateTime(2024, 1, 2, 14, 0, 0, DateTimeKind.Utc),
            Action = "Updated"
        };
    }

    /// <summary>
    /// Ensures that <see cref="LearningSpaceLogDtoMappers.ToDto"/> correctly maps all properties from a domain entity to a DTO.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapLearningSpaceLogToDtoCorrectly()
    {
        var dtoList = LearningSpaceLogDtoMappers.ToDto(new List<LearningSpaceLog> { _exampleLog });
        dtoList.Should().HaveCount(1);
        var dto = dtoList[0];
        dto.LearningSpaceLogInternalId.Should().Be(_exampleLog.LearningSpaceLogInternalId);
        dto.Name.Should().Be(_exampleLog.Name);
        dto.Type.Should().Be(_exampleLog.Type);
        dto.MaxCapacity.Should().Be(_exampleLog.MaxCapacity);
        dto.Width.Should().Be(_exampleLog.Width);
        dto.Height.Should().Be(_exampleLog.Height);
        dto.Length.Should().Be(_exampleLog.Length);
        dto.ColorFloor.Should().Be(_exampleLog.ColorFloor);
        dto.ColorWalls.Should().Be(_exampleLog.ColorWalls);
        dto.ColorCeiling.Should().Be(_exampleLog.ColorCeiling);
        dto.ModifiedAt.Should().Be(_exampleLog.ModifiedAt);
        dto.Action.Should().Be(_exampleLog.Action);
    }

    /// <summary>
    /// Ensures that <see cref="LearningSpaceLogDtoMappers.ToDto"/> correctly maps a list of logs.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapMultipleLearningSpaceLogsToDtoList()
    {
        var dtoList = LearningSpaceLogDtoMappers.ToDto(new List<LearningSpaceLog> { _exampleLog, _exampleLog2 });
        dtoList.Should().HaveCount(2);
        dtoList[0].Name.Should().Be(_exampleLog.Name);
        dtoList[1].Name.Should().Be(_exampleLog2.Name);
    }

    /// <summary>
    /// Ensures that <see cref="LearningSpaceLogDtoMappers.ToDto"/> returns an empty list when input is empty.
    /// </summary>
    [Fact]
    public void ToDto_ShouldReturnEmptyList_WhenInputIsEmpty()
    {
        var dtoList = LearningSpaceLogDtoMappers.ToDto(new List<LearningSpaceLog>());
        dtoList.Should().BeEmpty();
    }
}
