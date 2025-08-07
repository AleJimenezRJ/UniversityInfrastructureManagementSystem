using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities;
using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;


namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.UniversityManagement;


/// <summary>
/// Unit tests for the <see cref="University"/> class,
/// ensuring the creation and correctlogic.
/// </summary>

// PQL-AE-001-003 Add University #95
/*UserStory: Add and register a new university in the system.
 * As a ThemePark@UCR Administrator,
 * I want to be able to register a new university in the system,
 * so that I can maintain an updated record of the universities available in the system.
*/
// aceptanceScenario: Successful adding of the new university and registering of its information

public class UniversityTests
{
    private const string DefaultUniversityName = "Universidad de Costa Rica";
    private const string DefaultCountry = "Costa Rica";

    private readonly EntityName _universityName;
    private readonly EntityLocation _country;

    public UniversityTests()
    {
        _universityName = EntityName.Create(DefaultUniversityName);
        _country = EntityLocation.Create(DefaultCountry);
    }

    /// <summary>
    /// Verifies that the <see cref="University"/> constructor creates a non-null object 
    /// when provided with valid name and location inputs.
    /// </summary>
    [Fact]
    public void University_Constructor_WithValidNameAndCountry_ShouldCreateNonNullObject()
    {
        // Act
        var university = new University(_universityName, _country);

        // Assert
        university.Should().NotBeNull("because the constructor should create a valid object");
    }

    /// <summary>
    /// Ensures that the <see cref="University.Name"/> property is correctly assigned 
    /// when the constructor is used with valid parameters.
    /// </summary>
    [Fact]
    public void University_Constructor_ShouldAssign_Name_PropertyCorrectly()
    {
        // Act
        var university = new University(_universityName, _country);

        // Assert
        university.Name.Should().Be(_universityName);
    }

    /// <summary>
    /// Ensures that the <see cref="University.Country"/> property is correctly assigned 
    /// when the constructor is used with valid parameters.
    /// </summary>
    [Fact]
    public void University_Constructor_ShouldAssign_Country_PropertyCorrectly()
    {
        // Act
        var university = new University(_universityName, _country);

        // Assert
        university.Country.Should().Be(_country);
    }

    /// <summary>
    /// Ensures that the <see cref="University"/> constructor does not allow creation 
    /// when <see cref="EntityLocation.Create"/> receives a null string.
    /// </summary>
    [Fact]
    public void University_Constructor_WithNullCountry_ShouldThrowArgumentNullException()
    {
        // Arrange
        EntityLocation? nullCountry = null;

        // Act
        Action act = () => new University(_universityName, nullCountry!);

        // Assert
        act.Should().Throw<ArgumentNullException>("because the country parameter cannot be null when creating a University");
    }

    /// <summary>
    /// Ensures that name and country are null when inputs are invalid.
    /// </summary>
    [Fact]
    public void University_ShouldThrowArgumentNullException_WhenBothInputsAreInvalid()
    {
        // Arrange with different nulls type
        EntityName? nullName = null;
        EntityLocation? nullCountry = null;

        // Act
        Action act = () => new University(nullName!, nullCountry!); // both null

        // Assert
        act.Should().Throw<ArgumentNullException>("because the name and country parameters cannot be null when creating a Campus");
    }

    /// <summary>
    /// Ensures that <see cref="University.Name"/> is null when the input name is invalid (null).
    /// </summary>
    [Fact]
    public void University_ShouldThrowArgumentNullException_WhenInputNameIsInvalid()
    {
        // Arrange
        EntityName? nullName = null;

        // Act
        Action act = () => new University(nullName!, _country);

        // Assert
        act.Should().Throw<ArgumentNullException>("because the name parameter cannot be null when creating a University");

    }

}