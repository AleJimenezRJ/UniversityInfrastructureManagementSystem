using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;


namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Integration.Repositories.ComponentsManagement;

[Collection("Database collection")]
public class SqlLearningComponentRepositoryIntegrationTests
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlLearningComponentRepositoryIntegrationTests"/> class.
    /// </summary>
    /// <param name="fixture">Fixture providing the integration test services.</param>
    public SqlLearningComponentRepositoryIntegrationTests(IntegrationTestFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }

    /// <summary>
    /// Tests that GetSingleLearningComponentAsync successfully returns
    /// an existing learning component from the database when it has not been deleted.
    /// </summary>
    [Fact]
    public async Task GetSingleLearningComponentAsync_Should_Return_Component_When_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<ILearningComponentRepository>();

        // Arrange: Create full entity hierarchy (University → Campus → Area → Building → Floor → LearningSpace)
        var university = new University(EntityName.Create("UCR"), EntityLocation.Create("CR"));
        var campus = new Campus(EntityName.Create("Rodrigo Facio"), EntityLocation.Create("San Pedro"), university);
        var area = new Area(EntityName.Create("Finca1"), campus);
        var building = new Building(
            EntityName.Create("social sciences"),
            Coordinates.Create(1, 1, 0),
            Dimension.Create(10, 10, 3),
            Colors.Create("Blue"),
            area
        );

        context.University.Add(university);
        context.Campus.Add(campus);
        context.Area.Add(area);
        context.Buildings.Add(building);
        await context.SaveChangesAsync();

        var floor = new Floor(FloorNumber.Create(1));
        context.Floors.Add(floor);
        context.Entry(floor).Property("BuildingId").CurrentValue = building.BuildingInternalId;
        await context.SaveChangesAsync();

        var space = new LearningSpace(
            EntityName.Create("Room A"),
            LearningSpaceType.Create("Classroom"),
            Capacity.Create(20),
            Size.Create(3), Size.Create(5), Size.Create(7),
            Colors.Create("White"), Colors.Create("Red"), Colors.Create("White")
        );
        context.LearningSpaces.Add(space);
        context.Entry(space).Property("FloorId").CurrentValue = floor.FloorId;
        await context.SaveChangesAsync();

        var projector = new Projector
        {
            ProjectedContent = "Math",
            ProjectionArea = Area2D.Create(100, 75),
            Orientation = Orientation.Create("East"),
            Dimensions = Dimension.Create(2, 2, 2),
            Position = Coordinates.Create(1, 1, 0)
        };
        context.LearningComponents.Add(projector);
        context.Entry(projector).Property("LearningSpaceId").CurrentValue = space.LearningSpaceId;
        context.Entry(projector).Property("DisplayId").CurrentValue = "PROJ-"+projector.ComponentId;
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetSingleLearningComponentAsync(projector.ComponentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(projector.ComponentId, result.ComponentId);
        Assert.Equal("Math", ((Projector)result).ProjectedContent);
    }

    /// <summary>
    /// Tests that GetSingleLearningComponentAsync throws a NotFoundException
    /// when the component exists but has been soft-deleted.
    /// </summary>
    [Fact]
    public async Task GetSingleLearningComponentAsync_Should_Throw_When_Component_Is_Deleted()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<ILearningComponentRepository>();

        // Arrange: create full hierarchy
        var university = new University(EntityName.Create("UTN"), EntityLocation.Create("CR"));
        var campus = new Campus(new EntityName("Curri"), new EntityLocation("SJ"), university);
        var area = new Area(new EntityName("Area1"), campus);

        var building = new Building(
            name: EntityName.Create("Engineering"),
            coordinates: Coordinates.Create(10.0, 20.0, 0.0),
            dimensions: Dimension.Create(30, 40, 10),
            color: Colors.Create("Blue"),
            area: area
        );
        context.University.Add(university);
        context.Campus.Add(campus);
        context.Area.Add(area);
        context.Buildings.Add(building);
        await context.SaveChangesAsync();

        var floor = new Floor(FloorNumber.Create(1));
        context.Floors.Add(floor);
        context.Entry(floor).Property("BuildingId").CurrentValue = building.BuildingInternalId;
        await context.SaveChangesAsync();

        // Create LearningSpace
        var space = new LearningSpace(
            name: EntityName.Create("Lab B"),
            type: LearningSpaceType.Create("Laboratory"),
            maxCapacity: Capacity.Create(30),
            height: Size.Create(3.0),
            width: Size.Create(5.0),
            length: Size.Create(7.0),
            colorFloor: Colors.Create("Gray"),
            colorWalls: Colors.Create("White"),
            colorCeiling: Colors.Create("White")
        );
        context.LearningSpaces.Add(space);
        context.Entry(space).Property("FloorId").CurrentValue = floor.FloorId;
        await context.SaveChangesAsync();

        // Create projector
        var projector = new Projector
        {
            ProjectedContent = "sample content",
            ProjectionArea = Area2D.Create(100, 75),
            Orientation = Orientation.Create("South"),
            Dimensions = Dimension.Create(2, 2, 2),
            Position = Coordinates.Create(2, 2, 0)
        };

        context.LearningComponents.Add(projector);
        context.Entry(projector).Property("LearningSpaceId").CurrentValue = space.LearningSpaceId;
        context.Entry(projector).Property("DisplayId").CurrentValue = "PROJ-"+projector.ComponentId;
        await context.SaveChangesAsync();

        // Soft delete
        projector.IsDeleted = true;
        context.LearningComponents.Update(projector);
        await context.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            repo.GetSingleLearningComponentAsync(projector.ComponentId));
    }

    /// <summary>
    /// Tests that AddComponentAsync throws a NotFoundException when trying
    /// to add a learning component to a non-existent learning space.
    /// </summary>
    [Fact]
    public async Task AddComponentAsync_Should_Fail_When_LearningSpace_Not_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<ILearningComponentRepository>();

        // Arrange:
        var invalidLearningSpaceId = -1;

        var projector = new Projector
        {
            ProjectedContent = "Test content",
            ProjectionArea = Area2D.Create(100, 80),
            Orientation = Orientation.Create("North"),
            Dimensions = Dimension.Create(1, 1, 1),
            Position = Coordinates.Create(1, 1, 1)
        };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await repo.AddComponentAsync(invalidLearningSpaceId, projector);
        });
    }
}