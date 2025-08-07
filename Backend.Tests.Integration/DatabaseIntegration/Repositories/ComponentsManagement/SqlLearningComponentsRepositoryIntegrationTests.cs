using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.DbInfra.DatabaseIntegration;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.DbInfra.DatabaseIntegration.Repositories.ComponentsManagement;

/// <summary>
/// Integration tests for the SQL Learning Components Repository.
/// </summary>
public class SqlLearningComponentsRepositoryIntegrationTests : IAsyncLifetime
{
    /// <summary>
    /// Fixture for setting up the test database context for learning components.
    /// </summary>
    private readonly TestDatabaseFixtureLearningComponents _databaseFixture;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlLearningComponentsRepositoryIntegrationTests"/> class.
    /// </summary>
    public SqlLearningComponentsRepositoryIntegrationTests()
    {
        _databaseFixture = new TestDatabaseFixtureLearningComponents("LC");
    }

    /// <summary>
    /// Initializes the test database by seeding it with learning components data.
    /// </summary>
    /// <returns></returns>
    public Task InitializeAsync()
    {
        return _databaseFixture.SeedLearningComponentsAsync();
    }

    /// <summary>
    /// Disposes of the test database fixture after tests are completed.
    /// </summary>
    /// <returns></returns>
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Tests that the GetAllAsync method returns the expected learning components from the seeded database.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GivenSeededFixture_WhenGetAllAsync_ReturnsExpectedLearningComponents()
    {
        // Arrange
        var repository = new SqlLearningComponentRepository(
            _databaseFixture.DatabaseContext,
            Microsoft.Extensions.Logging.Abstractions.NullLogger<SqlLearningComponentRepository>.Instance);

        // Act
        var result = (await repository.GetAllAsync(pageSize: 10, pageIndex: 0)).ToList();

        // Assert
        result.Should().HaveCount(2);

        var projector = result.OfType<Projector>().FirstOrDefault(p => p.ProjectedContent == "Math Presentation");
        var whiteboard = result.OfType<Whiteboard>().FirstOrDefault(w => w.MarkerColor!.Value == "Blue");

        projector.Should().NotBeNull().And.BeOfType<Projector>();
        ((Projector)projector!).ProjectedContent.Should().Be("Math Presentation");

        whiteboard.Should().NotBeNull().And.BeOfType<Whiteboard>();
        ((Whiteboard)whiteboard!).MarkerColor.Value.Should().Be("Blue");
    }

    /// <summary>
    /// Tests that the GetAllAsync method returns only non-deleted learning components from the seeded database.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GivenSeededComponents_WhenGetAllAsync_ReturnsNonDeletedLearningComponents()
    {
        // Arrange
        var repository = new SqlLearningComponentRepository(
            _databaseFixture.DatabaseContext,
            Microsoft.Extensions.Logging.Abstractions.NullLogger<SqlLearningComponentRepository>.Instance);

        // Act
        var result = await repository.GetAllAsync(pageSize: 10, pageIndex: 0);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThan(0);
        result.Should().OnlyContain(c => c.IsDeleted == false);
    }

}
