using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using Xunit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.Spaces;

/// <summary>
/// Unit tests for the <see cref="SqlLearningSpaceLogRepository"/> class, using an in-memory EF Core context.
/// </summary>
public class SqlLearningSpaceLogRepositoryTests
{
    private readonly SqlLearningSpaceLogRepository _repository;
    private readonly ThemeParkDataBaseContext _context;
    private readonly LearningSpaceLog _log1;
    private readonly LearningSpaceLog _log2;
    private readonly ILogger<SqlLearningSpaceLogRepository> _logger;

    public SqlLearningSpaceLogRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ThemeParkDataBaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new ThemeParkDataBaseContext(options);
        _logger = new Mock<ILogger<SqlLearningSpaceLogRepository>>().Object;
        _repository = new SqlLearningSpaceLogRepository(_context, _logger);

        _log1 = new LearningSpaceLog
        {
            LearningSpaceLogInternalId = 1,
            Name = "Aula 1",
            Type = "Classroom",
            MaxCapacity = 30,
            Width = 5,
            Height = 3,
            Length = 7,
            ColorFloor = "Blue",
            ColorWalls = "White",
            ColorCeiling = "Gray",
            ModifiedAt = new DateTime(2024, 5, 1, 10, 0, 0, DateTimeKind.Utc),
            Action = "Created"
        };
        _log2 = new LearningSpaceLog
        {
            LearningSpaceLogInternalId = 2,
            Name = "Aula 2",
            Type = "Laboratory",
            MaxCapacity = 20,
            Width = 6,
            Height = 3,
            Length = 8,
            ColorFloor = "Green",
            ColorWalls = "Gray",
            ColorCeiling = "White",
            ModifiedAt = new DateTime(2024, 5, 2, 12, 0, 0, DateTimeKind.Utc),
            Action = "Updated"
        };
    }

    /// <summary>
    /// Ensures that ListLearningSpaceLogsAsync returns logs ordered by ModifiedAt descending.
    /// </summary>
    [Fact]
    public async Task ListLearningSpaceLogsAsync_WhenLogsExist_ReturnsOrderedLogs()
    {
        // Arrange
        _context.LearningSpaceLog.AddRange(_log1, _log2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ListLearningSpaceLogsAsync();

        // Assert
        result.Should().HaveCount(2);
        result[0].LearningSpaceLogInternalId.Should().Be(_log2.LearningSpaceLogInternalId);
        result[1].LearningSpaceLogInternalId.Should().Be(_log1.LearningSpaceLogInternalId);
    }

    /// <summary>
    /// Ensures that ListLearningSpaceLogsAsync returns an empty list when there are no logs.
    /// </summary>
    [Fact]
    public async Task ListLearningSpaceLogsAsync_WhenNoLogs_ReturnsEmptyList()
    {
        // Act
        var result = await _repository.ListLearningSpaceLogsAsync();

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Ensures that ListLearningSpaceLogsAsync returns an empty list if an exception occurs.
    /// </summary>
    [Fact]
    public async Task ListLearningSpaceLogsAsync_WhenExceptionOccurs_ReturnsEmptyList()
    {
        // Arrange
        var mockContext = new Mock<ThemeParkDataBaseContext>(new DbContextOptionsBuilder<ThemeParkDataBaseContext>().Options);
        var mockSet = new Mock<DbSet<LearningSpaceLog>>();
        var queryable = new List<LearningSpaceLog>().AsQueryable();
        mockSet.As<IQueryable<LearningSpaceLog>>().Setup(m => m.Provider).Throws(new Exception("DB error"));
        mockSet.As<IQueryable<LearningSpaceLog>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<LearningSpaceLog>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<LearningSpaceLog>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
        mockContext.Setup(c => c.LearningSpaceLog).Returns(mockSet.Object);
        var repo = new SqlLearningSpaceLogRepository(mockContext.Object, _logger);

        // Act
        var result = await repo.ListLearningSpaceLogsAsync();

        // Assert
        result.Should().BeEmpty();
    }
}
