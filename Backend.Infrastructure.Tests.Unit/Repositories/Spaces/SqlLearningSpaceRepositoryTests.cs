using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.Spaces;


/// <summary>
/// Unit tests for the <see cref="SqlLearningSpaceRepository"/> class, using an in-memory EF Core context.
/// </summary>
public class SqlLearningSpaceRepositoryTests
{
    /// <summary>
    /// Repository under test.
    /// </summary>
    private readonly SqlLearningSpaceRepository _repository;

    /// <summary>
    /// In-memory database context used for testing.
    /// </summary>
    private readonly ThemeParkDataBaseContext _context;

    /// <summary>
    /// Sample floor entity used for test setup.
    /// </summary>
    private readonly Floor _floor;

    /// <summary>
    /// Sample learning space entity used for test setup and assertions.
    /// </summary>
    private readonly LearningSpace _learningSpace;

    /// <summary>
    /// Index of the second learning space in tests, used for assertions and setup.
    /// </summary>
    private const int _learningSpaceExpectedIndex = 0;

    /// <summary>
    /// Sample learning space entity used for test setup and assertions.
    /// </summary>
    private readonly LearningSpace _learningSpace2;

    /// <summary>
    /// Sample learning space entity used for test create method tests.
    /// </summary>
    private readonly LearningSpace _learningSpaceToCreate;

    /// <summary>
    /// Size of the page for paginated results in tests.
    /// </summary>
    private const int _testPageSize = 1;

    /// <summary>
    /// Index of the page for paginated results in tests.
    /// </summary>
    private const int _testPageIndex = 0;

    /// <summary>
    /// Test search text used in paginated listing tests to ensure the repository can handle search functionality.
    /// </summary>
    private const string _emptySearchText = " ";

    /// <summary>
    /// Test search text used in paginated listing tests to ensure the repository can handle search functionality.
    /// </summary>
    private const string _exampleSearchText = "Physics Lab";

    /// <summary>
    /// Total count of learning spaces expected in tests, used for assertions.
    /// </summary>
    private const int _testTotalCount = 2;

    /// <summary>
    /// Initializes the test class by setting up the in-memory database, a test floor,
    /// a sample learning space, and the repository instance.
    /// </summary>
    public SqlLearningSpaceRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ThemeParkDataBaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ThemeParkDataBaseContext(options);

        _floor = new Floor(FloorNumber.Create(1));
        _context.Floors.Add(_floor);

        _learningSpace = new LearningSpace(
            EntityName.Create("Physics Lab"),
            LearningSpaceType.Create("Laboratory"),
            Capacity.Create(30),
            Size.Create(90),
            Size.Create(100),
            Size.Create(80),
            Colors.Create("Blue"),
            Colors.Create("Gray"),
            Colors.Create("White")
        );


        _learningSpace2 = new LearningSpace(
            EntityName.Create("Chemistry Lab"),
            LearningSpaceType.Create("Laboratory"),
            Capacity.Create(25),
            Size.Create(80),
            Size.Create(90),
            Size.Create(85),
            Colors.Create("Green"),
            Colors.Create("White"),
            Colors.Create("Gray")
        );

        _learningSpaceToCreate = new LearningSpace(
            EntityName.Create("Computer Sciences Auditorium"),
            LearningSpaceType.Create("Auditorium"),
            Capacity.Create(25),
            Size.Create(80),
            Size.Create(90),
            Size.Create(85),
            Colors.Create("Green"),
            Colors.Create("White"),
            Colors.Create("Gray")
        );

        // Associate the first learning space with the test floor
        _context.Entry(_learningSpace).Property("FloorId").CurrentValue = _floor.FloorId;
        _context.LearningSpaces.Add(_learningSpace);
        _context.SaveChangesAsync();

        // Associate the second learning space with the test floor
        _context.Entry(_learningSpace2).Property("FloorId").CurrentValue = _floor.FloorId;
        _context.LearningSpaces.Add(_learningSpace2);
        _context.SaveChangesAsync();

