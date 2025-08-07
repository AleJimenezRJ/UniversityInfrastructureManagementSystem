using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.UniversityManagement;


public class BuildingTests
{
    private const int _buildingInternalId = 1;
    private readonly EntityName _name;
    private readonly Coordinates _coordinates;
    private readonly Dimension _dimensions;
    private readonly Colors _color;
    private readonly Area _area;

    /// <summary>
    /// Arrange: Initializes the test dependencies for the <see cref="Building"/> class.
    /// </summary>
    public BuildingTests()
    {
        // Create Area
        _area = CreateUniversityCampusAreaDependencies();

        // Define Building parameters
        _name = EntityName.Create("Main Building");
        _coordinates = Coordinates.Create(10.0, -84.0, -20.6);
        _dimensions = Dimension.Create(50, 100, 20);
        _color = Colors.Create("Red");
    }
    /// <summary>
    /// Helper method to create and return an initialized instance of <see cref="Area"/>.
    /// </summary>
    /// <remarks>
    /// This method centralizes the creation of dependent objects for testing purposes,
    /// reducing code repetition in building-related tests.
    /// </remarks>
    private static Area CreateUniversityCampusAreaDependencies()
    {
        // Create University
        var universityName = EntityName.Create("University of Costa Rica");
        var universityLocation = EntityLocation.Create("Costa Rica");
        var university = new University(universityName, universityLocation);

        // Create Campus associated with the University
        var campusName = EntityName.Create("Main Campus");
        var campusLocation = EntityLocation.Create("San Pedro");
        var campus = new Campus(campusName, campusLocation, university);

        // Create Area associated with the Campus
        var areaName = EntityName.Create("Finca 1");
        var area = new Area(areaName, campus);

        return area;
    }
    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies that the constructor with ID correctly sets the <see cref="Building.BuildingInternalId"/> property.
    ///
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Successful addition of the new building and registration of its information.
    ///
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the Building constructor assigns the correct internal ID to the building.
    /// </remarks>
    [Fact]
    public void ConstructorWithId_WhenGivenValidParameters_ShouldSetInternalIdCorrectly()
    {
        // Act
        var building = new Building(
            _buildingInternalId,
            _name,
            _coordinates,
            _dimensions,
            _color,
            _area);

        // Assert
        building.BuildingInternalId.Should().Be(_buildingInternalId,
            because: "the constructor should set the internal ID");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies that the constructor with ID correctly sets the <see cref="Building.Name"/> property.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Successful addition of the new building and registration of its information.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the Building constructor assigns the correct name to the building.
    /// </remarks>
    [Fact]
    public void ConstructorWithId_WhenGivenValidParameters_ShouldSetNameCorrectly()
    {
        // Act
        var building = new Building(
            _buildingInternalId,
            _name,
            _coordinates,
            _dimensions,
            _color,
            _area);

        // Assert
        building.Name.Should().Be(_name,
            because: "the constructor should set the Name property");
    }
    
    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies that the constructor with ID correctly sets the <see cref="Building.BuildingCoordinates"/> property.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Successful addition of the new building and registration of its information.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the Building constructor assigns the correct coordinates to the building.
    /// </remarks>
    [Fact]
    public void ConstructorWithId_WhenGivenValidParameters_ShouldSetCoordinatesCorrectly()
    {
        // Act
        var building = new Building(
            _buildingInternalId,
            _name,
            _coordinates,
            _dimensions,
            _color,
            _area);

        // Assert
        building.BuildingCoordinates.Should().Be(_coordinates,
            because: "the constructor should set the geographical coordinates");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies that the constructor with ID correctly sets the <see cref="Building.Dimensions"/> property.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Successful addition of the new building and registration of its information.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the Building constructor assigns the correct dimensions to the building.
    /// </remarks>
    [Fact]
    public void ConstructorWithId_WhenGivenValidParameters_ShouldSetDimensionsCorrectly()
    {
        // Act
        var building = new Building(
            _buildingInternalId,
            _name,
            _coordinates,
            _dimensions,
            _color,
            _area);

        // Assert
        building.Dimensions.Should().Be(_dimensions,
            because: "the constructor should set the building dimensions");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies that the constructor with ID correctly sets the <see cref="Building.Color"/> property.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Successful addition of the new building and registration of its information.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the Building constructor assigns the correct color to the building.
    /// </remarks>
    [Fact]
    public void ConstructorWithId_WhenGivenValidParameters_ShouldSetColorCorrectly()
    {
        // Act
        var building = new Building(
            _buildingInternalId,
            _name,
            _coordinates,
            _dimensions,
            _color,
            _area);

        // Assert
        building.Color.Should().Be(_color,
            because: "the constructor should set the color scheme");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies that the constructor with ID correctly sets the <see cref="Building.Area"/> property.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Successful addition of the new building and registration of its information.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the Building constructor assigns the correct area to the building.
    /// </remarks>
    [Fact]
    public void ConstructorWithId_WhenGivenValidParameters_ShouldSetAreaCorrectly()
    {
        // Act
        var building = new Building(
            _buildingInternalId,
            _name,
            _coordinates,
            _dimensions,
            _color,
            _area);

        // Assert
        building.Area.Should().Be(_area,
            because: "the constructor should associate the building with the correct area");
    }


    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies the constructor without ID correctly sets the <see cref="Building.Name"/> property.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Successful addition of the new building and registration of its information.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the building constructor assigns the correct name to the building.
    /// </remarks>
    [Fact]
    public void ConstructorWithoutId_WhenGivenValidParameters_SetsNameCorrectly()
    {
        // Act
        var building = new Building(
            _name,
            _coordinates,
            _dimensions,
            _color,
            _area);

        // Assert
        building.Name.Should().Be(_name,
            because: "the constructor should set the Name property");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies the constructor without ID correctly sets the <see cref="Building.BuildingCoordinates"/> property.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Successful addition of the new building and registration of its information.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the building constructor assigns the correct geographical coordinates to the building.
    /// </remarks>
    [Fact]
    public void ConstructorWithoutId_WhenGivenValidParameters_SetsCoordinatesCorrectly()
    {
        // Act
        var building = new Building(
            _name,
            _coordinates,
            _dimensions,
            _color,
            _area);

        // Assert
        building.BuildingCoordinates.Should().Be(_coordinates,
            because: "the constructor should set the geographical coordinates");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies the constructor without ID correctly sets the <see cref="Building.Dimensions"/> property.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Successful addition of the new building and registration of its information.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the building constructor assigns the correct dimensions to the building.
    /// </remarks>
    [Fact]
    public void ConstructorWithoutId_WhenGivenValidParameters_SetsDimensionsCorrectly()
    {
        // Act
        var building = new Building(
            _name,
            _coordinates,
            _dimensions,
            _color,
            _area);

        // Assert
        building.Dimensions.Should().Be(_dimensions,
            because: "the constructor should set the building dimensions");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies the constructor without ID correctly sets the <see cref="Building.Color"/> property.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Successful addition of the new building and registration of its information.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the building constructor assigns the correct color scheme to the building.
    /// </remarks>
    [Fact]
    public void ConstructorWithoutId_WhenGivenValidParameters_SetsColorCorrectly()
    {
        // Act
        var building = new Building(
            _name,
            _coordinates,
            _dimensions,
            _color,
            _area);

        // Assert
        building.Color.Should().Be(_color,
            because: "the constructor should set the color scheme");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies the constructor without ID correctly sets the <see cref="Building.Area"/> property.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Successful addition of the new building and registration of its information.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the building constructor associates the building with the correct area.
    /// </remarks>
    [Fact]
    public void ConstructorWithoutId_WhenGivenValidParameters_SetsAreaCorrectly()
    {
        // Act
        var building = new Building(
            _name,
            _coordinates,
            _dimensions,
            _color,
            _area);

        // Assert
        building.Area.Should().Be(_area,
            because: "the constructor should associate the building with the correct area");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies the constructor with Id and with a null area returns an exception.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Prevent addition of new buildings with incorrect property format
    /// 
    /// Technical task: Implement the building-adding service.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the building constructor with Id rejects creating a building object when the area is null
    /// </remarks>
    [Fact]
    public void ConstructorWithId_WhenAreaIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => new Building(_buildingInternalId, _name, _coordinates, _dimensions, _color, area: null!);

        // Assert
        act.Should().Throw<ArgumentNullException>("the 'area' parameter cannot be null when creating a Building with an ID")
           .WithParameterName("area");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies the constructor with a null area returns an exception.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Prevent addition of new buildings with incorrect property format
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the building constructor rejects creating a building object when the area is null
    /// </remarks>
    [Fact]
    public void ConstructorWithoutId_WhenAreaIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => new Building(_name, _coordinates, _dimensions, _color, area: null!);

        // Assert
        act.Should().Throw<ArgumentNullException>("the 'area' parameter cannot be null when creating a Building without an ID")
           .WithParameterName("area");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies the constructor without Id throws an exception when the name is null.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Prevent addition of new buildings with incorrect property format.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the building constructor rejects creating a building object when the building name is null.
    /// </remarks>
    [Fact]
    public void ConstructorWithoutId_WhenNameIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => new Building(name: null!, _coordinates, _dimensions, _color, _area);

        // Assert
        act.Should().Throw<ArgumentNullException>("the 'name' parameter cannot be null when creating a Building without an ID")
           .WithParameterName("name");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies the constructor without Id throws an exception when coordinates are null.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Prevent addition of new buildings with incorrect property format.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the building constructor rejects creating a building object when the coordinates are null.
    /// </remarks>
    [Fact]
    public void ConstructorWithoutId_WhenCoordinatesIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => new Building(_name, coordinates: null!, _dimensions, _color, _area);

        // Assert
        act.Should().Throw<ArgumentNullException>("the 'coordinates' parameter cannot be null when creating a Building without an ID")
           .WithParameterName("coordinates");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies the constructor without Id throws an exception when dimensions are null.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Prevent addition of new buildings with incorrect property format.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the building constructor rejects creating a building object when the dimensions are null.
    /// </remarks>
    [Fact]
    public void ConstructorWithoutId_WhenDimensionsIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => new Building(_name, _coordinates, dimensions: null!, _color, _area);

        // Assert
        act.Should().Throw<ArgumentNullException>("the 'dimensions' parameter cannot be null when creating a Building without an ID")
           .WithParameterName("dimensions");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies the constructor without Id throws an exception when the color is null.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Prevent addition of new buildings with incorrect property format.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the building constructor rejects creating a building object when the color is null.
    /// </remarks>
    [Fact]
    public void ConstructorWithoutId_WhenColorIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => new Building(_name, _coordinates, _dimensions, color: null!, _area);

        // Assert
        act.Should().Throw<ArgumentNullException>("the 'color' parameter cannot be null when creating a Building without an ID")
           .WithParameterName("color");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies the constructor with Id throws an exception when the name is null.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Prevent addition of new buildings with incorrect property format.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the building constructor rejects creating a building object when the building name is null.
    /// </remarks>
    [Fact]
    public void ConstructorWithId_WhenNameIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => new Building(_buildingInternalId, name: null!, _coordinates, _dimensions, _color, _area);

        // Assert
        act.Should().Throw<ArgumentNullException>("the 'name' parameter cannot be null when creating a Building with an ID")
           .WithParameterName("name");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies the constructor with Id throws an exception when coordinates are null.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Prevent addition of new buildings with incorrect property format.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the building constructor rejects creating a building object when the coordinates are null.
    /// </remarks>
    [Fact]
    public void ConstructorWithId_WhenCoordinatesIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => new Building(_buildingInternalId, _name, coordinates: null!, _dimensions, _color, _area);

        // Assert
        act.Should().Throw<ArgumentNullException>("the 'coordinates' parameter cannot be null when creating a Building with an ID")
           .WithParameterName("coordinates");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies the constructor with Id throws an exception when dimensions are null.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Prevent addition of new buildings with incorrect property format.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the building constructor rejects creating a building object when the dimensions are null.
    /// </remarks>
    [Fact]
    public void ConstructorWithId_WhenDimensionsIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => new Building(_buildingInternalId, _name, _coordinates, dimensions: null!, _color, _area);

        // Assert
        act.Should().Throw<ArgumentNullException>("the 'dimensions' parameter cannot be null when creating a Building with an ID")
           .WithParameterName("dimensions");
    }

    /// <summary>
    /// Unit test for the <see cref="Building"/> 
    /// verifies the constructor with Id throws an exception when the color is null.
    /// 
    /// This test was created for the PBI PQL-AE-001-001: Add building.
    /// Acceptance criteria: Prevent addition of new buildings with incorrect property format.
    /// 
    /// Technical task: Implement the building-adding constructor.
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/30
    /// </summary>
    /// <remarks>
    /// This test ensures that the building constructor rejects creating a building object when the color is null.
    /// </remarks>
    [Fact]
    public void ConstructorWithId_WhenColorIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Action act = () => new Building(_buildingInternalId, _name, _coordinates, _dimensions, color: null!, _area);

        // Assert
        act.Should().Throw<ArgumentNullException>("the 'color' parameter cannot be null when creating a Building with an ID")
           .WithParameterName("color");
    }
}
