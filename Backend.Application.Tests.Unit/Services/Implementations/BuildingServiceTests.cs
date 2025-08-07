using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Services.Implementations;

/// <summary>
/// Provides unit tests for the <see cref="BuildingsServices"/> class, ensuring its methods behave as expected when
/// interacting with a mocked <see cref="IBuildingsRepository"/>.
/// </summary>
/// <remarks>This test class is designed to validate the functionality of the <see cref="BuildingsServices"/>
/// class by using a strict mock of the <see cref="IBuildingsRepository"/> interface. It includes setup for test data,
/// such as a sample <see cref="Building"/> instance, and ensures that the service behaves correctly under various
/// scenarios.</remarks>
public class BuildingServiceTests
{
    /// <summary>
    /// A mock implementation of the <see cref="IBuildingsRepository"/> interface used for testing purposes.
    /// </summary>
    private readonly Mock<IBuildingsRepository> _buildingsRepositoryMock;

    /// <summary>
    /// Represents the service used to manage and interact with building-related operations.
    /// </summary>
    /// <remarks>This field is intended to store an instance of <see cref="BuildingsServices"/> for use within
    /// the containing class. It is marked as readonly to ensure the reference cannot be reassigned after
    /// initialization.</remarks>
    private readonly BuildingsServices _serviceUnderTest;

    /// <summary>
    /// Represents the building associated with this instance.
    /// </summary>
    /// <remarks>This field is read-only and is intended to store a reference to a <see cref="Building"/>
    /// object. It is not accessible outside the containing class.</remarks>
    private readonly Building _building;

    /// <summary>
    /// Initializes a new instance of the <see cref="BuildingServiceTests"/> class.
    /// </summary>
    /// <remarks>This constructor sets up the necessary dependencies and test data for unit testing the  <see
    /// cref="BuildingsServices"/> class. It initializes a strict mock of the <see cref="IBuildingsRepository"/> 
    /// interface and creates a sample <see cref="Building"/> instance with predefined properties for use in
    /// tests.</remarks>
    public BuildingServiceTests()
    {
        _buildingsRepositoryMock = new Mock<IBuildingsRepository>(MockBehavior.Strict);
        _serviceUnderTest = new BuildingsServices(_buildingsRepositoryMock.Object);
        
        var name = new EntityName("Main Test");
        var coordinates = new Coordinates(9.93, 84.00, 0);
        var dimensions = new Dimension(30.0, 20.0, 10.0);
        var colors = new Colors("Blue");

        var areaName = new EntityName("Testing Area");
        var campusName = new EntityName("Testing Campus");
        var universityName = new EntityName("Testing Uni");

        var university = new University(universityName);
        var campusLocation = new EntityLocation("San Pedro");
        var campus = new Campus(campusName, campusLocation, university);
        var area = new Area(areaName, campus);

        _building = new Building(
            buildingInternalId: 1,
            name: name,
            coordinates: coordinates,
            dimensions: dimensions,
            color: colors,
            area: area
        );
    }

