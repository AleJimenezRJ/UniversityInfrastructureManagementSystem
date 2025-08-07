using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.Spaces;

/// <summary>
/// Tests for the <see cref="FloorDtoMapper"/> class, ensuring it correctly maps between Floor entities and Floor DTOs.
/// </summary>
public class FloorDtoMapperTests
{
    /// <summary>
    /// Valid Floor entity used for testing mapping to DTO.
    /// </summary>
    private readonly Floor _validFloor;

    /// <summary>
    /// Valid Floor DTO used for testing mapping to entity.
    /// </summary>
    private readonly FloorDto _validFloorDto;

    /// <summary>
    /// Represents an invalid floor with a zero value.
    /// </summary>
    private readonly FloorDto _invalidFloorZero;

    /// <summary>
    /// Represents an invalid floor with a negative value.
    /// </summary>
    private readonly FloorDto _invalidFloorNegative;

    /// <summary>
    /// Constructor for the FloorDtoMapperTests class, initializing valid and invalid Floor entities and DTOs.
    /// </summary>
    public FloorDtoMapperTests()
    {
        _validFloor = new Floor(1, FloorNumber.Create(1));
        _validFloorDto = new FloorDto(1, 1);
        _invalidFloorZero = new FloorDto(1, 0); 
        _invalidFloorNegative = new FloorDto(1, -1);
    }

    /// <summary>
    /// Tests that the <see cref="FloorDtoMapper.ToDto"/> method returns a DTO with the correct FloorId when called with
    /// a valid entity.
    /// </summary>
    [Fact]
    public void ToDto_WhenCalledWithValidEntity_ReturnsCorrectDtoFlooId()
    {
        // Act
        var dto = FloorDtoMapper.ToDto(_validFloor);

        // Assert
        dto.FloorId.Should().Be(_validFloor.FloorId, because: "the FloorId should match the entity's FloorId");
    }

    /// <summary>
    /// Tests that the <see cref="FloorDtoMapper.ToDto"/> method returns a DTO with the correct FloorNumber when
    /// </summary>
    [Fact]
    public void ToDto_WhenCalledWithValidEntity_ReturnsCorrectDtoFloorNumber()
    {
        // Act
        var dto = FloorDtoMapper.ToDto(_validFloor);

        // Assert
        dto.FloorNumber.Should().Be(_validFloor.Number.Value, because: "the FloorNumber should match the entity's FloorNumber value");
    }

    /// <summary>
    /// Tests that the <see cref="FloorDtoMapper.ToEntity"/> method returns a <c>Floor</c> entity with the correct floor
    /// number when called with a valid DTO.
    /// </summary>
    [Fact]
    public void ToEntity_WhenCalledWithValidDto_ReturnsCorrectFloorWithFloorNumber()
    {
        // Act
        var floorEntity = FloorDtoMapper.ToEntity(_validFloorDto);

        // Assert
        floorEntity.Number.Value.Should().Be(_validFloorDto.FloorNumber, because: "the FloorNumber in the entity should match the DTO's FloorNumber");
    }

    /// <summary>
    /// Tests that the <see cref="FloorDtoMapper.ToEntity"/> method throws a <see cref="ValidationException"/> when
    /// called with a DTO having a floor number of zero.
    /// </summary>
    [Fact]
    public void ToEntity_WhenCalledWithDtoFloorNumberZero_ThrowsValidationException()
    {
        // Act
        Action act = () => FloorDtoMapper.ToEntity(_invalidFloorZero);

        // Assert
        act.Should().Throw<ValidationException>()
        .Where(e => e.Errors.Any(err => err.Parameter == "FloorNumber" &&
                                        err.Message.Contains("The FloorNumber must be an integer greater than zero.")));
    }

    /// <summary>
    /// Tests that the <see cref="FloorDtoMapper.ToEntity"/> method throws a <see cref="ValidationException"/> when
    /// called with a DTO containing a negative floor number.
    /// </summary>
    [Fact]
    public void ToEntity_WhenCalledWithDtoFloorNumberNegative_ThrowsValidationException()
    {
        // Act
        Action act = () => FloorDtoMapper.ToEntity(_invalidFloorNegative);

        // Assert
        act.Should().Throw<ValidationException>()
        .Where(e => e.Errors.Any(err => err.Parameter == "FloorNumber" &&
                                        err.Message.Contains("The FloorNumber must be an integer greater than zero.")));
    }
}