        var mockLogger = new Mock<ILogger<SqlLearningSpaceRepository>>();
        _repository = new SqlLearningSpaceRepository(_context, mockLogger.Object);
    }

    /// <summary>
    /// Verifies that an existing learning space is correctly updated when using valid input.
    /// </summary>
    [Fact]
    public async Task UpdateLearningSpaceAsync_WhenExists_UpdatesSuccessfully()
    {
        _context.Entry(_learningSpace).Property("FloorId").CurrentValue = _floor.FloorId;
        _context.SaveChanges();

        var result = await _repository.UpdateLearningSpaceAsync(_learningSpace.LearningSpaceId, _learningSpace);

        result.Should().BeTrue();

        var fromDb = await _context.LearningSpaces.FindAsync(_learningSpace.LearningSpaceId);
        fromDb.Name.Name.Should().Be("Physics Lab");
        fromDb.MaxCapacity.Value.Should().Be(30);
    }

    /// <summary>
    /// Verifies that updating a non-existent learning space throws a <see cref="NotFoundException"/>.
    /// </summary>
    [Fact]
    public async Task UpdateLearningSpaceAsync_WhenNotFound_ThrowsNotFoundException()
    {
        var newSpace = _learningSpace;

        Func<Task> act = () => _repository.UpdateLearningSpaceAsync(999, newSpace);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*not found*");
    }

    /// <summary>
    /// Ensures that a learning space is created successfully when the floor exists.
    /// </summary>
    [Fact]
    public async Task CreateLearningSpaceAsync_WhenFloorExists_ShouldSucceed()
    {

        var result = await _repository.CreateLearningSpaceAsync(_floor.FloorId, _learningSpaceToCreate);

        result.Should().BeTrue("a valid floor exists and creation should succeed");
        _context.LearningSpaces.Count().Should().Be(3);
    }

    /// <summary>
    /// Ensures that creating a learning space for a non-existent floor throws a <see cref="NotFoundException"/>.
    /// </summary>
    [Fact]
    public async Task CreateLearningSpaceAsync_WhenFloorDoesNotExist_ShouldThrowNotFoundException()
    {
        Func<Task> action = async () => await _repository.CreateLearningSpaceAsync(999, _learningSpace);

        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Floor with Id*not found*");
    }

    /// <summary>
    /// Verifies that an existing learning space can be retrieved successfully by its ID.
    /// </summary>
    [Fact]
    public async Task ReadLearningSpaceAsync_WhenExists_ShouldReturnEntity()
    {
        var result = await _repository.ReadLearningSpaceAsync(_learningSpace.LearningSpaceId);

        result.Should().NotBeNull();
        result!.Name.Name.Should().Be("Physics Lab");
        result.MaxCapacity.Value.Should().Be(30);
    }

    /// <summary>
    /// Verifies that reading a non-existent learning space throws a <see cref="NotFoundException"/>.
    /// </summary>
    [Fact]
    public async Task ReadLearningSpaceAsync_WhenDoesNotExist_ShouldThrowNotFoundException()
    {
        Func<Task> action = async () => await _repository.ReadLearningSpaceAsync(9999);

        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*not found*");
    }

    /// <summary>
    /// Confirms that a learning space is deleted successfully when it exists.
    /// </summary>
    [Fact]
    public async Task DeleteLearningSpaceAsync_WhenExists_ShouldDeleteSuccessfully()
    {
        var result = await _repository.DeleteLearningSpaceAsync(_learningSpace.LearningSpaceId);

        result.Should().BeTrue();
        var deleted = await _context.LearningSpaces.FindAsync(_learningSpace.LearningSpaceId);
        deleted.Should().BeNull();
    }

    /// <summary>
    /// Confirms that attempting to delete a non-existent learning space throws a <see cref="NotFoundException"/>.
    /// </summary>
    [Fact]
    public async Task DeleteLearningSpaceAsync_WhenNotFound_ShouldThrowNotFoundException()
    {
        Func<Task> act = () => _repository.DeleteLearningSpaceAsync(999);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*not found*");
    }

    /// <summary>
    /// Ensures that listing learning spaces by a valid floor ID returns the correct result.
    /// </summary>
    [Fact]
    public async Task ListLearningSpacesAsync_WhenFloorExists_ShouldReturnLearningSpaces()
    {
        _context.Entry(_learningSpace).Property("FloorId").CurrentValue = _floor.FloorId;
        _context.SaveChanges();

        var result = await _repository.ListLearningSpacesAsync(_floor.FloorId);
        
        result![0].Name.Name.Should().Be("Physics Lab");
    }

    /// <summary>
    /// Ensures that listing learning spaces for a non-existent floor throws a <see cref="NotFoundException"/>.
    /// </summary>
    [Fact]
    public async Task ListLearningSpacesAsync_WhenFloorDoesNotExist_ShouldThrowNotFoundException()
    {
        Func<Task> act = () => _repository.ListLearningSpacesAsync(999);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*not found*");
    }

    /// <summary>
    /// Verifies that paginated listing of learning spaces for a non-existent floor throws a <see cref="NotFoundException"/>.
    /// </summary>
    [Fact]
    public async Task ListLearningSpacesPaginatedAsync_WhenFloorDoesNotExist_ShouldThrowNotFoundException()
    {
        Func<Task> act = () => _repository.ListLearningSpacesPaginatedAsync(999, pageSize: 5, pageIndex: 0, _emptySearchText);

        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*not found*");
    }

    /// <summary>
    /// Verifies that paginated listing of learning spaces for an existing floor returns the correct paginated result.
    /// </summary>
    [Fact]
    public async Task ListLearningSpacesPaginatedAsync_WhenFloorExists_ShouldReturnPaginatedList()
    {
        //Arrange 

        // Act
        var result = await _repository.ListLearningSpacesPaginatedAsync(_floor.FloorId, _testPageSize, _testPageIndex, _emptySearchText);

        // Assert
        result[_learningSpaceExpectedIndex].LearningSpaceId.Should().Be(_learningSpace2.LearningSpaceId,
        because: "the first learning space in the paginated list should match the expected learning space");
    }

    /// <summary>
    /// Verifies that paginated listing of learning spaces for an existing floor returns the correct total count expected in tests.
    /// </summary>
    [Fact]
    public async Task ListLearningSpacesPaginatedAsync_WhenFloorExists_ShouldReturnExpectedCount()
    {
        //Arrange 

        // Act
        var result = await _repository.ListLearningSpacesPaginatedAsync(_floor.FloorId, _testPageSize, _testPageIndex, _emptySearchText);

        // Assert
        result.TotalCount.Should().Be(_testTotalCount,
            because: "the total count of learning spaces should match the expected count in tests");
    }

    [Fact]
    public async Task ListLearningSpacesPaginatedAsync_WhenFloorExistsAndSearchTextIsProvided_ShouldReturnFilteredList()
    {

        // Act
        var result = await _repository.ListLearningSpacesPaginatedAsync(_floor.FloorId, _testPageSize, _testPageIndex, _exampleSearchText);
        // Assert
        foreach (var space in result)
        {
            space.Name.Name.Should().Contain(_exampleSearchText, "the search text should match the learning space name");
        }
    }

}
