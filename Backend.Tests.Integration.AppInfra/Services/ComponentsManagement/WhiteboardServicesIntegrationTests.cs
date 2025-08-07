using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Application;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AppInfra.Services.ComponentsManagement;

/// <summary>
/// Integration tests for WhiteboardServices that test the interaction between
/// Application and Infrastructure layers for whiteboard operations.
/// </summary>
public class WhiteboardServicesIntegrationTests
{
    private readonly IWhiteboardServices _whiteboardServices;

    public WhiteboardServicesIntegrationTests()
    {
        var databaseName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<ThemeParkDataBaseContext>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options;

        var context = new ThemeParkDataBaseContext(options);

        // Pre-seed learning space for tests
        context.LearningSpaces.Add(new LearningSpace(
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

        context.SaveChanges();

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
        _whiteboardServices = serviceProvider.GetRequiredService<IWhiteboardServices>();
    }

    private static void AddInfrastructureServicesForTesting(IServiceCollection services)
    {
        // Register the SqlLearningComponent implementation for ILearningComponentRepository interface
        services.AddTransient<ILearningComponentRepository, SqlLearningComponentRepository>();
        
        // Register the SqlLearningSpace implementation for ILearningSpaceRepository interface
        services.AddTransient<ILearningSpaceRepository, SqlLearningSpaceRepository>();
        
        // Add other necessary repositories without DbContext registration
        services.AddTransient<IWhiteboardRepository, SqlWhiteboardRepository>();
        services.AddTransient<IProjectorRepository, SqlProjectorRepository>();
    }

    /// <summary>
    /// Tests that GetWhiteboardAsync returns all whiteboards from the database.
    /// </summary>
    [Fact]
    public async Task GetWhiteboardAsync_ReturnsAllWhiteboards()
    {
        // Act
        var result = await _whiteboardServices.GetWhiteboardAsync();

        // Assert
        var whiteboards = result.ToList();
        whiteboards.Should().NotBeNull();
        whiteboards.Should().BeAssignableTo<IEnumerable<Whiteboard>>();
    }

    /// <summary>
    /// Tests that AddWhiteboardAsync successfully adds a whiteboard to the database.
    /// </summary>
    [Fact]
    public async Task AddWhiteboardAsync_WithValidWhiteboard_ReturnsTrue()
    {
        // Arrange
        var whiteboard = new Whiteboard
        {
            MarkerColor = Colors.TryCreate("Red", out var color) ? color! : throw new InvalidOperationException("Invalid color"),
            Orientation = Orientation.TryCreate("North", out var orientation) ? orientation! : throw new InvalidOperationException("Invalid orientation"),
            Position = Coordinates.Create(10, 10, 0),
            Dimensions = Dimension.Create(3, 2, 0.1),
            DisplayId = "WB-"
        };

        // Act
        var result = await _whiteboardServices.AddWhiteboardAsync(1, whiteboard);

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Tests that AddWhiteboardAsync persists the whiteboard to the database correctly.
    /// </summary>
    [Fact]
    public async Task AddWhiteboardAsync_WithValidWhiteboard_PersistsToDatabase()
    {
        // Arrange
        var whiteboard = new Whiteboard
        {
            MarkerColor = Colors.TryCreate("Blue", out var color) ? color! : throw new InvalidOperationException("Invalid color"),
            Orientation = Orientation.TryCreate("South", out var orientation) ? orientation! : throw new InvalidOperationException("Invalid orientation"),
            Position = Coordinates.Create(20, 20, 0),
            Dimensions = Dimension.Create(4, 3, 0.15),
            DisplayId = "WB-"
        };

        // Act
        await _whiteboardServices.AddWhiteboardAsync(1, whiteboard);

        // Assert
        var allWhiteboards = await _whiteboardServices.GetWhiteboardAsync();
        allWhiteboards.Should().Contain(w => w.MarkerColor!.Value == "Blue");
    }

    /// <summary>
    /// Tests that AddWhiteboardAsync throws NotFoundException with invalid learning space ID.
    /// </summary>
    [Fact]
    public async Task AddWhiteboardAsync_WithInvalidLearningSpaceId_ThrowsNotFoundException()
    {
        // Arrange
        var whiteboard = new Whiteboard(
            markerColor: Colors.TryCreate("Green", out var color) ? color! : throw new InvalidOperationException("Invalid color"),
            orientation: Orientation.TryCreate("East", out var orientation) ? orientation! : throw new InvalidOperationException("Invalid orientation"),
            position: new Coordinates(0, 0, 0),
            dimensions: new Dimension(2, 1, 0.1)
        );

        // Act & Assert
        await _whiteboardServices.Invoking(x => x.AddWhiteboardAsync(999, whiteboard))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage("Learning space with ID 999 not found.");
    }

    /// <summary>
    /// Tests that UpdateWhiteboardAsync fails with invalid component ID.
    /// </summary>
    [Fact]
    public async Task UpdateWhiteboardAsync_WithInvalidComponentId_ReturnsFalse()
    {
        // Arrange
        var updatedWhiteboard = new Whiteboard
        {
            MarkerColor = Colors.TryCreate("Yellow", out var color) ? color! : throw new InvalidOperationException("Invalid color"),
            Orientation = Orientation.TryCreate("West", out var orientation) ? orientation! : throw new InvalidOperationException("Invalid orientation"),
            Position = Coordinates.Create(0, 0, 0),
            Dimensions = Dimension.Create(1, 1, 0.1),
            DisplayId = "WB-1"
        };

        // Act
        var result = await _whiteboardServices.UpdateWhiteboardAsync(1, 999, updatedWhiteboard);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that UpdateWhiteboardAsync successfully updates a whiteboard and persists changes.
    /// </summary>
    [Fact]
    public async Task UpdateWhiteboardAsync_WithValidWhiteboard_PersistsChanges()
    {
        // Arrange - Create a new whiteboard with a unique color for testing
        var originalWhiteboard = new Whiteboard
        {
            
            MarkerColor = Colors.TryCreate("Lime", out var color) ? color! : throw new InvalidOperationException("Invalid color"),
            Orientation = Orientation.TryCreate("East", out var orientation) ? orientation! : throw new InvalidOperationException("Invalid orientation"),
            Position = Coordinates.Create(15, 15, 0),
            Dimensions = Dimension.Create(3, 2, 0.1),
            DisplayId = "WB-1"
        };

        await _whiteboardServices.AddWhiteboardAsync(1, originalWhiteboard);

        var allWhiteboards = await _whiteboardServices.GetWhiteboardAsync();
        var addedWhiteboard = allWhiteboards.LastOrDefault(w => w.MarkerColor!.Value == "Lime");
        addedWhiteboard.Should().NotBeNull("the lime whiteboard should have been added");

        var updatedWhiteboard = new Whiteboard
        {
            
        
            MarkerColor = Colors.TryCreate("Teal", out var updatedColor) ? updatedColor! : throw new InvalidOperationException("Invalid color"),
            Orientation = Orientation.TryCreate("West", out var updatedOrientation) ? updatedOrientation! : throw new InvalidOperationException("Invalid orientation"),
            Position = Coordinates.Create(25, 25, 0),
            Dimensions = Dimension.Create(4, 3, 0.2),
            DisplayId = "WB-"
        };

        // Act
        await _whiteboardServices.UpdateWhiteboardAsync(1, addedWhiteboard!.ComponentId, updatedWhiteboard);

        // Assert
        var updatedWhiteboards = await _whiteboardServices.GetWhiteboardAsync();
        updatedWhiteboards.Should().Contain(w => w.ComponentId == addedWhiteboard.ComponentId && w.MarkerColor!.Value == "Teal", 
            "the specific whiteboard should now have the updated color");
    }

    /// <summary>
    /// Tests that AddWhiteboardAsync with valid data successfully adds a whiteboard.
    /// </summary>
    [Fact]
    public async Task AddWhiteboardAsync_WithValidData_ReturnsTrue()
    {
        // Arrange
        var whiteboard = new Whiteboard
        {
            MarkerColor = Colors.TryCreate("Purple", out var color) ? color! : throw new InvalidOperationException("Invalid color"),
            Orientation = Orientation.TryCreate("North", out var orientation) ? orientation! : throw new InvalidOperationException("Invalid orientation"),
            Position = Coordinates.Create(30, 30, 0),
            Dimensions = Dimension.Create(3, 2, 0.1),
            DisplayId = "WB-"
        };

        // Act
        var addResult = await _whiteboardServices.AddWhiteboardAsync(1, whiteboard);

        // Assert
        addResult.Should().BeTrue("adding a whiteboard with valid data should succeed");
    }

    /// <summary>
    /// Tests that whiteboards can be added with different colors.
    /// </summary>
    [Fact]
    public async Task AddWhiteboardAsync_WithSpecificColor_ReturnsTrue()
    {
        // Arrange
        var whiteboard = new Whiteboard
        {
            MarkerColor = Colors.TryCreate("Orange", out var color) ? color! : throw new InvalidOperationException("Invalid color"),
            Orientation = Orientation.TryCreate("East", out var orientation) ? orientation! : throw new InvalidOperationException("Invalid orientation"),
            Position = Coordinates.Create(15, 15, 0),
            Dimensions = Dimension.Create(2.5, 1.5, 0.1),
            DisplayId = "WB-"
        };

        // Act
        var addResult = await _whiteboardServices.AddWhiteboardAsync(1, whiteboard);

        // Assert
        addResult.Should().BeTrue("adding a whiteboard with a specific color should succeed");
    }
}