    /// <summary>
    /// Tests that <see cref="BuildingsServices.AddBuildingAsync"/> returns <c>true</c>
    /// when the repository successfully adds the building.
    /// This test was done for the PBI PQL-AE-001-001 Add building
    /// For the acceptance criteria: Successful adding of the new building and registering of its information
    /// Technical task: Implement adding building item service
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/197
    /// </summary>
    /// <remarks>
    /// This test ensures that the service correctly forwards a successful result from the repository
    /// and verifies that the repository's <c>AddBuildingAsync</c> method is called exactly once
    /// with the expected building instance.
    /// </remarks>
    [Fact]
    public async Task AddBuildingAsync_ReturnsTrue_WhenRepositoryReturnsTrue()
    {
        // Arrange
        _buildingsRepositoryMock
            .Setup(repo => repo.AddBuildingAsync(_building))
            .ReturnsAsync(true);

        // Act
        var result = await _serviceUnderTest.AddBuildingAsync(_building);

        // Assert
        result.Should().BeTrue();
        _buildingsRepositoryMock.Verify(repo => repo.AddBuildingAsync(_building), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="BuildingsServices.UpdateBuildingAsync"/> returns <c>true</c>
    /// when the repository successfully edits all the building information.
    /// This test was done for the PBI PQL-AE-001-002 Edit building information
    /// For the acceptance criteria: Edit all the building information.
    /// Technical task: Implement editing building item service
    /// Link to the PBI: https://github.com/orgs/UCR-PI-IS/projects/65/views/1?filterQuery=iteration%3A%40current+edit+&pane=issue&itemId=105752062&issue=UCR-PI-IS%7Cecci_ci0128_i2025_g01_pi%7C16
    /// </summary>
    /// <remarks>
    /// This test ensures that the service correctly forwards a successful result from the repository
    /// and verifies that the repository's <c>UpdateBuildingAsync</c> method is called exactly once
    /// with the expected building instance.
    /// </remarks>
    [Fact]
    public async Task UpdateBuildingAsync_ReturnsTrue_WhenRepositoryReturnsTrue()
    {
        // Arrange
        int buildingId = 1;
        _buildingsRepositoryMock
            .Setup(repo => repo.UpdateBuildingAsync(_building, buildingId))
            .ReturnsAsync(true);
        // Act
        var result = await _serviceUnderTest.UpdateBuildingAsync(_building, buildingId);
        // Assert
        result.Should().BeTrue();
        _buildingsRepositoryMock.Verify(repo => repo.UpdateBuildingAsync(_building, buildingId), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="BuildingsServices.DisplayBuildingAsync"/> returns <c>true</c>
    /// when the repository successfully retrieves all the building information.
    /// This test was done for the PBI PQL-LB-002-002: Display Specific Building Information
    /// For the acceptance criteria: View information of existing building
    /// Technical task: Implement displaying building information service	
    /// Link to the PBI: https://github.com/orgs/UCR-PI-IS/projects/65/views/1?filterQuery=iteration%3A%40current+display&pane=issue&itemId=108177858&issue=UCR-PI-IS%7Cecci_ci0128_i2025_g01_pi%7C33
    /// </summary>
    /// <remarks>
    /// This test ensures that the service correctly forwards a successful result from the repository
    /// and verifies that the repository's <c>DisplayBuildingAsync</c> method is called exactly once
    /// with the expected building instance.
    /// </remarks>

    [Fact]
    public async Task DisplayBuildingAsync_ReturnsBuilding_WhenFound()
    {
        // Arrange
        int buildingId = 1;
        _buildingsRepositoryMock
            .Setup(repo => repo.DisplayBuildingAsync(buildingId))
            .ReturnsAsync(_building);

        // Act
        var result = await _serviceUnderTest.DisplayBuildingAsync(buildingId);

        // Assert
        result.Should().Be(_building);
        _buildingsRepositoryMock.Verify(repo => repo.DisplayBuildingAsync(buildingId), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="BuildingsServices.DisplayBuildingAsync"/> returns <c>Null</c>
    /// when the repository has no building information for that specific one.
    /// This test was done for the PBI PQL-LB-002-002: Display Specific Building Information
    /// For the acceptance criteria: View information of building that does not exist.
    /// Technical task: Implement displaying building information service	
    /// Link to the PBI: https://github.com/orgs/UCR-PI-IS/projects/65/views/1?filterQuery=iteration%3A%40current+display&pane=issue&itemId=108177858&issue=UCR-PI-IS%7Cecci_ci0128_i2025_g01_pi%7C33
    /// </summary>
    /// <remarks>
    /// This test ensures that the service correctly forwards a null result from the repository
    /// and verifies that the repository's <c>DisplayBuildingAsync</c> method is called exactly once
    /// with the expected null building instance.
    /// </remarks>

    [Fact]
    public async Task DisplayBuildingAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        int buildingId = 60; 
        _buildingsRepositoryMock
            .Setup(repo => repo.DisplayBuildingAsync(buildingId))
            .ReturnsAsync((Building?)null);
        // Act
        var result = await _serviceUnderTest.DisplayBuildingAsync(buildingId);
        // Assert
        result.Should().BeNull();
        _buildingsRepositoryMock.Verify(repo => repo.DisplayBuildingAsync(buildingId), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="BuildingsServices.ListBuildingAsync"/> returns <c>A list</c>
    /// when the repository has a list of buildings.
    /// This test was done for the PBI PQL-LB-002-001 List buildings #40
    /// For the acceptance criteria: Display all registered buildings
    /// Technical task: Implement displaying building list service
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/40
    /// </summary>
    /// <remarks>
    /// This test ensures that the service correctly forwards a list result from the repository
    /// and verifies that the repository's <c>ListBuildingAsync</c> method is called exactly once
    /// with the expected building list instance.
    /// </remarks>

    [Fact]
    public async Task ListBuildingAsync_ReturnsListOfBuildings()
    {
        // Arrange
        var buildings = new List<Building> { _building };
        _buildingsRepositoryMock
            .Setup(repo => repo.ListBuildingAsync())
            .ReturnsAsync(buildings);

        // Act
        var result = await _serviceUnderTest.ListBuildingAsync();

        // Assert
        result.Should().ContainSingle().Which.Should().Be(_building);
        _buildingsRepositoryMock.Verify(repo => repo.ListBuildingAsync(), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="BuildingsServices.DeleteBuildingAsync"/> returns <c>true</c>
    /// when the repository successfully deletes a building.
    /// This test was done for the PQL-RE-003-001 Delete building
    /// For the acceptance criteria: Delete an existing building from the system
    /// Technical task: Implement deleting building item service
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/57
    /// </summary>
    /// <remarks>
    /// This test ensures that the service correctly forwards a successful result from the repository
    /// and verifies that the repository's <c>DeleteBuildingAsync</c> method is called exactly once
    /// with the expected building instance.
    /// </remarks>
    [Fact]
    public async Task DeleteBuildingAsync_ReturnsTrue_WhenRepositoryReturnsTrue()
    {
        // Arrange
        int buildingId = 1;
        _buildingsRepositoryMock
            .Setup(repo => repo.DeleteBuildingAsync(buildingId))
            .ReturnsAsync(true);

        // Act
        var result = await _serviceUnderTest.DeleteBuildingAsync(buildingId);

        // Assert
        result.Should().BeTrue();
        _buildingsRepositoryMock.Verify(repo => repo.DeleteBuildingAsync(buildingId), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="BuildingsServices.AddBuildingAsync"/> returns <c>false</c>
    /// when the repository fails to add the building.
    /// This test was done for the PBI PQL-AE-001-001 Add building
    /// For the acceptance criteria: Prevent addition of new buildings with incorrect property format and Prevent duplicate building registration
    /// Technical task: Implement adding building item service
    /// Link to the PBI: https://github.com/UCR-PI-IS/ecci_ci0128_i2025_g01_pi/issues/197
    /// </summary>
    /// <remarks>
    /// This test ensures that the service correctly forwards a fails result from the repository
    /// and verifies that the repository's <c>AddBuildingAsync</c> method is called exactly once
    /// with the expected building instance.
    /// </remarks>
    [Fact]
    public async Task AddBuildingAsync_ReturnsFalse_WhenRepositoryFails()
    {
        // Arrange
        _buildingsRepositoryMock
            .Setup(repo => repo.AddBuildingAsync(_building))
            .ReturnsAsync(false);

        // Act
        var result = await _serviceUnderTest.AddBuildingAsync(_building);

        // Assert
        result.Should().BeFalse();
        _buildingsRepositoryMock.Verify(repo => repo.AddBuildingAsync(_building), Times.Once);
    }
}
