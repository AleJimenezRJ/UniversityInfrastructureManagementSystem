using System.Collections.Generic;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.ComponentsManagement;
using Xunit;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Tests.Unit.Repositories.ComponentsManagement;
/// <summary>
/// Unit tests for the Whiteboard repository.
/// Theres not an specific PBI for listing whiteboards, but it is a common functionality in the system. #9
/// Tasks: Add unit tests for the SqlWhiteboardRepository method GetAllAsync.
/// Participants: Sebastian Arce, Marcelo Picado
/// </summary>

public class SqlWhiteboardRepositoryTests : IDisposable
{
    private readonly DbContextOptions<ThemeParkDataBaseContext> options;
    private readonly List<Whiteboard> whiteboards;
    private readonly ThemeParkDataBaseContext context;
    private readonly SqlWhiteboardRepository repository;
    private readonly Mock<ILogger<SqlWhiteboardRepository>> logger;
    private readonly Whiteboard deletedWhiteboard;
    ///
    public SqlWhiteboardRepositoryTests()
    {
        options = new DbContextOptionsBuilder<ThemeParkDataBaseContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;
        context = new ThemeParkDataBaseContext(options);
        logger = new Mock<ILogger<SqlWhiteboardRepository>>();
        repository = new(context, logger.Object);
        whiteboards = new List<Whiteboard>
       {
           new Whiteboard(
               Colors.Create("White"),
               Orientation.Create("South"),
               Coordinates.Create(1, 2, 3),
               Dimension.Create(10, 5, 1)
           ) { ComponentId = 1, IsDeleted = false, DisplayId = "WB-1"},

           new Whiteboard(
               Colors.Create("Black"),
               Orientation.Create("North"),
               Coordinates.Create(4, 5, 6),
               Dimension.Create(8, 4, 1)
           ) { ComponentId = 2, IsDeleted = false, DisplayId = "WB-2"},

           new Whiteboard(
               Colors.Create("Green"),
               Orientation.Create("West"),
               Coordinates.Create(7, 8, 9),
               Dimension.Create(12, 6, 1)
           ) { ComponentId = 3, IsDeleted = true, DisplayId = "WB-3"}
       };
        deletedWhiteboard = new Whiteboard(
           Colors.Create("Red"),
           Orientation.Create("East"),
           Coordinates.Create(10, 11, 12),
           Dimension.Create(15, 7, 1)
       )
        { ComponentId = 4, IsDeleted = true, DisplayId = "WB-4"};
    }

    /// <summary>
    /// Disposes the resources used by the SqlWhiteboardRepositoryTests class.
    /// </summary>
    public void Dispose()
    {
        context?.Dispose();
    }

    /// <summary>  
    /// Tests the GetAllAsync method of the SqlWhiteboardRepository class.  
    /// </summary>  
    /// <returns> should return only non-deleted whiteboards from the database.  
    /// </returns>  
    [Fact]
    public async Task GetAllAsync_ShouldReturnOnlyNonDeletedWhiteboards()
    {
        // Arrange
        context.Whiteboards.AddRange(whiteboards);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        result.Should().NotBeNull(because: "there are non-deleted whiteboards");
        result.Should().OnlyContain(w => !w.IsDeleted);
    }

    /// <summary>
    /// Tests the GetAllAsync method of the SqlWhiteboardRepository class.
    /// </summary>
    /// <returns> should return an empty list when no whiteboards exist in the database.
    /// </returns>
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoWhiteboardsExist()
    {
        var result = await repository.GetAllAsync();
        // Assert
        result.Should().BeEmpty(because: "No whiteboards exist in the database");
    }

    /// <summary>
    /// Tests the GetAllAsync method of the SqlWhiteboardRepository class.
    /// </summary>
    /// <returns> should return an empty list when all whiteboards are marked as deleted.
    /// </returns>
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenAllWhiteboardsAreDeleted()
    {

        context.Whiteboards.Add(deletedWhiteboard);
        await context.SaveChangesAsync();
        // Act
        var result = await repository.GetAllAsync();
        // Assert
        result.Should().BeEmpty(because: "All whiteboards are marked as deleted");
    }
}
