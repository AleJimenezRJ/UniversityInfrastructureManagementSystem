using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.Spaces;

/*
Story: This task corresponds to the epic EPIC ID: SQL-FL-001 and is associated with PBIs #109, #110 and #111.

Technical Tasks Performed:
- Implementation of unit tests
- Follow Clean Code Principles
- Commented using XML style
- Handle Input Parameters and Validation

Participants:
    - Anderson Vargas
    - Keylor Palacios
*/

/// <summary>
/// Provides unit tests for the <see cref="Floor"/> class, ensuring its constructors and methods behave as expected
/// under various conditions.
/// </summary>
public class FloorTests
{
    /// <summary>
    /// /// Represents the floor number used in the tests.
    /// </summary>
    private FloorNumber _number;

    /// <summary>
    /// Represents the floor associated with the current context.
    /// </summary>
    private Floor _floor;

    /// <summary>
    /// Initializes a new instance of the <see cref="FloorTests"/> class.
    /// </summary>
    public FloorTests()
    {
        _number = FloorNumber.Create(1);

        _floor = new Floor(_number);
    }

    /// <summary>
    /// Tests that the <see cref="Floor"/> constructor correctly assigns the provided number to the <see
    /// cref="Floor.Number"/> property when a valid argument is supplied.
    /// </summary>
    [Fact]
    public void Constructor_SingleParameter_WithValidArguments_AssignNumberCorrectly()
    {
        // Arrange

        // Act
        var floor = new Floor(_number);

        // Assert
        floor.Number.Should().Be(_number,
            because: "constructor should correctly assign the number value");
    }

    /// <summary>
    /// Tests that the <see cref="Floor"/> constructor correctly assigns the <see cref="Floor.FloorId"/> property when
    /// provided with valid arguments.
    /// </summary>
    [Fact]
    public void Constructor_WithValidArguments_AssignFloorIdCorrectly()
    {
        // Arrange

        // Act
        var floor = new Floor(floorId: 1, _number);

        // Assert
        floor.FloorId.Should().Be(1,
            because: "constructor should correctly assign the floor id value");
    }

    /// <summary>
    /// Tests that the <see cref="Floor"/> constructor correctly assigns the <see cref="Floor.Number"/> property when
    /// valid arguments are provided.
    /// </summary>
    [Fact]
    public void Constructor_TwoParameters_WithValidArguments_AssignNumberCorrectly()
    {
        // Arrange

        // Act
        var floor = new Floor(floorId: 1, _number);

        // Assert
        floor.Number.Should().Be(_number,
            because: "constructor should correctly assign the number value");
    }

    /// <summary>
    /// Tests that the <see cref="Floor.ChangeFloorNumber(int)"/> method updates the <see cref="Floor.Number"/> property
    /// to the specified valid number.
    /// </summary>
    [Fact]
    public void ChangeFloorNumber_WithValidNumber_UpdatesNumberProperty()
    {
        // Arrange
        int newNumber = 2;

        // Act
        _floor.ChangeFloorNumber(newNumber);

        // Assert
        _floor.Number.Value.Should().Be(newNumber,
            because: "ChangeFloorNumber should update the Number property to the new value");
    }

    /// <summary>
    /// Verifies that attempting to change the floor number to an invalid value throws a <see
    /// cref="ValidationException"/>.
    /// </summary>
    [Fact]
    public void ChangeFloorNumber_WithInvalidNumber_ThrowsValidationException()
    {
        // Arrange
        int invalidNumber = -1;

        // Act
        Action act = () => _floor.ChangeFloorNumber(invalidNumber);

        // Assert
        act.Should().Throw<ValidationException>();
    }
}
