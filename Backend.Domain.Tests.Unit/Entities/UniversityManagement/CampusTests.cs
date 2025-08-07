using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities;
using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.UniversityManagement;


// PQL-AE-001-004 Add Campus #92
/*
    Feature: Add and register a new campus in the system.

    As a ThemePark@UCR Administrator,
    I want to be able to register a new campus in the system,
    so that I can maintain an updated record of the campuses available in the system.

    Acceptance Criteria
    Scenario: Successful adding of the new campus and registering of its information

*/
public class CampusTests
{
    private const string DefaultCampusName = "Rodrigo Facio";
    private const string DefaultUniversityName = "UCR";
    private const string DefaultLocation = "San Pedro";

    private readonly EntityName _campusName;
    private readonly EntityName _universityName;
    private readonly EntityLocation _location;
    private readonly University _university;

    /// <summary>
    /// Initializes shared test data for all test methods
    /// </summary>
    public CampusTests()
    {
        _campusName = EntityName.Create(DefaultCampusName);
        _universityName = EntityName.Create(DefaultUniversityName);
        _location = EntityLocation.Create(DefaultLocation);
        _university = new University(_universityName!, _location!);
    }

    /// <summary>
    /// Ensures that the <see cref="Campus.Location"/> property is correctly set when valid parameters are provided.
    /// </summary>
    [Fact]
    public void Campus_Constructor_WithValidParams_ShouldSetLocationCorrectly()
    {
        // Act
        var campus = new Campus(_campusName!, _location!, _university);

        // Assert
        campus.Location.Should().Be(_location);
    }

    /// <summary>
    /// Ensures that the <see cref="Campus.Name"/> property is correctly set when valid parameters are provided.
    /// </summary>
    [Fact]
    public void Campus_Constructor_WithValidParams_ShouldSetNameCorrectly()
    {
        // Act
        var campus = new Campus(_campusName!, _location!, _university);

        // Assert
        campus.Name.Should().Be(_campusName);
    }

    /// <summary>
    /// Ensures that the <see cref="Campus.University"/> property is correctly set when valid parameters are provided.
    /// </summary>
    [Fact]
    public void Campus_Constructor_WithValidParams_ShouldSetUniversityCorrectly()
    {
        // Act
        var campus = new Campus(_campusName!, _location!, _university);

        // Assert
        campus.University.Should().Be(_university);
    }

    /// <summary>
    /// Ensures that the <see cref="Campus.Name"/> throws exception when the input name is invalid.
    /// </summary>
    [Fact]
    public void Campus_ShouldThrowArgumentNullException_WhenInputNameIsInvalid()
    {
        // Arrange
        EntityName? name = null;

        // Act
        Action act = () => new Campus(name!, _location!, _university!);

        // Assert
        act.Should().Throw<ArgumentNullException>("because the name parameter cannot be null when creating a Campus");
    }

    /// <summary>
    /// Ensures that the <see cref="Campus.Location"/> throws exception when the input location is invalid.
    /// </summary>
    [Fact]
    public void Campus_ShouldThrowArgumentNullException_WhenInputLocationIsInvalid()
    {
        // Arrange
        EntityLocation? location = null;

        // Act
        Action act = () => new Campus(_campusName!, location!, _university!);

        // Assert
        act.Should().Throw<ArgumentNullException>("because the location parameter cannot be null when creating a Campus");
    }

    /// <summary>
    /// Ensures that the <see cref="Campus.University"/> is null when no university is provided.
    /// </summary>
    [Fact]
    public void Campus_ShouldThrowArgumentNullException_WhenUniversityIsNull()
    {
        // Arrange
        University? university = null;
        
        // Act
        Action act = () => new Campus(_campusName!, _location!, university!);

        // Assert
        act.Should().Throw<ArgumentNullException>("because the University parameter cannot be null when creating a Campus");
    }

    /// <summary>
    /// Ensures that the <see cref="Campus"/> constructor throws exception
    /// </summary>
    [Fact]
    public void Campus_Constructor_WithNullValues_ShouldThrowArgumentNullException()
    {
        // Arrange
        EntityName? name = null;
        EntityLocation? location = null;
        University? university = null;

        // Act
        Action act = () => new Campus(name!, location!, university!);

        // Assert
        act.Should().Throw<ArgumentNullException>
            ("because the campus name, the location and the university parameters cannot be null when creating a Campus");

    }

    /// <summary>
    /// Ensures that the <see cref="Campus"/> constructor creates independent instances.
    /// </summary>
    [Fact]
    public void Campus_Constructor_WithSameValuesMultipleTimes_ShouldCreateIndependentInstances()
    {
        // Act
        var campus1 = new Campus(_campusName!, _location!, _university);
        var campus2 = new Campus(_campusName!, _location!, _university);

        // Assert
        campus1.Should().NotBeSameAs(campus2);
        campus1.Name.Should().Be(campus2.Name);
    }
}
