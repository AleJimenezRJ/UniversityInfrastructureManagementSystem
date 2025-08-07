using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Spaces;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Integration.Repositories.Spaces;

[Collection("Database collection")]
public class SqlLearningSpaceLogRepositoryIntegrationTests
{
    private readonly IServiceProvider _serviceProvider;

    public SqlLearningSpaceLogRepositoryIntegrationTests(IntegrationTestFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }

    /// <summary>
    /// Tests that ListLearningSpaceLogsAsync returns logs ordered by most recent modification.
    /// </summary>
    [Fact]
    public async Task ListLearningSpaceLogsAsync_Should_Return_Logs_Ordered_By_ModifiedAt()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<ILearningSpaceLogRepository>();

        // Arrange: Insert logs with different ModifiedAt
        var now = DateTime.UtcNow;
        var log1 = new LearningSpaceLog
        {
            Name = "Aula 1",
            Type = "Classroom",
            MaxCapacity = 30,
            Width = 5,
            Height = 3,
            Length = 7,
            ColorFloor = "Blue",
            ColorWalls = "White",
            ColorCeiling = "White",
            ModifiedAt = now.AddMinutes(-10),
            Action = "Created"
        };
        var log2 = new LearningSpaceLog
        {
            Name = "Lab 2",
            Type = "Laboratory",
            MaxCapacity = 20,
            Width = 6,
            Height = 3,
            Length = 8,
            ColorFloor = "Gray",
            ColorWalls = "White",
            ColorCeiling = "White",
            ModifiedAt = now,
            Action = "Updated"
        };
        context.LearningSpaceLog.AddRange(log1, log2);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.ListLearningSpaceLogsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Lab 2", result[0].Name); // Most recent first
        Assert.Equal("Aula 1", result[1].Name);
    }

    /// <summary>
    /// Tests that ListLearningSpaceLogsAsync returns an empty list when there are no logs.
    /// </summary>
    [Fact]
    public async Task ListLearningSpaceLogsAsync_Should_Return_Empty_When_No_Logs()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<ILearningSpaceLogRepository>();

        // Ensure no logs exist
        context.LearningSpaceLog.RemoveRange(context.LearningSpaceLog);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.ListLearningSpaceLogsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
