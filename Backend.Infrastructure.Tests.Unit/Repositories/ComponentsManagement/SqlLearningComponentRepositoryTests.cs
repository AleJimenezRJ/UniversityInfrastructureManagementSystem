using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.ComponentsManagement;

/// <summary>
/// Unit tests for <see cref="SqlLearningComponentRepository"/> covering CRUD operations,
/// pagination, and filtering for learning components in the infrastructure layer.
/// </summary>
public class SqlLearningComponentRepositoryTests
{
    // Repository under test
    private readonly SqlLearningComponentRepository _repository;

    // In-memory database context used for testing
    private readonly ThemeParkDataBaseContext _context;

    // Sample domain objects used in tests
    private readonly Whiteboard _whiteboard;
    private readonly Projector _projector;

    /// <summary>
    /// Default page size used when testing pagination-related methods.
    /// </summary>
    private readonly int _pageSize;

    /// <summary>
    /// Default page index used when testing pagination-related methods.
    /// </summary>
    private readonly int _pageIndex;

    /// <summary>
    /// String used to filter components.
    /// </summary>
    private readonly string _stringSearch;

    /// <summary>
    /// Initializes the test class, setting up the in-memory database, test data,
    /// and repository instance with mocked dependencies.
    /// </summary>
    public SqlLearningComponentRepositoryTests()
    {
        _pageSize = 10;
        _pageIndex = 0;
        _stringSearch = string.Empty;
        var options = new DbContextOptionsBuilder<ThemeParkDataBaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ThemeParkDataBaseContext(options);

        // Pre-seed learning space
        _context.LearningSpaces.Add(new LearningSpace(
            1,
            EntityName.Create("Lab 101"),
            LearningSpaceType.Create("Laboratory"),
            Capacity.Create(30),
            Size.Create(3.0),
            Size.Create(5.0),
            Size.Create(6.0),
            Colors.Create("White"),
            Colors.Create("Blue"),
            Colors.Create("Gray")
        ));

        // Sample components
        _whiteboard = new Whiteboard
        {
            MarkerColor = Colors.Create("blue"),
            ComponentId = 1,
            Orientation = Orientation.Create("North"),
            Dimensions = Dimension.Create(120, 80, 2),
            Position = Coordinates.Create(3, 10, 20),
            DisplayId = "WB-1"
        };

        _projector = new Projector
        {
            ProjectedContent = "sample content",
            ProjectionArea = Area2D.Create(100, 75),
            ComponentId = 2,
            Orientation = Orientation.Create("North"),
            Position = Coordinates.Create(3, 10, 20),
            Dimensions = Dimension.Create(120, 80, 2),
            DisplayId = "PROJ-2"
        };

        _context.SaveChanges();

        var mockLogger = new Mock<ILogger<SqlLearningComponentRepository>>();
        _repository = new SqlLearningComponentRepository(_context, mockLogger.Object);
    }

    /// <summary>
    /// Verifies that retrieving components from an empty database returns an empty list.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_WhenNoComponents_ReturnsEmptyList()
    {
        // Act
        var result = await _repository.GetAllAsync(_pageSize, _pageIndex);

        // Assert
        result.Should().NotBeNull(because: "the repository should return an empty list, not null, when no components exist");
        result.Should().BeEmpty(because: "no components were added to the database");
    }

    /// <summary>
    /// Verifies that all non-deleted components are retrieved when they exist in the database.
    /// </summary>
    [Fact]
    public async Task GetAllAsync_WhenComponentsExist_ReturnsAllNotDeletedComponents()
    {
        // Arrange
        await _repository.AddComponentAsync(1, _whiteboard);
        await _repository.AddComponentAsync(1, _projector);

        // Act
        var result = await _repository.GetAllAsync(_pageSize, _pageIndex);

        // Assert
        result.Should().NotBeNull(because: "the repository should return a list of components");
        result.Should().HaveCount(2, because: "two components were added to the database");
        result.All(c => !c.IsDeleted).Should().BeTrue(because: "only components that are not marked as deleted should be returned");
    }

