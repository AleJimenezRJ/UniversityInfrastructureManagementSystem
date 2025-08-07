using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure;

namespace UCR.ECCI.PI.ThemePark.Backend.Tests.Integration.DbInfra.DatabaseIntegration;

/// <summary>
/// Fixture for setting up a test database context specifically for learning components in the ThemePark application.
/// </summary>
public class TestDatabaseFixtureLearningComponents : IDisposable
{
    /// <summary>
    /// Base connection string template for the test database.
    /// </summary>
    private const string BaseConnectionStringTemplate = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog={0};Integrated Security=True;Connect Timeout=60;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

    /// <summary>
    /// Database context for the ThemePark application, specifically for learning components testing.
    /// </summary>
    internal ThemeParkDataBaseContext DatabaseContext { get; private set; }

    public const int TestLearningSpaceId = 1;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestDatabaseFixtureLearningComponents"/> class with a unique test ID.
    /// </summary>
    /// <param name="testId"></param>
    public TestDatabaseFixtureLearningComponents(string testId)
    {
        var dbName = $"UCR.ECCI.PI.ThemePark.Database.Tests_{testId}";
        var connectionString = string.Format(BaseConnectionStringTemplate, dbName);

        var optionsBuilder = new DbContextOptionsBuilder<ThemeParkDataBaseContext>();
        optionsBuilder.UseSqlServer(connectionString);
        DatabaseContext = new ThemeParkDataBaseContext(optionsBuilder.Options);

        DatabaseContext.Database.EnsureDeleted();
        DatabaseContext.Database.EnsureCreated();
    }

    /// <summary>
    /// Seeds the learning components and related entities into the test database.
    /// </summary>
    /// <returns></returns>
    public async Task SeedLearningComponentsAsync()
    {
        DatabaseContext.LearningComponents.RemoveRange(await DatabaseContext.LearningComponents.ToListAsync());
        DatabaseContext.LearningSpaces.RemoveRange(await DatabaseContext.LearningSpaces.ToListAsync());
        DatabaseContext.Floors.RemoveRange(await DatabaseContext.Floors.ToListAsync());
        DatabaseContext.Buildings.RemoveRange(await DatabaseContext.Buildings.ToListAsync());
        DatabaseContext.Area.RemoveRange(await DatabaseContext.Area.ToListAsync());
        DatabaseContext.Campus.RemoveRange(await DatabaseContext.Campus.ToListAsync());
        DatabaseContext.University.RemoveRange(await DatabaseContext.University.ToListAsync());

        await DatabaseContext.SaveChangesAsync();

        // All the entities related to learning components
        var university = new University(EntityName.Create("UCR"), EntityLocation.Create("CR"));
        var campus = new Campus(EntityName.Create("Rodrigo Facio"), EntityLocation.Create("San Pedro"), university);
        var area = new Area(EntityName.Create("Finca1"), campus);
        var building = new Building(
            EntityName.Create("Social Sciences"),
            Coordinates.Create(1, 1, 0),
            Dimension.Create(10, 10, 3),
            Colors.Create("Blue"),
            area
        );

        var floor = new Floor(FloorNumber.Create(1));
        var space = new LearningSpace(
            EntityName.Create("Room A"),
            LearningSpaceType.Create("Classroom"),
            Capacity.Create(20),
            Size.Create(3), Size.Create(5), Size.Create(7),
            Colors.Create("White"), Colors.Create("White"), Colors.Create("White")
        );

        var projector = new Projector(
            projectedContent: "Math Presentation",
            projectionArea: Area2D.Create(2, 2),
            orientation: Orientation.Create("North"),
            position: Coordinates.Create(1.0, 2.0, 0.0),
            dimensions: Dimension.Create(1.5, 2.0, 0.5)
        );
        projector.DisplayId = "PRJ-001";

        var whiteboard = new Whiteboard(
            markerColor: Colors.Create("Blue"),
            orientation: Orientation.Create("South"),
            position: Coordinates.Create(2.0, 1.0, 0.0),
            dimensions: Dimension.Create(2.0, 3.0, 0.1)
        );
        whiteboard.DisplayId = "WBD-001";

        // Linking entities together
        await DatabaseContext.University.AddAsync(university);
        await DatabaseContext.Campus.AddAsync(campus);
        await DatabaseContext.Area.AddAsync(area);
        await DatabaseContext.Buildings.AddAsync(building);
        await DatabaseContext.SaveChangesAsync();

        DatabaseContext.Entry(floor).Property("BuildingId").CurrentValue = building.BuildingInternalId;
        await DatabaseContext.Floors.AddAsync(floor);
        await DatabaseContext.SaveChangesAsync();

        DatabaseContext.Entry(space).Property("FloorId").CurrentValue = floor.FloorId;
        await DatabaseContext.LearningSpaces.AddAsync(space);
        await DatabaseContext.SaveChangesAsync();

        DatabaseContext.Entry(projector).Property("LearningSpaceId").CurrentValue = space.LearningSpaceId;
        DatabaseContext.Entry(whiteboard).Property("LearningSpaceId").CurrentValue = space.LearningSpaceId;

        await DatabaseContext.LearningComponents.AddAsync(projector);
        await DatabaseContext.LearningComponents.AddAsync(whiteboard);
        await DatabaseContext.SaveChangesAsync();

        DatabaseContext.ChangeTracker.Clear();
    }

    /// <summary>
    /// Disposes of the test database context and deletes the database.
    /// </summary>
    public void Dispose()
    {
        DatabaseContext.Database.EnsureDeleted();
        DatabaseContext.Dispose();
    }
}
