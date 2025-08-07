using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Locations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Integration.Repositories.UniversityManagement;

/// <summary>
/// Integration tests for the <see cref="IAreaRepository"/> using a SQL-based implementation.
/// These tests ensure correct persistence behavior for the Area entity.
/// </summary>

[Collection("Database collection")]
public class SqlAreaRepositoryIntegrationTests
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlAreaRepositoryIntegrationTests"/> class.
    /// </summary>
    /// <param name="fixture">Fixture providing the integration test services.</param>
    public SqlAreaRepositoryIntegrationTests(IntegrationTestFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }

    /// <summary>
    /// Verifies that a Area can be successfully added to the database 
    /// when its associated University and Campus is properly persisted.
    /// </summary>
    [Fact]
    public async Task AddArea_Should_Insert_Area()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IAreaRepository>();

        // Arrange: Create and persist the entity hierarchy
        var university = new University(EntityName.Create("Online"), EntityLocation.Create("Mexico"));
        var campus = new Campus(EntityName.Create("Call Saul"), EntityLocation.Create("Albuq"),university);

        context.University.Add(university);
        context.Campus.Add(campus);
        await context.SaveChangesAsync();

        // Create Area associated with the persisted campus
        var area = new Area(
            EntityName.Create("areaY"),
            campus
        );

        area.CampusName = campus.Name;

        // Act: Attempt to insert the Area
        var result = await repo.AddAreaAsync(area);

        // Assert: Ensure insertion succeeds
        Assert.True(result);
    }

    /// <summary>
    /// Tests that the <see cref="IAreaRepository.ListAreaAsync"/> method 
    /// returns areas that have been inserted into the database.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task ListArea_Should_Return_Inserted_Area()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IAreaRepository>();

        var university = new University(EntityName.Create("Universidad Area Test"), EntityLocation.Create("CR"));
        var campus = new Campus(EntityName.Create("Campus Area test"), EntityLocation.Create("Test City"), university);
        var area = new Area(EntityName.Create("Area Test"), campus);

        context.University.Add(university);
        context.Campus.Add(campus);
        context.Area.Add(area);
        await context.SaveChangesAsync();

        var result = await repo.ListAreaAsync();

        Assert.Contains(result, c => c.Name == area.Name);
    }

    /// <summary>
    /// Tests that adding a <see cref="Area"/> without persisting its related 
    /// <see cref="University"/> and <see cref="Campus"/> entity results in a <see cref="DbUpdateException"/>,
    /// due to a foreign key constraint violation.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task AddArea_Should_Fail_To_Insert_Area()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IAreaRepository>();

        var university = new University(EntityName.Create("Universidad X"));
        var campus = new Campus(EntityName.Create("Campus X"), EntityLocation.Create("X City"), university);
        var area = new Area(EntityName.Create("Area X"), campus);

        // Wait for an exception in the database
        await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            await repo.AddAreaAsync(area);
        });
    }

}