    /// <summary>
    /// Verifies retrieval of a single non-deleted component by ID.
    /// </summary>
    [Fact]
    public async Task GetSingleLearningComponentAsync_WhenComponentExistsAndNotDeleted_ReturnsComponent()
    {
        // Arrange
        await _repository.AddComponentAsync(1, _whiteboard);
        var inserted = await _context.LearningComponents.FirstAsync();

        // Act
        var result = await _repository.GetSingleLearningComponentAsync(inserted.ComponentId);

        // Assert
        result.Should().NotBeNull(because: "the component exists and is not deleted");
        result.ComponentId.Should().Be(inserted.ComponentId, because: "the correct component should be returned by ID");
    }

    /// <summary>
    /// Verifies that a NotFoundException is thrown when attempting to retrieve a non-existent component.
    /// </summary>
    [Fact]
    public async Task GetSingleLearningComponentAsync_WhenComponentDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        int nonExistentId = 999;

        // Act
        Func<Task> act = async () => await _repository.GetSingleLearningComponentAsync(nonExistentId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Component with ID '{nonExistentId}' not found.");
    }

    /// <summary>
    /// Ensures that components are returned when the learning space exists
    /// and two components have been previously added to it.
    /// </summary>
    [Fact]
    public async Task GetLearningComponentsByIdAsync_ShouldReturnComponents_WhenLearningSpaceExists()
    {
        // Arrange
        await _repository.AddComponentAsync(1, _whiteboard);
        await _repository.AddComponentAsync(1, _projector);

        // Act
        var components = await _repository.GetLearningComponentsByIdAsync(1, _pageSize, _pageIndex, _stringSearch);

        // Assert
        components.Should().NotBeNull(because: "components should be returned when the learning space exists");
        components.Should().HaveCount(2, because: "two components were added to the learning space");
    }

    /// <summary>
    /// Ensures that components marked as deleted are not returned
    /// when fetching components by learning space ID.
    /// </summary>
    [Fact]
    public async Task GetLearningComponentsByIdAsync_ShouldNotReturnDeletedComponents()
    {
        // Arrange
        await _repository.AddComponentAsync(1, _whiteboard);
        await _repository.AddComponentAsync(1, _projector);

        // Manually mark one component as deleted
        var component = await _context.LearningComponents.FirstAsync();
        component.IsDeleted = true;
        await _context.SaveChangesAsync();

        // Act
        var components = await _repository.GetLearningComponentsByIdAsync(1, _pageSize, _pageIndex, _stringSearch);

        // Assert
        components.All(c => !c.IsDeleted).Should().BeTrue(because: "only components not marked as deleted should be returned");
    }

    /// <summary>
    /// Ensures that the specific components (whiteboard and projector)
    /// added to the learning space are included in the result.
    /// </summary>
    [Fact]
    public async Task GetLearningComponentsByIdAsync_ShouldContainSpecificComponents()
    {
        // Arrange
        await _repository.AddComponentAsync(1, _whiteboard);
        await _repository.AddComponentAsync(1, _projector);

        // Act
        var components = await _repository.GetLearningComponentsByIdAsync(1, _pageSize, _pageIndex, _stringSearch);

        // Assert
        components.Should().Contain(c => c.ComponentId == _whiteboard.ComponentId, because: "the whiteboard was added and should be included");
        components.Should().Contain(c => c.ComponentId == _projector.ComponentId, because: "the projector was added and should be included");
    }


