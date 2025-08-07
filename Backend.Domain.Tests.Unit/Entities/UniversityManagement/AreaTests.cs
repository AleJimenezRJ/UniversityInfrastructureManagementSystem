using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.UniversityManagement;

public class AreaTests
{
    private readonly EntityName _areaName;
    private readonly Campus _campus;

    /// <summary>
    /// Assign: Initializes the test dependencies for the <see cref="Area"/> class.
    /// </summary>
    public AreaTests()
    {
        _areaName = EntityName.Create("Finca 1");
        _campus = CreateUniversityCampusDependencies();
    }

    /// <summary>
    /// Helper method to create and return an initialized instance of <see cref="Campus"/>.
    /// </summary>
    /// <remarks>
    /// This method centralizes the creation of dependent objects for testing purposes,
    /// reducing code repetition in area-related tests.
    /// </remarks>

    private static Campus CreateUniversityCampusDependencies()
    {
        // Create University
        var universityName = EntityName.Create("University of Costa Rica");
        var universityLocation = EntityLocation.Create("Costa Rica");
        var university = new University(universityName, universityLocation);

        // Create Campus associated with the University
        var campusName = EntityName.Create("Main Campus");
        var campusLocation = EntityLocation.Create("San Pedro");
        var campus = new Campus(campusName, campusLocation, university);

        return campus;
    }

    /// <summary>
    /// Unit test for the <see cref="Area"/> 
    /// verifies the constructor of the Area class and that it assings the name properly.
    /// 
    /// This test was created for the PBI PQL-AE-001-005: Add area.
    /// Acceptance criteria: Successful addition of the new area and registration of its information.
    /// 
    /// Technical task: Implement the area-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/97
    /// </summary>
    /// <remarks>
    /// This test ensures that the area constructor correctly creates a area object,
    /// and sets its name value correctly
    /// </remarks>
    [Fact]
    public void ConstructorWithoutId_WhenGivenValidParameters_InitializesNameCorrectly()
    {
        // Act
        // Create Area with valid parameters
        var area = new Area(
            _areaName,
            _campus);

        // Assert
        // Verify campus property is correctly initialized

        area.Name.Should().Be(_areaName,
            because: "the constructor should set the Name property");
    }

    /// <summary>
    /// Unit test for the <see cref="Area"/> 
    /// verifies the constructor of the Area class correctly assigns the Campus parameter
    /// 
    /// This test was created for the PBI PQL-AE-001-005: Add area.
    /// Acceptance criteria: Successful addition of the new area and registration of its information.
    /// 
    /// Technical task: Implement the area-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/97
    /// </summary>
    /// <remarks>
    /// This test ensures that the area constructor correctly creates a area object,
    /// and sets its campus value correctly
    /// </remarks>
    [Fact]
    public void ConstructorWithoutId_WhenGivenValidParameters_InitializesCampusCorrectly()
    {
        // Act
        // Create Area with valid parameters
        var area = new Area(
            _areaName,
            _campus);

        // Assert
        // Verify campus is correctly initialized

        area.Campus.Should().Be(_campus,
            because: "the constructor should associate the area with the correct campus");
    }


    /// <summary>
    /// Unit test for the <see cref="Area"/> 
    /// verifies the constructor with a null area returns an exception.
    /// 
    /// This test was created for the PBI PQL-AE-001-005: Add area.
    /// Acceptance criteria: Prevent addition of new areas with incorrect property format
    /// 
    /// Technical task: Implement the area-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/97
    /// </summary>
    /// <remarks>
    /// This test ensures that the area constructor rejects creating a area object when the campus is null
    /// </remarks>
    [Fact]
    public void ConstructorWithoutId_WhenCampusIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => new Area(_areaName, campus: null!);

        // Assert
        act.Should().Throw<ArgumentNullException>("the 'campus' parameter cannot be null when creating an Area without an ID")
           .WithParameterName("campus");
    }

    /// <summary>
    /// Unit test for the <see cref="Area"/> 
    /// verifies the constructor with a null name returns an exception.
    /// 
    /// This test was created for the PBI PQL-AE-001-005: Add area.
    /// Acceptance criteria: Prevent addition of new areas with incorrect property format
    /// 
    /// Technical task: Implement the area-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/97
    /// </summary>
    /// <remarks>
    /// This test ensures that the area constructor rejects creating an area object when the name is null.
    /// </remarks>
    [Fact]
    public void ConstructorWithoutId_WhenNameIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => new Area(name: null!, _campus);

        // Assert
        act.Should().Throw<ArgumentNullException>("the 'name' parameter cannot be null when creating an Area")
           .WithParameterName("name");
    }
}
