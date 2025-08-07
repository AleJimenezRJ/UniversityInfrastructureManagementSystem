using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Locations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Integration.Repositories.UniversityManagement;

/// <summary>
/// Integration tests for the <see cref="IUniversityRepository"/> using a SQL-based implementation.
/// These tests ensure correct persistence behavior for the University entity.
/// </summary>

[Collection("Database collection")]
public class SqlUniversityRepositoryIntegrationTests
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlUniversityRepositoryIntegrationTests"/> class.
    /// </summary>
    /// <param name="fixture">Fixture providing the integration test services.</param>
    public SqlUniversityRepositoryIntegrationTests(IntegrationTestFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }

    /// <summary>
    /// Verifies that a University can be successfully added to the database 
    /// </summary>
    [Fact]
    public async Task AddUniversity_Should_Insert_University()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IUniversityRepository>();

        // Create University associated with the persisted area
        var university = new University(EntityName.Create("NameX"), EntityLocation.Create("CountryX"));

        // Act: Attempt to insert the University
        var result = await repo.AddUniversityAsync(university);

        // Assert: Ensure insertion succeeds
        Assert.True(result);
    }

    /// <summary>
    /// Tests that the <see cref="IUniversityRepository.ListUniversityAsync"/> method 
    /// returns universities that have been inserted into the database.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task ListUniversity_Should_Return_Inserted_University()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IUniversityRepository>();

        var university = new University(EntityName.Create("Universidad Test"), EntityLocation.Create("CR"));

        context.University.Add(university);
        await context.SaveChangesAsync();

        var result = await repo.ListUniversityAsync();

        Assert.Contains(result, c => c.Name == university.Name);
    }

    /// <summary>
    /// Tests that adding a <see cref="University"/>  results in a <see cref="DuplicatedEntityException"/>,
    /// due to a university already existing with that name.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task AddDuplicateUniversity_Should_Fail_To_Insert_University()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IUniversityRepository>();

        var university = new University(new EntityName("Duplicado Test"), EntityLocation.Create("CR"));

        await repo.AddUniversityAsync(university);

        // Wait for an exception in the database due to duplicate entity
        await Assert.ThrowsAsync<DuplicatedEntityException>(async () =>
        {
            await repo.AddUniversityAsync(university);
        });
    }

}