    /// <summary>
    /// Verifies that querying a non-existent learning space returns an empty list.
    /// </summary>
    [Fact]
    public async Task GetLearningComponentsByIdAsync_WhenLearningSpaceDoesNotExist_ReturnsEmptyCollection()
    {
        // Act
        var components = await _repository.GetLearningComponentsByIdAsync(999, _pageSize, _pageIndex, _stringSearch);

        // Assert
        components.Should().NotBeNull(because: "the result should not be null even if no learning space matches");
        components.Should().BeEmpty(because: "no components should be returned when the learning space does not exist");
    }

    /// <summary>
    /// Verifies that a component is successfully added to a valid learning space.
    /// </summary>
    [Fact]
    public async Task AddLearningComponentAsync_WhenComponentIsAdded_ShouldReturnTrue()
    {
        // Act
        var result = await _repository.AddComponentAsync(1, _whiteboard);

        // Assert
        result.Should().BeTrue(because: "the component was added successfully to a valid learning space");
    }

    /// <summary>
    /// Verifies that a component is correctly inserted into the database.
    /// </summary>
    [Fact]
    public async Task AddLearningComponentAsync_WithValidLearningSpaceId_AddsComponentSuccessfully()
    {
        // Act
        await _repository.AddComponentAsync(1, _whiteboard);

        // Assert
        var added = await _context.LearningComponents
            .FirstOrDefaultAsync(c =>
                EF.Property<int>(c, "LearningSpaceId") == 1);

        added.Should().NotBeNull(because: "the component should have been added to the database");
    }

    /// <summary>
    /// Verifies that a component update returns true when both component and learning space exist.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_WhenLearningSpaceAndComponentExist_ShouldReturnTrue()
    {
        // Arrange
        await _repository.AddComponentAsync(1, _whiteboard);
        var updatedComponent = new Whiteboard
        {
            ComponentId = _whiteboard.ComponentId,
            MarkerColor = _whiteboard.MarkerColor,
            Orientation = Orientation.Create("South"),
            Dimensions = _whiteboard.Dimensions,
            Position = _whiteboard.Position,
            IsDeleted = false
        };

        // Act
        var result = await _repository.UpdateAsync(1, _whiteboard.ComponentId, updatedComponent);

        // Assert
        result.Should().BeTrue(because: "the update should succeed if the learning space and component exist");
    }

    /// <summary>
    /// Verifies that updated component values are correctly persisted in the database.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_WhenComponentIsUpdated_ShouldReflectChangesInDatabase()
    {
        // Arrange
        await _repository.AddComponentAsync(1, _whiteboard);
        var updatedComponent = new Whiteboard
        {
            ComponentId = _whiteboard.ComponentId,
            MarkerColor = _whiteboard.MarkerColor,
            Orientation = Orientation.Create("South"),
            Dimensions = _whiteboard.Dimensions,
            Position = _whiteboard.Position,
            IsDeleted = false
        };

        await _repository.UpdateAsync(1, _whiteboard.ComponentId, updatedComponent);

        // Act
        var componentInDb = await _context.LearningComponents
            .FirstOrDefaultAsync(c => c.ComponentId == _whiteboard.ComponentId);

        // Assert
        componentInDb.Should().NotBeNull(because: "the component should still exist in the database after update");
        componentInDb!.Orientation.Should().Be(updatedComponent.Orientation, because: "the component's orientation was updated");
    }

    /// <summary>
    /// Verifies that update returns false when the learning space does not exist.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_WhenLearningSpaceDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var updatedComponent = _whiteboard;

        // Act
        var result = await _repository.UpdateAsync(999, _whiteboard.ComponentId, updatedComponent);

