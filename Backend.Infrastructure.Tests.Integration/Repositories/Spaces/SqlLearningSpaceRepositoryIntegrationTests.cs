using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Integration.Repositories.Spaces;

/// <summary>
/// Integration tests for <see cref="SqlLearningSpaceRepository"/> using a real database context.
/// </summary>
[Collection("Database collection")]
public class SqlLearningSpaceRepositoryIntegrationTests
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes the test class with a shared integration fixture that provides configured services and database.
    /// </summary>
    /// <param name="fixture">The shared integration test fixture.</param>
    public SqlLearningSpaceRepositoryIntegrationTests(IntegrationTestFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }

    /// <summary>
    /// Tests that <see cref="ILearningSpaceRepository.ReadLearningSpaceAsync(int)"/> returns the correct entity 
    /// when a valid learning space ID is provided.
    /// </summary>
    [Fact]
    public async Task ReadLearningSpaceAsync_Should_Return_Entity_When_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<ILearningSpaceRepository>();

        // Arrange: Create full entity hierarchy (University → Campus → Area → Building → Floor → LearningSpace)
        var university = new University(EntityName.Create("UCRLS"), EntityLocation.Create("CRLS"));
        var campus = new Campus(EntityName.Create("Rodrigo FacioLS"), EntityLocation.Create("San PedroLs"), university);
        var area = new Area(EntityName.Create("Finca1LS"), campus);
        var building = new Building(
            EntityName.Create("Eng HallLS"),
            Coordinates.Create(1, 1, 0),
            Dimension.Create(10, 10, 3),
            Colors.Create("Blue"),
            area
        );

        context.AddRange(university, campus, area, building);
        await context.SaveChangesAsync();

        var floor = new Floor(FloorNumber.Create(1));
        context.Floors.Add(floor);
        context.Entry(floor).Property("BuildingId").CurrentValue = building.BuildingInternalId;
        await context.SaveChangesAsync();

        var space = new LearningSpace(
            EntityName.Create("Sala 101"),
            LearningSpaceType.Create("Classroom"),
            Capacity.Create(20),
            Size.Create(3), Size.Create(5), Size.Create(7),
            Colors.Create("White"), Colors.Create("Red"), Colors.Create("White")
        );
        context.LearningSpaces.Add(space);
        context.Entry(space).Property("FloorId").CurrentValue = floor.FloorId;
        await context.SaveChangesAsync();

        // Act
        var result = await repo.ReadLearningSpaceAsync(space.LearningSpaceId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Sala 101", result.Name.Name);
        Assert.Equal(space.LearningSpaceId, result.LearningSpaceId);
    }

    /// <summary>
    /// Tests that <see cref="ILearningSpaceRepository.ListLearningSpacesAsync(int)"/> returns all learning spaces
    /// associated with a specific floor.
    /// </summary>
    //[Fact]
    public async Task ListLearningSpacesAsync_Should_Return_All_Spaces_For_Floor()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<ILearningSpaceRepository>();

        // Arrange: Create hierarchy and add multiple learning spaces to the same floor
        var university = new University(EntityName.Create("TEC"), EntityLocation.Create("CR"));
        var campus = new Campus(EntityName.Create("Rodrigo"), EntityLocation.Create("San Pedro Osula"), university);
        var area = new Area(EntityName.Create("Finca2"), campus);
        var building = new Building(
            EntityName.Create("Second Building"),
            Coordinates.Create(1, 1, 1),
            Dimension.Create(11, 11, 4),
            Colors.Create("Blue"),
            area
        );
        context.AddRange(university, campus, area, building);
        await context.SaveChangesAsync();

        var floor = new Floor(FloorNumber.Create(2));
        context.Floors.Add(floor);
        context.Entry(floor).Property("BuildingId").CurrentValue = building.BuildingInternalId;
        await context.SaveChangesAsync();

        var space1 = new LearningSpace(
            EntityName.Create("Aula 102"),
            LearningSpaceType.Create("Classroom"),
            Capacity.Create(25),
            Size.Create(3), Size.Create(4), Size.Create(5),
            Colors.Create("White"), Colors.Create("Gray"), Colors.Create("White")
        );
        var space2 = new LearningSpace(
            EntityName.Create("Aula 103"),
            LearningSpaceType.Create("Classroom"),
            Capacity.Create(30),
            Size.Create(3), Size.Create(4), Size.Create(5),
            Colors.Create("White"), Colors.Create("Gray"), Colors.Create("White")
        );
        context.LearningSpaces.AddRange(space1, space2);
        context.Entry(space1).Property("FloorId").CurrentValue = floor.FloorId;
        context.Entry(space2).Property("FloorId").CurrentValue = floor.FloorId;
        await context.SaveChangesAsync();

        // Act
        var result = await repo.ListLearningSpacesAsync(floor.FloorId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Name.Name == "Aula 102");
        Assert.Contains(result, r => r.Name.Name == "Aula 103");
    }

    /// <summary>
    /// Tests that <see cref="ILearningSpaceRepository.CreateLearningSpaceAsync(int, LearningSpace)"/> creates a new learning space entity when the floor exists.
    /// </summary>
    [Fact]
    public async Task CreateLearningSpaceAsync_Should_Create_Entity_When_Floor_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<ILearningSpaceRepository>();

        // Usa nombres únicos para evitar conflictos de clave primaria
        var university = new University(EntityName.Create("UCR_CreateTest"), EntityLocation.Create("CR_CreateTest"));
        var campus = new Campus(EntityName.Create("Rodrigo_CreateTest"), EntityLocation.Create("San Pedro_CreateTest"), university);
        var area = new Area(EntityName.Create("Finca1_CreateTest"), campus);
        var building = new Building(
            EntityName.Create("Eng Hall_CreateTest"),
            Coordinates.Create(1, 1, 0),
            Dimension.Create(10, 10, 3),
            Colors.Create("Blue"),
            area
        );
        context.AddRange(university, campus, area, building);
        await context.SaveChangesAsync();

        var floor = new Floor(FloorNumber.Create(1));
        context.Floors.Add(floor);
        context.Entry(floor).Property("BuildingId").CurrentValue = building.BuildingInternalId;
        await context.SaveChangesAsync();

        var space = new LearningSpace(
            EntityName.Create("Sala 201"),
            LearningSpaceType.Create("Classroom"),
            Capacity.Create(15),
            Size.Create(3), Size.Create(5), Size.Create(7),
            Colors.Create("White"), Colors.Create("Red"), Colors.Create("White")
        );

        // Act
        var result = await repo.CreateLearningSpaceAsync(floor.FloorId, space);

        // Assert
        Assert.True(result);
        var fromDb = context.LearningSpaces.AsEnumerable().FirstOrDefault(ls => ls.Name.Name == "Sala 201");
        Assert.NotNull(fromDb);
    }

    /// <summary>
    /// Tests that <see cref="ILearningSpaceRepository.CreateLearningSpaceAsync(int, LearningSpace)"/> throws <see cref="NotFoundException"/> when the floor does not exist.
    /// </summary>
    [Fact]
    public async Task CreateLearningSpaceAsync_Should_Throw_When_Floor_Not_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ILearningSpaceRepository>();
        var space = new LearningSpace(
            EntityName.Create("Sala 404"),
            LearningSpaceType.Create("Classroom"),
            Capacity.Create(10),
            Size.Create(2), Size.Create(2), Size.Create(2),
            Colors.Create("White"), Colors.Create("Red"), Colors.Create("White")
        );
        await Assert.ThrowsAsync<NotFoundException>(() => repo.CreateLearningSpaceAsync(-999, space));
    }

    /// <summary>
    /// Tests that <see cref="ILearningSpaceRepository.UpdateLearningSpaceAsync(int, LearningSpace)"/> updates an existing learning space entity.
    /// </summary>
    [Fact]
    public async Task UpdateLearningSpaceAsync_Should_Update_Entity_When_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<ILearningSpaceRepository>();

        // Arrange: create hierarchy and a learning space
        var university = new University(EntityName.Create("UCRU"), EntityLocation.Create("CRU"));
        var campus = new Campus(EntityName.Create("RodrigoU"), EntityLocation.Create("San PedroU"), university);
        var area = new Area(EntityName.Create("FincaU"), campus);
        var building = new Building(
            EntityName.Create("Eng HallU"),
            Coordinates.Create(1, 1, 0),
            Dimension.Create(10, 10, 3),
            Colors.Create("Blue"),
            area
        );
        context.AddRange(university, campus, area, building);
        await context.SaveChangesAsync();
        var floor = new Floor(FloorNumber.Create(1));
        context.Floors.Add(floor);
        context.Entry(floor).Property("BuildingId").CurrentValue = building.BuildingInternalId;
        await context.SaveChangesAsync();
        var space = new LearningSpace(
            EntityName.Create("Sala 301"),
            LearningSpaceType.Create("Classroom"),
            Capacity.Create(10),
            Size.Create(2), Size.Create(2), Size.Create(2),
            Colors.Create("White"), Colors.Create("Red"), Colors.Create("White")
        );
        context.LearningSpaces.Add(space);
        context.Entry(space).Property("FloorId").CurrentValue = floor.FloorId;
        await context.SaveChangesAsync();

        // Act
        var updated = new LearningSpace(
            EntityName.Create("Sala 301 Updated"),
            LearningSpaceType.Create("Laboratory"),
            Capacity.Create(20),
            Size.Create(3), Size.Create(3), Size.Create(3),
            Colors.Create("Black"), Colors.Create("Green"), Colors.Create("Yellow")
        );
        var result = await repo.UpdateLearningSpaceAsync(space.LearningSpaceId, updated);

        // Assert
        Assert.True(result);
        var fromDb = context.LearningSpaces.FirstOrDefault(ls => ls.LearningSpaceId == space.LearningSpaceId);
        Assert.NotNull(fromDb);
        Assert.Equal("Sala 301 Updated", fromDb.Name.Name);
        Assert.Equal("Laboratory", fromDb.Type.Value);
        Assert.Equal(20, fromDb.MaxCapacity.Value);
    }

    /// <summary>
    /// Tests that <see cref="ILearningSpaceRepository.UpdateLearningSpaceAsync(int, LearningSpace)"/> throws <see cref="NotFoundException"/> when the learning space does not exist.
    /// </summary>
    [Fact]
    public async Task UpdateLearningSpaceAsync_Should_Throw_When_Not_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ILearningSpaceRepository>();
        var updated = new LearningSpace(
            EntityName.Create("Sala 999"),
            LearningSpaceType.Create("Classroom"),
            Capacity.Create(10),
            Size.Create(2), Size.Create(2), Size.Create(2),
            Colors.Create("White"), Colors.Create("Red"), Colors.Create("White")
        );
        await Assert.ThrowsAsync<NotFoundException>(() => repo.UpdateLearningSpaceAsync(-999, updated));
    }

    /// <summary>
    /// Tests that <see cref="ILearningSpaceRepository.DeleteLearningSpaceAsync(int)"/> deletes an existing learning space entity.
    /// </summary>
    [Fact]
    public async Task DeleteLearningSpaceAsync_Should_Delete_Entity_When_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<ILearningSpaceRepository>();

        // Arrange: create hierarchy and a learning space
        var university = new University(EntityName.Create("UCRD"), EntityLocation.Create("CRD"));
        var campus = new Campus(EntityName.Create("RodrigoD"), EntityLocation.Create("San PedroD"), university);
        var area = new Area(EntityName.Create("FincaD"), campus);
        var building = new Building(
            EntityName.Create("Eng HallD"),
            Coordinates.Create(1, 1, 0),
            Dimension.Create(10, 10, 3),
            Colors.Create("Blue"),
            area
        );
        context.AddRange(university, campus, area, building);
        await context.SaveChangesAsync();
        var floor = new Floor(FloorNumber.Create(1));
        context.Floors.Add(floor);
        context.Entry(floor).Property("BuildingId").CurrentValue = building.BuildingInternalId;
        await context.SaveChangesAsync();
        var space = new LearningSpace(
            EntityName.Create("Sala 401"),
            LearningSpaceType.Create("Classroom"),
            Capacity.Create(10),
            Size.Create(2), Size.Create(2), Size.Create(2),
            Colors.Create("White"), Colors.Create("Red"), Colors.Create("White")
        );
        context.LearningSpaces.Add(space);
        context.Entry(space).Property("FloorId").CurrentValue = floor.FloorId;
        await context.SaveChangesAsync();

        // Act
        var result = await repo.DeleteLearningSpaceAsync(space.LearningSpaceId);

        // Assert
        Assert.True(result);
        var fromDb = context.LearningSpaces.FirstOrDefault(ls => ls.LearningSpaceId == space.LearningSpaceId);
        Assert.Null(fromDb);
    }

    /// <summary>
    /// Tests that <see cref="ILearningSpaceRepository.DeleteLearningSpaceAsync(int)"/> throws <see cref="NotFoundException"/> when the learning space does not exist.
    /// </summary>
    [Fact]
    public async Task DeleteLearningSpaceAsync_Should_Throw_When_Not_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ILearningSpaceRepository>();
        await Assert.ThrowsAsync<NotFoundException>(() => repo.DeleteLearningSpaceAsync(-999));
    }

    /// <summary>
    /// Tests that <see cref="ILearningSpaceRepository.ListLearningSpacesPaginatedAsync(int, int, int, string)"/> returns a paginated list of learning spaces for a floor.
    /// </summary>
    [Fact]
    public async Task ListLearningSpacesPaginatedAsync_Should_Return_Paginated_List()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<ILearningSpaceRepository>();

        // Arrange: create hierarchy and add multiple learning spaces
        var university = new University(EntityName.Create("UCRP"), EntityLocation.Create("CRP"));
        var campus = new Campus(EntityName.Create("RodrigoP"), EntityLocation.Create("San PedroP"), university);
        var area = new Area(EntityName.Create("FincaP"), campus);
        var building = new Building(
            EntityName.Create("Eng HallP"),
            Coordinates.Create(1, 1, 0),
            Dimension.Create(10, 10, 3),
            Colors.Create("Blue"),
            area
        );
        context.AddRange(university, campus, area, building);
        await context.SaveChangesAsync();
        var floor = new Floor(FloorNumber.Create(1));
        context.Floors.Add(floor);
        context.Entry(floor).Property("BuildingId").CurrentValue = building.BuildingInternalId;
        await context.SaveChangesAsync();
        var space1 = new LearningSpace(
            EntityName.Create("Aula 201"),
            LearningSpaceType.Create("Classroom"),
            Capacity.Create(25),
            Size.Create(3), Size.Create(4), Size.Create(5),
            Colors.Create("White"), Colors.Create("Gray"), Colors.Create("White")
        );
        var space2 = new LearningSpace(
            EntityName.Create("Aula 202"),
            LearningSpaceType.Create("Classroom"),
            Capacity.Create(30),
            Size.Create(3), Size.Create(4), Size.Create(5),
            Colors.Create("White"), Colors.Create("Gray"), Colors.Create("White")
        );
        context.LearningSpaces.AddRange(space1, space2);
        context.Entry(space1).Property("FloorId").CurrentValue = floor.FloorId;
        context.Entry(space2).Property("FloorId").CurrentValue = floor.FloorId;
        await context.SaveChangesAsync();

        // Act
        var result = await repo.ListLearningSpacesPaginatedAsync(floor.FloorId, 1, 0, "Aula");

        // Assert
        Assert.NotNull(result);
        Assert.True(result.TotalCount >= 2);
        Assert.Single(result);
        Assert.Contains(result, r => r.Name.Name.StartsWith("Aula"));
    }

    /// <summary>
    /// Tests that <see cref="ILearningSpaceRepository.ListLearningSpacesPaginatedAsync(int, int, int, string)"/> throws <see cref="NotFoundException"/> when the floor does not exist.
    /// </summary>
    [Fact]
    public async Task ListLearningSpacesPaginatedAsync_Should_Throw_When_Floor_Not_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ILearningSpaceRepository>();
        await Assert.ThrowsAsync<NotFoundException>(() => repo.ListLearningSpacesPaginatedAsync(-999, 1, 0, ""));
    }
}
