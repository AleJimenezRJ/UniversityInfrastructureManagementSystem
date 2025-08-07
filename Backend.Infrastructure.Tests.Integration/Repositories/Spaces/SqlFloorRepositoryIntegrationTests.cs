using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Integration.Repositories.Spaces;

[Collection("Database collection")]
public class SqlFloorRepositoryIntegrationTests
{
    private readonly IServiceProvider _serviceProvider;

    public SqlFloorRepositoryIntegrationTests(IntegrationTestFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }

    /// <summary>
    /// Tests that CreateFloorAsync creates a floor when the building exists.
    /// </summary>
    [Fact]
    public async Task CreateFloorAsync_Should_Create_Floor_When_Building_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IFloorRepository>();

        var university = new University(EntityName.Create("UCR_Floor"), EntityLocation.Create("CR_Floor"));
        var campus = new Campus(EntityName.Create("Rodrigo_Floor"), EntityLocation.Create("San Pedro_Floor"), university);
        var area = new Area(EntityName.Create("Finca_Floor"), campus);
        var building = new Building(
            EntityName.Create("Eng Hall_Floor"),
            Coordinates.Create(1, 1, 0),
            Dimension.Create(10, 10, 3),
            Colors.Create("Blue"),
            area
        );
        context.AddRange(university, campus, area, building);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.CreateFloorAsync(building.BuildingInternalId);

        // Assert
        Assert.True(result);
        var fromDb = context.Floors.ToList().FirstOrDefault(f => context.Entry(f).Property<int>("BuildingId").CurrentValue == building.BuildingInternalId);
        Assert.NotNull(fromDb);
        Assert.Equal(1, fromDb.Number.Value);
    }

    /// <summary>
    /// Tests that CreateFloorAsync throws NotFoundException when the building does not exist.
    /// </summary>
    [Fact]
    public async Task CreateFloorAsync_Should_Throw_When_Building_Not_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IFloorRepository>();
        await Assert.ThrowsAsync<NotFoundException>(() => repo.CreateFloorAsync(-999));
    }

    /// <summary>
    /// Tests that ListFloorsAsync returns all floors for a building.
    /// </summary>
    [Fact]
    public async Task ListFloorsAsync_Should_Return_All_Floors_For_Building()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IFloorRepository>();

        var university = new University(EntityName.Create("UCR_Floor2"), EntityLocation.Create("CR_Floor2"));
        var campus = new Campus(EntityName.Create("Rodrigo_Floor2"), EntityLocation.Create("San Pedro_Floor2"), university);
        var area = new Area(EntityName.Create("Finca_Floor2"), campus);
        var building = new Building(
            EntityName.Create("Eng Hall_Floor2"),
            Coordinates.Create(1, 1, 0),
            Dimension.Create(10, 10, 3),
            Colors.Create("Blue"),
            area
        );
        context.AddRange(university, campus, area, building);
        await context.SaveChangesAsync();
        // Add two floors
        var floor1 = new Floor(FloorNumber.Create(1));
        var floor2 = new Floor(FloorNumber.Create(2));
        context.Floors.AddRange(floor1, floor2);
        context.Entry(floor1).Property("BuildingId").CurrentValue = building.BuildingInternalId;
        context.Entry(floor2).Property("BuildingId").CurrentValue = building.BuildingInternalId;
        await context.SaveChangesAsync();

        // Act
        var result = await repo.ListFloorsAsync(building.BuildingInternalId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, f => f.Number.Value == 1);
        Assert.Contains(result, f => f.Number.Value == 2);
    }

    /// <summary>
    /// Tests that ListFloorsAsync throws NotFoundException when the building does not exist.
    /// </summary>
    [Fact]
    public async Task ListFloorsAsync_Should_Throw_When_Building_Not_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IFloorRepository>();
        await Assert.ThrowsAsync<NotFoundException>(() => repo.ListFloorsAsync(-999));
    }

    /// <summary>
    /// Tests that ListFloorsPaginatedAsync returns a paginated list of floors.
    /// </summary>
    [Fact]
    public async Task ListFloorsPaginatedAsync_Should_Return_Paginated_List()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IFloorRepository>();

        var university = new University(EntityName.Create("UCR_Floor3"), EntityLocation.Create("CR_Floor3"));
        var campus = new Campus(EntityName.Create("Rodrigo_Floor3"), EntityLocation.Create("San Pedro_Floor3"), university);
        var area = new Area(EntityName.Create("Finca_Floor3"), campus);
        var building = new Building(
            EntityName.Create("Eng Hall_Floor3"),
            Coordinates.Create(1, 1, 0),
            Dimension.Create(10, 10, 3),
            Colors.Create("Blue"),
            area
        );
        context.AddRange(university, campus, area, building);
        await context.SaveChangesAsync();
        // Add three floors
        var floor1 = new Floor(FloorNumber.Create(1));
        var floor2 = new Floor(FloorNumber.Create(2));
        var floor3 = new Floor(FloorNumber.Create(3));
        context.Floors.AddRange(floor1, floor2, floor3);
        context.Entry(floor1).Property("BuildingId").CurrentValue = building.BuildingInternalId;
        context.Entry(floor2).Property("BuildingId").CurrentValue = building.BuildingInternalId;
        context.Entry(floor3).Property("BuildingId").CurrentValue = building.BuildingInternalId;
        await context.SaveChangesAsync();

        // Act
        var result = await repo.ListFloorsPaginatedAsync(building.BuildingInternalId, 2, 0);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, f => f.Number.Value == 1);
        Assert.Contains(result, f => f.Number.Value == 2);
    }

    /// <summary>
    /// Tests that ListFloorsPaginatedAsync throws NotFoundException when the building does not exist.
    /// </summary>
    [Fact]
    public async Task ListFloorsPaginatedAsync_Should_Throw_When_Building_Not_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IFloorRepository>();
        await Assert.ThrowsAsync<NotFoundException>(() => repo.ListFloorsPaginatedAsync(-999, 2, 0));
    }

    /// <summary>
    /// Tests that DeleteFloorAsync deletes a floor when it exists.
    /// </summary>
    [Fact]
    public async Task DeleteFloorAsync_Should_Delete_Floor_When_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ThemeParkDataBaseContext>();
        var repo = scope.ServiceProvider.GetRequiredService<IFloorRepository>();

        var university = new University(EntityName.Create("UCR_Floor4"), EntityLocation.Create("CR_Floor4"));
        var campus = new Campus(EntityName.Create("Rodrigo_Floor4"), EntityLocation.Create("San Pedro_Floor4"), university);
        var area = new Area(EntityName.Create("Finca_Floor4"), campus);
        var building = new Building(
            EntityName.Create("Eng Hall_Floor4"),
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

        // Act
        var result = await repo.DeleteFloorAsync(floor.FloorId);

        // Assert
        Assert.True(result);
        var fromDb = context.Floors.FirstOrDefault(f => f.FloorId == floor.FloorId);
        Assert.Null(fromDb);
    }

    /// <summary>
    /// Tests that DeleteFloorAsync throws NotFoundException when the floor does not exist.
    /// </summary>
    [Fact]
    public async Task DeleteFloorAsync_Should_Throw_When_Floor_Not_Exists()
    {
        using var scope = _serviceProvider.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IFloorRepository>();
        await Assert.ThrowsAsync<NotFoundException>(() => repo.DeleteFloorAsync(-999));
    }
}