        // Assert
        result.Should().BeFalse(because: "the learning space with ID 999 does not exist");
    }

    /// <summary>
    /// Verifies that update returns false when the component does not exist in the given learning space.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_WhenComponentDoesNotExistInLearningSpace_ShouldReturnFalse()
    {
        // Arrange
        await _repository.AddComponentAsync(1, _whiteboard);
        var updatedComponent = _whiteboard;

        // Act
        var result = await _repository.UpdateAsync(1, 999, updatedComponent);

        // Assert
        result.Should().BeFalse(because: "the component with ID 999 does not exist in the learning space");
    }

    /// <summary>
    /// Verifies that the component's IsDeleted flag is set to true
    /// after it has been deleted.
    /// </summary>
    [Fact]
    public async Task DeleteLearningComponentAsync_InDatabase_ShouldMarkComponentAsDeleted()
    {
        // Arrange
        await _repository.AddComponentAsync(1, _whiteboard);
        var insertedComponent = await _context.LearningComponents.FirstAsync();

        // Act
        await _repository.DeleteComponentAsync(insertedComponent.ComponentId);
        var deleted = await _context.LearningComponents.FindAsync(insertedComponent.ComponentId);

        // Assert
        deleted.Should().NotBeNull(because: "the component should still be retrievable after deletion");
        deleted!.IsDeleted.Should().BeTrue(because: "the deletion should mark the component as deleted");
    }

    /// <summary>
    /// Verifies that the DeleteComponentAsync method returns true
    /// when deleting a component that exists.
    /// </summary>
    [Fact]
    public async Task DeleteLearningComponentAsync_WhenComponentExists_ShouldReturnTrue()
    {
        // Arrange
        await _repository.AddComponentAsync(1, _whiteboard);
        var insertedComponent = await _context.LearningComponents.FirstAsync();

        // Act
        var result = await _repository.DeleteComponentAsync(insertedComponent.ComponentId);

        // Assert
        result.Should().BeTrue(because: "the component exists and should be marked as deleted");
    }

    /// <summary>
    /// Verifies that deletion returns false when the component ID does not exist.
    /// </summary>
    [Fact]
    public async Task DeleteLearningComponentAsync_WhenNotExists_ShouldReturnFalse()
    {
        // Act
        var result = await _repository.DeleteComponentAsync(999);

        // Assert
        result.Should().BeFalse(because: "no component exists with ID 999, so nothing should be deleted");
    }

    /// <summary>
    /// Tests that <see cref="SqlLearningComponentRepository.GetSingleLearningComponentAsync(int)"/>
    /// throws a <see cref="NotFoundException"/> and logs an error when the database fails.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task GetSingleLearningComponentAsync_WhenDbFails_ShouldLogErrorAndThrowNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ThemeParkDataBaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var failingDbContext = new FailingDbContext(options);
        var mockLogger = new Mock<ILogger<SqlLearningComponentRepository>>();
        var repository = new SqlLearningComponentRepository(failingDbContext, mockLogger.Object);

        // Act
        Func<Task> act = async () => await repository.GetSingleLearningComponentAsync(999);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Component with ID '999' not found.");

        mockLogger.Verify(
            l => l.Log(
                It.Is<LogLevel>(level => level == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="SqlLearningComponentRepository.GetLearningComponentsByIdAsync(int, int, int)"/>
    /// returns an empty collection and logs an error when the database fails during retrieval.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task GetLearningComponentsByIdAsync_WhenDbFails_ShouldLogErrorAndReturnEmpty()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ThemeParkDataBaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        // Add a valid LearningSpace before simulating failure
        using (var context = new ThemeParkDataBaseContext(options))
        {
            context.LearningSpaces.Add(new LearningSpace(
                1,
                EntityName.Create("Lab 101"),
                LearningSpaceType.Create("Laboratory"),
                Capacity.Create(10),
                Size.Create(1), Size.Create(1), Size.Create(1),
                Colors.Create("white"), Colors.Create("black"), Colors.Create("gray")
            ));
            await context.SaveChangesAsync();
        }

        var failingContext = new FailingDbContext(options);
        var mockLogger = new Mock<ILogger<SqlLearningComponentRepository>>();
        var repository = new SqlLearningComponentRepository(failingContext, mockLogger.Object);

        // Act
        var result = await repository.GetLearningComponentsByIdAsync(1, 10, 0, "");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty("because an exception occurred and the method catches it returning an empty list");

        mockLogger.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("Error retrieving components")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="SqlLearningComponentRepository.AddComponentAsync(int, LearningComponent)"/>
    /// returns false and logs an error when the database fails during addition.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task AddComponentAsync_WhenDbFails_ShouldLogErrorAndReturnFalse()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ThemeParkDataBaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        // Prepare a valid LearningSpace to associate with the new component
        using var workingContext = new ThemeParkDataBaseContext(options);
        workingContext.LearningSpaces.Add(new LearningSpace(
            1,
            EntityName.Create("Lab 101"),
            LearningSpaceType.Create("Laboratory"),
            Capacity.Create(10),
            Size.Create(1), Size.Create(1), Size.Create(1),
            Colors.Create("white"), Colors.Create("black"), Colors.Create("gray")
        ));
        await workingContext.SaveChangesAsync();

        var whiteboard = new Whiteboard
        {
            MarkerColor = Colors.Create("blue"),
            Orientation = Orientation.Create("North"),
            Dimensions = Dimension.Create(100, 80, 5),
            Position = Coordinates.Create(1, 2, 3),
        };

        var failingContext = new FailingDbContext(options);
        var mockLogger = new Mock<ILogger<SqlLearningComponentRepository>>();
        var repository = new SqlLearningComponentRepository(failingContext, mockLogger.Object);

        // Act
        var result = await repository.AddComponentAsync(1, whiteboard);

        // Assert
        result.Should().BeFalse("because the DbContext throws when accessing the LearningComponents set");

        mockLogger.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("Error adding component")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="SqlLearningComponentRepository.UpdateAsync(int, int, LearningComponent)"/>
    /// returns false and logs an error when the database fails during update.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task UpdateAsync_WhenDbFails_ShouldLogErrorAndReturnFalse()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ThemeParkDataBaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        // Insert valid LearningSpace and component data
        using (var context = new ThemeParkDataBaseContext(options))
        {
            var learningSpace = new LearningSpace(
                1,
                EntityName.Create("Lab 101"),
                LearningSpaceType.Create("Laboratory"),
                Capacity.Create(10),
                Size.Create(1), Size.Create(1), Size.Create(1),
                Colors.Create("white"), Colors.Create("black"), Colors.Create("gray")
            );
            await context.LearningSpaces.AddAsync(learningSpace);

            var whiteboard = new Whiteboard
            {
                ComponentId = 1,
                MarkerColor = Colors.Create("blue"),
                Orientation = Orientation.Create("North"),
                Dimensions = Dimension.Create(100, 80, 5),
                Position = Coordinates.Create(1, 2, 3),
            };
            whiteboard.DisplayId = "WBD-001";
            context.LearningComponents.Add(whiteboard);

            await context.SaveChangesAsync();
        }

        var failingContext = new FailingDbContext(options);
        var mockLogger = new Mock<ILogger<SqlLearningComponentRepository>>();
        var repository = new SqlLearningComponentRepository(failingContext, mockLogger.Object);

        var componentToUpdate = new Whiteboard
        {
            ComponentId = 1,
            MarkerColor = Colors.Create("green"),
            Orientation = Orientation.Create("East"),
            Dimensions = Dimension.Create(100, 80, 5),
            Position = Coordinates.Create(1, 2, 3),
            IsDeleted = false
        };

        // Act
        var result = await repository.UpdateAsync(1, 1, componentToUpdate);

        // Assert
        result.Should().BeFalse("because the database context throws an exception when accessed");

        mockLogger.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("Error updating component")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

}

internal class FailingDbContext : ThemeParkDataBaseContext
{
    public FailingDbContext(DbContextOptions<ThemeParkDataBaseContext> options)
        : base(options)
    {
    }

    public override DbSet<LearningComponent> LearningComponents
    {
        get
        {
            throw new Exception("DB failure");
        }
    }
}