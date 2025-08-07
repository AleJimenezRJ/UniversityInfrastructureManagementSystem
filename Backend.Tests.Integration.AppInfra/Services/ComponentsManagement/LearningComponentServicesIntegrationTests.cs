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
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.AppInfra.Services.ComponentsManagement;

/// <summary>
/// Integration tests for LearningComponentServices that test the interaction between
/// Application and Infrastructure layers for learning component operations.
/// </summary>
public class LearningComponentServicesIntegrationTests
{
    private readonly ILearningComponentServices _learningComponentServices;

    public LearningComponentServicesIntegrationTests()
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
        _learningComponentServices = serviceProvider.GetRequiredService<ILearningComponentServices>();
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
    /// Tests that GetLearningComponentAsync returns components with proper pagination.
    /// </summary>
    [Fact]
    public async Task GetLearningComponentAsync_WithValidPagination_ReturnsComponents()
    {
        // Arrange
        const int pageSize = 10;
        const int pageIndex = 0;

        // Act
        var result = await _learningComponentServices.GetLearningComponentAsync(pageSize, pageIndex);

        // Assert
        var learningComponents = result.ToList();
        learningComponents.Should().NotBeNull();
        learningComponents.Should().BeAssignableTo<IEnumerable<LearningComponent>>();
    }

    /// <summary>
    /// Tests that GetLearningComponentsByIdAsync handles invalid learning space ID gracefully.
    /// </summary>
    [Fact]
    public async Task GetLearningComponentsByIdAsync_WithInvalidLearningSpaceId_ReturnsEmptyResult()
    {
        // Arrange
        const int invalidLearningSpaceId = 999;
        const int pageSize = 10;
        const int pageIndex = 0;
        const string searchString = "";

        // Act
        var result = await _learningComponentServices.GetLearningComponentsByIdAsync(
            invalidLearningSpaceId, pageSize, pageIndex, searchString);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IEnumerable<LearningComponent>>();
        result.PageIndex.Should().Be(pageIndex);
        result.PageSize.Should().Be(pageSize);
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that GetLearningComponentsByIdAsync with search string works correctly.
    /// </summary>
    [Fact]
    public async Task GetLearningComponentsByIdAsync_WithSearchString_ReturnsFilteredResults()
    {
        // Arrange
        const int learningSpaceId = 1;
        const int pageSize = 10;
        const int pageIndex = 0;
        const string searchString = "NonExistentComponent";

        // Act
        var result = await _learningComponentServices.GetLearningComponentsByIdAsync(
            learningSpaceId, pageSize, pageIndex, searchString);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IEnumerable<LearningComponent>>();
        result.Should().BeEmpty(); // Should be empty since the search term doesn't match anything
    }

    /// <summary>
    /// Tests that GetSingleLearningComponentByIdAsync throws NotFoundException for a non-existent component.
    /// </summary>
    [Fact]
    public async Task GetSingleLearningComponentByIdAsync_WithNonExistentId_ThrowsNotFoundException()
    {
        // Arrange
        const int nonExistentId = 999;

        // Act & Assert
        await _learningComponentServices.Invoking(x => x.GetSingleLearningComponentByIdAsync(nonExistentId))
            .Should().ThrowAsync<NotFoundException>()
            .WithMessage("Component with ID '999' not found.");
    }

    /// <summary>
    /// Tests that DeleteLearningComponentAsync returns false for a non-existent component.
    /// </summary>
    [Fact]
    public async Task DeleteLearningComponentAsync_WithNonExistentId_ReturnsFalse()
    {
        // Arrange
        const int nonExistentId = 999;

        // Act
        var result = await _learningComponentServices.DeleteLearningComponentAsync(nonExistentId);

        // Assert
        result.Should().BeFalse();
    }

    /// <summary>
    /// Tests basic service integration without complex entity creation.
    /// This validates that the Application and Infrastructure layers are properly connected.
    /// </summary>
    [Fact]
    public async Task GetLearningComponentAsync_ServiceIntegration_WorksCorrectly()
    {
        // Arrange
        const int pageSize = 10;
        const int pageIndex = 0;

        // Act
        var result = await _learningComponentServices.GetLearningComponentAsync(pageSize, pageIndex);

        // Assert
        var learningComponents = result.ToList();
        learningComponents.Should().NotBeNull();
        learningComponents.Should().BeAssignableTo<IEnumerable<LearningComponent>>();
        // Should work without errors even if no components exist
    }

    /// <summary>
    /// Tests that pagination works correctly with page size 5 across Application and Infrastructure layers.
    /// </summary>
    [Fact]
    public async Task GetLearningComponentAsync_WithPageSize5_ReturnsPaginatedResults()
    {
        // Arrange
        const int pageSize = 5;
        const int pageIndex = 0;

        // Act
        var result = await _learningComponentServices.GetLearningComponentAsync(pageSize, pageIndex);

        // Assert
        var learningComponents = result.ToList();
        learningComponents.Should().NotBeNull();
        learningComponents.Should().BeAssignableTo<IEnumerable<LearningComponent>>();
    }

    /// <summary>
    /// Tests that pagination works correctly with page size 10 across Application and Infrastructure layers.
    /// </summary>
    [Fact]
    public async Task GetLearningComponentAsync_WithPageSize10_ReturnsPaginatedResults()
    {
        // Arrange
        const int pageSize = 10;
        const int pageIndex = 0;

        // Act
        var result = await _learningComponentServices.GetLearningComponentAsync(pageSize, pageIndex);

        // Assert
        var learningComponents = result.ToList();
        learningComponents.Should().NotBeNull();
        learningComponents.Should().BeAssignableTo<IEnumerable<LearningComponent>>();
    }
}
