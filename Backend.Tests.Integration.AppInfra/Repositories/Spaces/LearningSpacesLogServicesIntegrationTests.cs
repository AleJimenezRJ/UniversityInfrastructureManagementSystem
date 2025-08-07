using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Application;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AppInfra.Services.Spaces;

/// <summary>
/// Integration tests for LearningSpaceLogServices that test the interaction between
/// Application and Infrastructure layers for learning space log operations.
/// </summary>
public class LearningSpaceLogServicesIntegrationTests
{
    private readonly ILearningSpaceLogServices _learningSpaceLogServices;
    private readonly ThemeParkDataBaseContext _context;

    public LearningSpaceLogServicesIntegrationTests()
    {
        var databaseName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<ThemeParkDataBaseContext>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;

        _context = new ThemeParkDataBaseContext(options);

        // Pre-seed learning space logs for tests
        SeedTestData();
        _context.SaveChanges();

        // Setup services with in-memory database
        var services = new ServiceCollection();
        services.AddLogging();

        // Configure the same in-memory database for all services
        services.AddDbContext<ThemeParkDataBaseContext>(contextOptions =>
            contextOptions.UseInMemoryDatabase(databaseName: databaseName));

        // Register infrastructure services without DbContext registration
        AddInfrastructureServicesForTesting(services);
        services.AddApplicationLayerServices();

        var serviceProvider = services.BuildServiceProvider();
        _learningSpaceLogServices = serviceProvider.GetRequiredService<ILearningSpaceLogServices>();
    }

    /// <summary>
    /// Seeds the test database with sample learning space log entries.
    /// </summary>
    private void SeedTestData()
    {
        var now = DateTime.UtcNow;

        var log1 = new LearningSpaceLog
        {
            LearningSpaceLogInternalId = 1,
            Name = "Lab 101",
            Type = "Laboratory",
            MaxCapacity = 30,
            Width = 5.0m,
            Height = 3.0m,
            Length = 6.0m,
            ColorFloor = "White",
            ColorWalls = "Blue",
            ColorCeiling = "Gray",
            ModifiedAt = now.AddDays(-2),
            Action = "Created"
        };

        var log2 = new LearningSpaceLog
        {
            LearningSpaceLogInternalId = 2,
            Name = "Lab 101",
            Type = "Laboratory",
            MaxCapacity = 35,
            Width = 5.0m,
            Height = 3.0m,
            Length = 6.0m,
            ColorFloor = "White",
            ColorWalls = "Blue",
            ColorCeiling = "Gray",
            ModifiedAt = now.AddDays(-1),
            Action = "Updated"
        };

        var log3 = new LearningSpaceLog
        {
            LearningSpaceLogInternalId = 3,
            Name = "Classroom 201",
            Type = "Classroom",
            MaxCapacity = 25,
            Width = 6.0m,
            Height = 3.5m,
            Length = 7.0m,
            ColorFloor = "Gray",
            ColorWalls = "White",
            ColorCeiling = "Blue",
            ModifiedAt = now,
            Action = "Created"
        };

        _context.LearningSpaceLog.AddRange(log1, log2, log3);
    }

    private static void AddInfrastructureServicesForTesting(IServiceCollection services)
    {
        // Register the SqlLearningSpaceLog implementation for ILearningSpaceLogRepository interface
        services.AddTransient<ILearningSpaceLogRepository, SqlLearningSpaceLogRepository>();
    }

    /// <summary>
    /// Tests that ListLearningSpaceLogsAsync returns all learning space logs in the correct order.
    /// </summary>
    [Fact]
    public async Task ListLearningSpaceLogsAsync_WithData_ReturnsAllLogsOrderedByModifiedAtDescending()
    {
        // Act
        var result = await _learningSpaceLogServices.ListLearningSpaceLogsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);

