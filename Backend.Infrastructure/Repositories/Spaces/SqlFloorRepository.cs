using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.Spaces;

internal class SqlFloorRepository : IFloorRepository
{
    /// <summary>
    /// Database context for accessing the theme park database.
    /// </summary>
    private readonly ThemeParkDataBaseContext _dbContext;

    /// <summary>
    /// Logger for logging information and errors.
    /// </summary>
    private readonly ILogger<SqlFloorRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlFloorRepository"/> class.
    /// </summary>
    /// <param name="dbContext">Database context used for data access.</param>
    /// <param name="logger">Logger instance for logging.</param>
    public SqlFloorRepository(ThemeParkDataBaseContext dbContext, ILogger<SqlFloorRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new floor within a specified building.
    /// </summary>
    /// <param name="buildingId">The building identifier where the floor will be added.</param>
    /// <returns>True if the floor was created successfully; otherwise, an exception is thrown.</returns>
    /// <exception cref="NotFoundException">Thrown when the building does not exist.</exception>
    /// <exception cref="ConcurrencyConflictException">Thrown on concurrency conflicts during creation.</exception>
    /// <exception cref="DuplicatedEntityException">Thrown if a floor with the same number already exists.</exception>
    public async Task<bool> CreateFloorAsync(int buildingId)
    {
        var building = await _dbContext.Buildings.FirstOrDefaultAsync(b => b.BuildingInternalId == buildingId);
        if (building is null)
        {
            _logger.LogWarning("Building with ID '{BuildingId}' not found.", buildingId);
            throw new NotFoundException($"Building with ID '{buildingId}' not found.");
        }

        var maxFloorNumber = _dbContext.Floors
            .Where(f => EF.Property<int>(f, "BuildingId") == building.BuildingInternalId)
            .AsEnumerable()
            .Select(f => f.Number.Value)
            .DefaultIfEmpty(0)
            .Max();

        var newFloorNumber = maxFloorNumber + 1;
        var floor = new Floor(FloorNumber.Create(newFloorNumber));

        try
        {
            _dbContext.Entry(floor).Property("BuildingId").CurrentValue = building.BuildingInternalId;
            await _dbContext.Floors.AddAsync(floor);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Floor number '{FloorNumber}' created in building '{BuildingId}'.", newFloorNumber, buildingId);
            return true;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency conflict creating the new floor in the building '{BuildingId}'. It may have been updated or deleted by another user.", buildingId);
            throw new ConcurrencyConflictException($"Concurrency conflict creating the new floor in the building '{buildingId}'. It may have been updated or deleted by another user.");
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
        {
            _logger.LogWarning(ex, "Unable to create floor. The specified buildingId '{BuildingId}' does not exist.", buildingId);
            throw new NotFoundException($"Unable to create floor. The specified buildingId '{buildingId}' does not exist.");
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            throw new DuplicatedEntityException($"A floor with number '{newFloorNumber}' already exists in building '{buildingId}'.");
        }
    }

    /// <summary>
    /// Lists all floors available in a specified building.
    /// </summary>
    /// <param name="buildingId">The building identifier to list floors from.</param>
    /// <returns>A list of floors in the building.</returns>
    /// <exception cref="NotFoundException">Thrown if the building does not exist.</exception>
    /// <exception cref="DomainException">Thrown on database update errors.</exception>
    public async Task<List<Floor>?> ListFloorsAsync(int buildingId)
    {
        var message = string.Empty;

        try
        {
            var buildingExists = await _dbContext.Buildings.AnyAsync(b => b.BuildingInternalId == buildingId);

            if (!buildingExists)
            {
                _logger.LogInformation("Building with id '{BuildingId}' does not exist.", buildingId);
                throw new NotFoundException($"Building with id {buildingId} does not exist.");
            }

            var floors = await _dbContext.Floors.Where(f =>
                EF.Property<int>(f, "BuildingId") == buildingId).ToListAsync();

            return floors;
        }
        catch (DbUpdateException ex)
        {
            message = $"Database update error while listing floors for buildingId {buildingId}.";
            _logger.LogWarning(ex, message);
            throw new DbUpdateException(message);
        }
    }



    /// <summary>
    /// Lists all floors available in a building with pagination.
    /// </summary>
    /// <param name="buildingId">The identifier of the building.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <param name="pageIndex">The index of the current page</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a paginated list of <see cref="Floor"/> instances on the specified building.
    /// </returns>
    public async Task<PaginatedList<Floor>> ListFloorsPaginatedAsync(int buildingId, int pageSize, int pageIndex)
    {
        var message = string.Empty;

        try
        {
            var building = await _dbContext.Buildings.FirstOrDefaultAsync(b => b.BuildingInternalId == buildingId);

            if (building is null)
            {
                message = $"Building with id {buildingId} does not exist.";
                _logger.LogInformation(message);
                throw new NotFoundException(message);
            }

            // Get the query for floors on the specified building
            var query = _dbContext.Floors
                .AsNoTracking()
                .OrderBy(f => f.Number)
                .Where(f => EF.Property<int>(f, "BuildingId") == building.BuildingInternalId);

            // Get the total count of floors for this building
            var totalCount = await query.CountAsync();
            // Calculate offset for pagination
            var offset = pageSize * pageIndex;

            // Apply pagination
            var items = await query
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync();

            // Create and return the paginated list
            return new PaginatedList<Floor>(items, totalCount, pageSize, pageIndex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving floors for building with id '{BuildingId}'", buildingId);
            throw new DbUpdateException($"Error retrieving floors for building with id '{buildingId}'", ex);
        }
    }

    /// <summary>
    /// Deletes a floor by its identifier.
    /// </summary>
    /// <param name="floorId">The identifier of the floor to delete.</param>
    /// <returns>True if deletion was successful; otherwise, an exception is thrown.</returns>
    /// <exception cref="NotFoundException">Thrown if the floor or its building reference is not found.</exception>
    /// <exception cref="ConcurrencyConflictException">Thrown on concurrency conflicts during deletion.</exception>
    public async Task<bool> DeleteFloorAsync(int floorId)
    {
        var floor = await _dbContext.Floors.FirstOrDefaultAsync(f => f.FloorId == floorId);
        if (floor is null)
        {
            _logger.LogWarning("Floor with id '{FloorId}' not found.", floorId);
            throw new NotFoundException($"Floor with id '{floorId}' not found.");
        }

        var buildingId = _dbContext.Entry(floor).Property<int>("BuildingId").CurrentValue;
        var building = await _dbContext.Buildings.FirstOrDefaultAsync(b => b.BuildingInternalId == buildingId);

        if (building is null)
        {
            _logger.LogWarning("Building reference for floor '{FloorId}' not found.", floorId);
            throw new NotFoundException($"Building reference for floor '{floorId}' not found.");
        }

        try
        {
            _dbContext.Floors.Remove(floor);
            await _dbContext.SaveChangesAsync();

            var remainingFloors = await _dbContext.Floors
                .Where(f => EF.Property<int>(f, "BuildingId") == buildingId)
                .OrderBy(f => f.Number)
                .ToListAsync();

            for (int i = 0; i < remainingFloors.Count; ++i)
            {
                remainingFloors[i].ChangeFloorNumber(i + 1);
            }

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Floor with ID '{FloorId}' successfully deleted.", floorId);
            return true;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency conflict deleting the floor '{FloorId}' of building. It may have been updated or deleted by another user.", floorId);
            throw new ConcurrencyConflictException($"Concurrency conflict deleting the floor '{floorId}' of building. It may have been updated or deleted by another user.");
        }
    }
}
