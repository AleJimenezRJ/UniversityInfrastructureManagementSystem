using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.UniversityManagement;

public class BuildingLogDtoMappersTest
{
    /// <summary>
    /// Revisa funcionamiento correcto del método ToDto al mapear un objeto BuildingLog a un objeto BuildingLogDto.
    /// </summary>
    [Fact]
    public void ToDto_WhenGivenValidList_ShouldMapCorrectly()
    {
        var logs = new List<BuildingLog>
    {
        new BuildingLog
        {
            BuildingsLogInternalId = 1,
            Name = "Engineering",
            X = 10, Y = 20, Z = 30,
            Width = 40, Length = 50, Height = 60,
            Color = "Red",
            AreaName = "Tech",
            ModifiedAt = new DateTime(2024, 1, 1),
            Action = "Created"
        }
    };

        var dtoList = BuildingLogDtoMappers.ToDto(logs);

        dtoList.Should().HaveCount(1);
        var dto = dtoList.First();
        dto.BuildingLogInternalId.Should().Be(1);
        dto.Name.Should().Be("Engineering");
        dto.X.Should().Be(10);
        dto.Y.Should().Be(20);
        dto.Z.Should().Be(30);
        dto.Width.Should().Be(40);
        dto.Length.Should().Be(50);
        dto.Height.Should().Be(60);
        dto.Color.Should().Be("Red");
        dto.AreaName.Should().Be("Tech");
        dto.ModifiedAt.Should().Be(new DateTime(2024, 1, 1));
        dto.Action.Should().Be("Created");
    }

    /// <summary>
    /// Tests that the ToDto method correctly handles an empty list of BuildingLog entities and returns an empty list of BuildingLogDto objects.
    /// </summary>
    [Fact]
    public void ToDto_WhenGivenEmptyList_ShouldReturnEmptyList()
    {
        var emptyList = new List<BuildingLog>();


        var result = BuildingLogDtoMappers.ToDto(emptyList);

        result.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that an ArgumentNullException is thrown when a null list is passed to the ToDto method.
    /// </summary>
    [Fact]
    public void ToDto_WhenGivenNullList_ShouldThrowArgumentNullException()
    {
        Action act = () => BuildingLogDtoMappers.ToDto(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Tests that the ToDto method correctly maps multiple BuildingLog entities to their corresponding BuildingLogDto objects.
    /// </summary>
    [Fact]
    public void ToDto_WhenGivenMultipleLogs_ShouldMapAllCorrectly()
    {
        // Arrange
        var logs = new List<BuildingLog>
    {
        new BuildingLog
        {
            BuildingsLogInternalId = 1,
            Name = "Engineering",
            X = 10, Y = 20, Z = 30,
            Width = 40, Length = 50, Height = 60,
            Color = "Red",
            AreaName = "Tech",
            ModifiedAt = new DateTime(2024, 1, 1),
            Action = "Created"
        },
        new BuildingLog
        {
            BuildingsLogInternalId = 2,
            Name = "Library",
            X = 15, Y = 25, Z = 35,
            Width = 45, Length = 55, Height = 65,
            Color = "Blue",
            AreaName = "Humanities",
            ModifiedAt = new DateTime(2024, 2, 2),
            Action = "Updated"
        }
    };

        // Act
        var dtoList = BuildingLogDtoMappers.ToDto(logs);

        // Assert
        dtoList.Should().HaveCount(2);

        dtoList[0].BuildingLogInternalId.Should().Be(1);
        dtoList[0].Name.Should().Be("Engineering");
        dtoList[0].Color.Should().Be("Red");
        dtoList[0].Action.Should().Be("Created");

        dtoList[1].BuildingLogInternalId.Should().Be(2);
        dtoList[1].Name.Should().Be("Library");
        dtoList[1].Color.Should().Be("Blue");
        dtoList[1].Action.Should().Be("Updated");
    }


}