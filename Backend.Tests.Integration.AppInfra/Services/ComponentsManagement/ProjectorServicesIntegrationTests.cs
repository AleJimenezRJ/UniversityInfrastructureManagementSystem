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
/// Integration tests for ProjectorServices that test the interaction between
/// Application and Infrastructure layers for projector operations.
/// </summary>
public class ProjectorServicesIntegrationTests
{
    private readonly IProjectorServices _projectorServices;

    public ProjectorServicesIntegrationTests()
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
        _projectorServices = serviceProvider.GetRequiredService<IProjectorServices>();
    }

    private static void AddInfrastructureServicesForTesting(IServiceCollection services)
    {
        // Register the SqlLearningComponent implementation for ILearningComponentRepository interface
        services.AddTransient<ILearningComponentRepository, SqlLearningComponentRepository>();
        services.AddTransient<ILearningSpaceRepository, SqlLearningSpaceRepository>();
        services.AddTransient<IWhiteboardRepository, SqlWhiteboardRepository>();
        services.AddTransient<IProjectorRepository, SqlProjectorRepository>();
    }

    /// <summary>
    /// Tests that GetProjectorAsync returns all projectors from the database.
    /// </summary>
    [Fact]
    public async Task GetProjectorAsync_ReturnsAllProjectors()
    {
        // Act
        var result = await _projectorServices.GetProjectorAsync();

        // Assert
        var projectors = result.ToList();
        projectors.Should().NotBeNull();
        projectors.Should().BeAssignableTo<IEnumerable<Projector>>();
    }

    /// <summary>
    /// Tests basic projector service integration.
    /// Validates that the service layer properly handles requests with invalid learning space ID.
    /// </summary>
    [Fact]
    public async Task AddProjectorAsync_ServiceIntegration_ThrowsNotFoundException()
    {
        // Arrange
        var projector = new Projector(
            projectedContent: "Service Integration Test",
            projectionArea: Area2D.Create(5, 4),
            orientation: Orientation.TryCreate("South", out var orientation) ? orientation! : throw new InvalidOperationException("Invalid orientation"),
            position: new Coordinates(10, 10, 0),
            dimensions: new Dimension(3, 3, 1.5)
        );

        // Act & Assert
        // Should throw NotFoundException when learning space doesn't exist
        await _projectorServices.Invoking(x => x.AddProjectorAsync(999, projector))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage("Learning space with ID 999 not found.");
    }

    /// <summary>
    /// Tests that GetProjectorAsync successfully retrieves projectors from repository.
    /// </summary>
    [Fact]
    public async Task GetProjectorAsync_ServiceConnectedToRepository_ReturnsProjectors()
    {
        // Act
        var allProjectors = await _projectorServices.GetProjectorAsync();

        // Assert
        var projectors = allProjectors.ToList();
        projectors.Should().NotBeNull();
        projectors.Should().BeAssignableTo<IEnumerable<Projector>>();
    }

    /// <summary>
    /// Tests that AddProjectorAsync throws NotFoundException with invalid learning space ID.
    /// </summary>
    [Fact]
    public async Task AddProjectorAsync_WithInvalidLearningSpaceId_ThrowsNotFoundException()
    {
        // Arrange
        var projector = new Projector(
            projectedContent: "Invalid Space Test",
            projectionArea: Area2D.Create(3, 3),
            orientation: Orientation.TryCreate("West", out var orientation) ? orientation! : throw new InvalidOperationException("Invalid orientation"),
            position: new Coordinates(0, 0, 0),
            dimensions: new Dimension(1, 1, 1)
        );

        // Act & Assert
        await _projectorServices.Invoking(x => x.AddProjectorAsync(999, projector))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage("Learning space with ID 999 not found.");
    }

    /// <summary>
    /// Tests that UpdateProjectorAsync successfully updates an existing projector.
    /// </summary>
    [Fact]
    public async Task UpdateProjectorAsync_WithValidProjector_ReturnsTrue()
    {
        // Arrange
        var originalProjector = new Projector
        {
            ProjectedContent = "Original Content",
            ProjectionArea = Area2D.Create(4, 3),
            Orientation = Orientation.TryCreate("North", out var orientation) ? orientation! : throw new InvalidOperationException("Invalid orientation"),
            Position = Coordinates.Create(5, 5, 0),
            Dimensions = Dimension.Create(2, 2, 1),
            DisplayId = "PROJ-2"
        };

        await _projectorServices.AddProjectorAsync(1, originalProjector);

        var allProjectors = await _projectorServices.GetProjectorAsync();
        var addedProjector = allProjectors.FirstOrDefault(p => p.ProjectedContent == "Original Content");
        addedProjector.Should().NotBeNull();

        var updatedProjector = new Projector
        {
            ProjectedContent = "Updated Content",
            ProjectionArea = Area2D.Create(5, 4),
            Orientation = Orientation.TryCreate("South", out var updatedOrientation) ? updatedOrientation! : throw new InvalidOperationException("Invalid orientation"),
            Position = Coordinates.Create(10, 10, 0),
            Dimensions = Dimension.Create(3, 3, 1.5),
            DisplayId = "PROJ-"
        };

        // Act
        var updateResult = await _projectorServices.UpdateProjectorAsync(1, addedProjector!.ComponentId, updatedProjector);

        // Assert
        updateResult.Should().BeTrue();
    }

    /// <summary>
    /// Tests that UpdateProjectorAsync fails with invalid component ID.
    /// </summary>
    [Fact]
    public async Task UpdateProjectorAsync_WithInvalidComponentId_ReturnsFalse()
    {
        // Arrange
        var updatedProjector = new Projector
        {
            ProjectedContent = "This should not update",
            ProjectionArea = Area2D.Create(2, 2),
            Orientation = Orientation.TryCreate("North", out var orientation) ? orientation! : throw new InvalidOperationException("Invalid orientation"),
            Position = Coordinates.Create(0, 0, 0),
            Dimensions = Dimension.Create(1, 1, 1),
            DisplayId = "PROJ-"
        };

        // Act
        var result = await _projectorServices.UpdateProjectorAsync(1, 999, updatedProjector);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests that UpdateProjectorAsync persists changes to the database.
    /// </summary>
    [Fact]
    public async Task UpdateProjectorAsync_WithValidProjector_PersistsChanges()
    {
        // Arrange
        var originalProjector = new Projector
        {
            ProjectedContent = "Original Persistent Content",
            ProjectionArea = Area2D.Create(4, 3),
            Orientation = Orientation.TryCreate("North", out var orientation)
                ? orientation!
                : throw new InvalidOperationException("Invalid orientation"),
            Position = Coordinates.Create(5, 5, 0),
            Dimensions = Dimension.Create(2, 2, 1),
            DisplayId = "PROJ-"
        };

        await _projectorServices.AddProjectorAsync(1, originalProjector);

        var allProjectors = await _projectorServices.GetProjectorAsync();
        var addedProjector = allProjectors.FirstOrDefault(p => p.ProjectedContent == "Original Persistent Content");
        addedProjector.Should().NotBeNull();

        var updatedProjector = new Projector
        {
            ProjectedContent = "Updated Persistent Content",
            ProjectionArea = Area2D.Create(6, 5),
            Orientation = Orientation.TryCreate("East", out var updatedOrientation)
                ? updatedOrientation!
                : throw new InvalidOperationException("Invalid orientation"),
            Position = Coordinates.Create(15, 15, 0),
            Dimensions = Dimension.Create(3, 3, 1.5),
            DisplayId = "PROJ-"
        };

        // Act
        await _projectorServices.UpdateProjectorAsync(1, addedProjector!.ComponentId, updatedProjector);

        // Assert
        var updatedProjectors = await _projectorServices.GetProjectorAsync();
        updatedProjectors.Should().Contain(p => p.ProjectedContent == "Updated Persistent Content");
    }

    /// <summary>
    /// Tests that AddProjectorAsync with valid data successfully adds a projector.
    /// </summary>
    [Fact]
    public async Task AddProjectorAsync_WithValidData_ReturnsTrue()
    {
        // Arrange
        var projector = new Projector
        {
            ProjectedContent = "Valid Projector Test",
            ProjectionArea = Area2D.Create(4, 3),
            Orientation = Orientation.TryCreate("North", out var orientation)
                ? orientation!
                : throw new InvalidOperationException("Invalid orientation"),
            Position = Coordinates.Create(25, 25, 0),
            Dimensions = Dimension.Create(2, 2, 1),
            DisplayId = "PROJ-"
        };
            

        // Act
        var addResult = await _projectorServices.AddProjectorAsync(1, projector);

        // Assert
        addResult.Should().BeTrue("adding a projector with valid data should succeed");
    }
}
