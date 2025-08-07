using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Integration.Repositories.UniversityManagement;

/// <summary>
/// Integration tests for the <see cref="IBuildingsRepository"/> using a SQL-based implementation.
/// These tests ensure correct persistence behavior for the Building entity.
/// </summary>

[Collection("Database collection")]
public class SqlBuildingRepositoryIntegrationTests
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlBuildingRepositoryIntegrationTests"/> class.
    /// </summary>
    /// <param name="fixture">Fixture providing the integration test services.</param>
    public SqlBuildingRepositoryIntegrationTests(IntegrationTestFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }

    /// <summary>
    /// Verifies that attempting to add a building without saving its related entities 
    /// (University, Campus, Area) fails as expected due to foreign key constraints and saving them to the database
    /// </summary>
    [Fact]
    public async Task AddBuilding_Should_Fail_To_Insert_Building()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IBuildingsRepository>();

        // Arrange: Create building and related hierarchy (not persisted)
        var university = new University(new EntityName("U"));
        var campus = new Campus(new EntityName("C"), new EntityLocation("Loc"), university);
        var area = new Area(new EntityName("A"), campus);
        var building = new Building(
            name: EntityName.Create("Engineering Fails"),
            coordinates: Coordinates.Create(10.0, 20.0, 0.0),
            dimensions: Dimension.Create(30, 40, 10),
            color: Colors.Create("Blue"),
            area: area
        );
        building.AreaName = area.Name;

        // Act: Attempt to add building without persisting its related entities
        var result = await repo.AddBuildingAsync(building);

        // Assert: Ensure insertion fails
        Assert.False(result);
    }

    /// <summary>
    /// Verifies that a building can be successfully added to the database 
    /// when its associated University, Campus, and Area entities are properly persisted.
    /// </summary>
    [Fact]
    public async Task AddBuilding_Should_Insert_Building()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IBuildingsRepository>();

        // Arrange: Create and persist the entity hierarchy
        var university = new University(EntityName.Create("Uni"), EntityLocation.Create("CR"));
        var campus = new Campus(EntityName.Create("Campus"), EntityLocation.Create("Location"), university);
        var area = new Area(EntityName.Create("Areas"), campus);

        context.University.Add(university);
        context.Campus.Add(campus);
        context.Area.Add(area);
        await context.SaveChangesAsync();

        // Create building associated with the persisted area
        var building = new Building(
            EntityName.Create("Engineering Test Building"),
            Coordinates.Create(10.0, 20.0, 0.0),
            Dimension.Create(30, 40, 10),
            Colors.Create("Blue"),
            area
        );
        building.AreaName = area.Name;

        // Act: Attempt to insert the building
        var result = await repo.AddBuildingAsync(building);

        // Assert: Ensure insertion succeeds
        Assert.True(result);
    }
}
