using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.ComponentsManagement;

/// <summary>
/// Unit tests for the <see cref="SqlProjectorRepository"/> class.
/// These tests verify the correct behavior of projector repository methods, including retrieval of all projectors,
/// filtering out deleted projectors, and handling cases where no projectors exist.
/// </summary>
public class SqlProjectorRepositoryTests
{
    private readonly SqlProjectorRepository _repository;

    private readonly ThemeParkDataBaseContext _context;

    private readonly Projector _projector1;

    private readonly Projector _projector2;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlProjectorRepositoryTests"/> class.
    /// Sets up an in-memory database context and seeds it with sample projector data for testing.
    /// </summary>
    public SqlProjectorRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ThemeParkDataBaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ThemeParkDataBaseContext(options);

        _projector1 = new Projector
        {
            ProjectedContent = "sample content 1",
            ProjectionArea = Area2D.Create(110, 55),
            ComponentId = 3,
            Orientation = Orientation.Create("East"),
            Position = Coordinates.Create(2, 6, 88),
            Dimensions = Dimension.Create(111, 22, 3),
            DisplayId = "PROJ-3"
        };

        _projector2 = new Projector
        {
            ProjectedContent = "sample content 2",
            ProjectionArea = Area2D.Create(100, 75),
            ComponentId = 2,
            Orientation = Orientation.Create("North"),
            Position = Coordinates.Create(3, 10, 20),
            Dimensions = Dimension.Create(120, 80, 2),
            DisplayId = "PROJ-2"
        };

        _context.Projectors.AddRange(_projector1, _projector2);
        _context.SaveChanges();

        var mockLogger = new Mock<ILogger<SqlProjectorRepository>>();
        _repository = new SqlProjectorRepository(_context, mockLogger.Object);
    }

    /// <summary>
    /// Verifies that <see cref="SqlProjectorRepository.GetAllAsync"/> does not return null when projectors exist.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_WhenProjectorsExist_ResultIsNotNull()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
    }

    /// <summary>
    /// Verifies that <see cref="SqlProjectorRepository.GetAllAsync"/> returns all expected projectors.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_WhenProjectorsExist_ReturnsAllExpectedProjectors()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().Contain(p => p.ComponentId == _projector1.ComponentId, because: "projector 1 was added and should be returned");
        result.Should().Contain(p => p.ComponentId == _projector2.ComponentId, because: "projector 2 was added and should be returned");
    }

    /// <summary>
    /// Verifies that <see cref="SqlProjectorRepository.GetAllAsync"/> returns the correct count of projectors.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_WhenProjectorsExist_ReturnsCorrectCount()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    /// <summary>
    /// Verifies that <see cref="SqlProjectorRepository.GetAllAsync"/> does not return projectors marked as deleted.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_ShouldNotReturnDeletedProjectors()
    {
        // Arrange
        var projectorToDelete = await _context.Projectors.FirstAsync();
        projectorToDelete.GetType().GetProperty("IsDeleted")?.SetValue(projectorToDelete, true);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.All(c => !c.IsDeleted).Should().BeTrue(because: "only components not marked as deleted should be returned");
    }

    /// <summary>
    /// Verifies that <see cref="SqlProjectorRepository.GetAllAsync"/> returns an empty list when no projectors exist.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_WhenNoProjectorsExist_ReturnsEmptyList()
    {
        // Arrange
        _context.Projectors.RemoveRange(_context.Projectors);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().NotBeNull(because: "an empty list should be returned, not null");
        result.Should().BeEmpty(because: "no projectors were added to the database");
    }
}
