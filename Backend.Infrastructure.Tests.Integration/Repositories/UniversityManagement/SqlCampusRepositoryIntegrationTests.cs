using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Integration.Repositories.UniversityManagement;

/// <summary>
/// Integration tests for the <see cref="ICampusRepository"/> using a SQL-based implementation.
/// These tests ensure correct persistence behavior for the Campus entity.
/// </summary>

[Collection("Database collection")]
public class SqlCampusRepositoryIntegrationTests
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlCampusRepositoryIntegrationTests"/> class.
    /// </summary>
    /// <param name="fixture">Fixture providing the integration test services.</param>
    public SqlCampusRepositoryIntegrationTests(IntegrationTestFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }

    /// <summary>
    /// Verifies that a Campus can be successfully added to the database 
    /// when its associated University is properly persisted.
    /// </summary>
    [Fact]
    public async Task AddCampus_Should_Insert_Campus()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<ICampusRepository>();

        // Arrange: Create and persist the entity hierarchy
        var university = new University(EntityName.Create("Online Courses"), EntityLocation.Create("New Mexico"));

        context.University.Add(university);
        await context.SaveChangesAsync();

        // Create Campus associated with the persisted area
        var Campus = new Campus(
            name: EntityName.Create("Better Call Saul"),
            location: EntityLocation.Create("Albuquerque"),
            university
        );

        // Act: Attempt to insert the Campus
        var result = await repo.AddCampusAsync(Campus);

        // Assert: Ensure insertion succeeds
        Assert.True(result);
    }

    /// <summary>
    /// Tests that the <see cref="ICampusRepository.ListCampusAsync"/> method 
    /// returns campuses that have been inserted into the database.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task ListCampus_Should_Return_Inserted_Campus()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<ICampusRepository>();

        var university = new University(EntityName.Create("Universidad Campus Test"), EntityLocation.Create("CR"));
        var campus = new Campus(EntityName.Create("Campus Test"), EntityLocation.Create("Test City"), university);

        context.University.Add(university);
        context.Campus.Add(campus);
        await context.SaveChangesAsync();

        var result = await repo.ListCampusAsync();

        Assert.Contains(result, c => c.Name == campus.Name);
    }

    /// <summary>
    /// Tests that adding a <see cref="Campus"/> without persisting its related 
    /// <see cref="University"/> entity results in a <see cref="DbUpdateException"/>,
    /// due to a foreign key constraint violation.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task AddCampus_Should_Fail_To_Insert_Campus()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ICampusRepository>();

        var university = new University(new EntityName("Universidad X"));
        var campus = new Campus(new EntityName("Campus Norte"), new EntityLocation("Zona 1"), university);

        // Wait for an exception in the database
        await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            await repo.AddCampusAsync(campus);
        });
    }

}