        // Verify logs are ordered by ModifiedAt descending (most recent first)
        result[0].Name.Should().Be("Classroom 201");
        result[0].Action.Should().Be("Created");
        result[1].Name.Should().Be("Lab 101");
        result[1].Action.Should().Be("Updated");
        result[2].Name.Should().Be("Lab 101");
        result[2].Action.Should().Be("Created");
    }

    /// <summary>
    /// Tests that ListLearningSpaceLogsAsync returns correct properties for each log entry.
    /// </summary>
    [Fact]
    public async Task ListLearningSpaceLogsAsync_WithData_ReturnsLogsWithCorrectProperties()
    {
        // Act
        var result = await _learningSpaceLogServices.ListLearningSpaceLogsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);

        var firstLog = result.First(l => l.Action == "Created" && l.Name == "Lab 101");
        firstLog.Should().NotBeNull();
        firstLog.LearningSpaceLogInternalId.Should().Be(1);
        firstLog.Name.Should().Be("Lab 101");
        firstLog.Type.Should().Be("Laboratory");
        firstLog.MaxCapacity.Should().Be(30);
        firstLog.Width.Should().Be(5.0m);
        firstLog.Height.Should().Be(3.0m);
        firstLog.Length.Should().Be(6.0m);
        firstLog.ColorFloor.Should().Be("White");
        firstLog.ColorWalls.Should().Be("Blue");
        firstLog.ColorCeiling.Should().Be("Gray");
        firstLog.Action.Should().Be("Created");
    }

    /// <summary>
    /// Tests that ListLearningSpaceLogsAsync correctly handles updated entries.
    /// </summary>
    [Fact]
    public async Task ListLearningSpaceLogsAsync_WithUpdatedEntry_ReturnsUpdatedLogWithCorrectData()
    {
        // Act
        var result = await _learningSpaceLogServices.ListLearningSpaceLogsAsync();

        // Assert
        result.Should().NotBeNull();

        var updatedLog = result.First(l => l.Action == "Updated");
        updatedLog.Should().NotBeNull();
        updatedLog.LearningSpaceLogInternalId.Should().Be(2);
        updatedLog.Name.Should().Be("Lab 101");
        updatedLog.Type.Should().Be("Laboratory");
        updatedLog.MaxCapacity.Should().Be(35); // Updated capacity
        updatedLog.Action.Should().Be("Updated");
    }

    /// <summary>
    /// Tests that ListLearningSpaceLogsAsync returns empty list when no logs exist.
    /// </summary>
    [Fact]
    public async Task ListLearningSpaceLogsAsync_WithNoData_ReturnsEmptyList()
    {
        // Arrange - Create a fresh context with no data
        var emptyDatabaseName = Guid.NewGuid().ToString();
        var emptyOptions = new DbContextOptionsBuilder<ThemeParkDataBaseContext>()
            .UseInMemoryDatabase(databaseName: emptyDatabaseName)
            .Options;

        var emptyContext = new ThemeParkDataBaseContext(emptyOptions);

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<ThemeParkDataBaseContext>(contextOptions =>
            contextOptions.UseInMemoryDatabase(databaseName: emptyDatabaseName));
        AddInfrastructureServicesForTesting(services);
        services.AddApplicationLayerServices();

        var serviceProvider = services.BuildServiceProvider();
        var emptyLearningSpaceLogServices = serviceProvider.GetRequiredService<ILearningSpaceLogServices>();

        // Act
        var result = await emptyLearningSpaceLogServices.ListLearningSpaceLogsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that ListLearningSpaceLogsAsync handles different action types correctly.
    /// </summary>
    [Fact]
    public async Task ListLearningSpaceLogsAsync_WithDifferentActions_ReturnsLogsWithCorrectActionTypes()
    {
        // Act
        var result = await _learningSpaceLogServices.ListLearningSpaceLogsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);

        var actions = result.Select(l => l.Action).ToList();
        actions.Should().Contain("Created");
        actions.Should().Contain("Updated");
        actions.Count(a => a == "Created").Should().Be(2);
        actions.Count(a => a == "Updated").Should().Be(1);
    }

    /// <summary>
    /// Tests that ListLearningSpaceLogsAsync properly handles different learning space types.
    /// </summary>
    [Fact]
    public async Task ListLearningSpaceLogsAsync_WithDifferentTypes_ReturnsLogsWithCorrectTypes()
    {
        // Act
        var result = await _learningSpaceLogServices.ListLearningSpaceLogsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);

        var types = result.Select(l => l.Type).Distinct().ToList();
        types.Should().Contain("Laboratory");
        types.Should().Contain("Classroom");
        types.Should().HaveCount(2);
    }

    /// <summary>
    /// Tests that ListLearningSpaceLogsAsync returns logs with valid timestamps.
    /// </summary>
    [Fact]
    public async Task ListLearningSpaceLogsAsync_WithData_ReturnsLogsWithValidTimestamps()
    {
        // Act
        var result = await _learningSpaceLogServices.ListLearningSpaceLogsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);

        foreach (var log in result)
        {
            log.ModifiedAt.Should().NotBe(default(DateTime));
            log.ModifiedAt.Should().BeBefore(DateTime.UtcNow.AddMinutes(1)); // Allow for small time differences
        }

        // Verify ordering by ModifiedAt descending
        for (int i = 0; i < result.Count - 1; i++)
        {
            result[i].ModifiedAt.Should().BeOnOrAfter(result[i + 1].ModifiedAt);
        }
    }

    /// <summary>
    /// Tests that ListLearningSpaceLogsAsync returns logs with all required string properties populated.
    /// </summary>
    [Fact]
    public async Task ListLearningSpaceLogsAsync_WithData_ReturnsLogsWithAllRequiredProperties()
    {
        // Act
        var result = await _learningSpaceLogServices.ListLearningSpaceLogsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);

        foreach (var log in result)
        {
            log.Name.Should().NotBeNullOrWhiteSpace();
            log.Type.Should().NotBeNullOrWhiteSpace();
            log.ColorFloor.Should().NotBeNullOrWhiteSpace();
            log.ColorWalls.Should().NotBeNullOrWhiteSpace();
            log.ColorCeiling.Should().NotBeNullOrWhiteSpace();
            log.Action.Should().NotBeNullOrWhiteSpace();
            log.MaxCapacity.Should().BeGreaterThan(0);
            log.Width.Should().BeGreaterThan(0);
            log.Height.Should().BeGreaterThan(0);
            log.Length.Should().BeGreaterThan(0);
        }
    }

    /// <summary>
    /// Integration test that validates service behavior with a more complex scenario.
    /// </summary>
    [Fact]
    public async Task ListLearningSpaceLogsAsync_IntegrationTest_WorksCorrectly()
    {
        // Act
        var result = await _learningSpaceLogServices.ListLearningSpaceLogsAsync();

        // Assert - Verify complete integration
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<List<LearningSpaceLog>>();
        result.Should().HaveCount(3);

        // Verify the service correctly delegates to repository and returns expected data
        var mostRecentLog = result.First();
        mostRecentLog.Name.Should().Be("Classroom 201");
        mostRecentLog.Action.Should().Be("Created");

        // Verify all logs have sequential internal IDs
        var internalIds = result.Select(l => l.LearningSpaceLogInternalId).OrderBy(id => id).ToList();
        internalIds.Should().Equal(1, 2, 3);
    }
}
