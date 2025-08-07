using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;

internal class FloorServices : IFloorServices
{
    /// <summary>
    /// Repository for managing Floor entities.
    /// </summary>
    private readonly IFloorRepository _floorRepository;

    /// <summary>
    /// Constructor for the FloorServices class.
    /// </summary>
    /// <param name="floorRepository">The repository instance used for data persistence operations.</param>
    public FloorServices(IFloorRepository floorRepository)
    {
        _floorRepository = floorRepository;
    }

    /// <summary>
    /// Creates a new floor in a building.
    /// </summary>
    /// <param name="buildingId">The internal Id of the building where the floor will be created.</param>
    /// <returns>
    ///  A <see cref="Task"/> result that is true if the floor was successfully created;
    ///  otherwise, false.
    ///  </returns>
    public Task<bool> CreateFloorAsync(int buildingId)
    {
        return _floorRepository.CreateFloorAsync(buildingId);
    }

    /// <summary>
    /// Deletes a specific floor in a building.
    /// </summary>
    /// <param name="floorId">The specific identifier of the floor that will be deleted.</param>
    /// <returns>
    /// A <see cref="Task"/> that returns true if the operation was successful; otherwise, false.
    /// </returns>
    public Task<bool> DeleteFloorAsync(int floorId)
    {
        return _floorRepository.DeleteFloorAsync(floorId);
    }
    /// <summary>
    /// Lists all floors available in a building.
    /// </summary>
    /// <param name="buildingId">The internal Id of the building.</param>
    /// <returns>
    /// A list of <see cref="Floor"/> entities for the specified building,
    /// </returns>
    public Task<List<Floor>?> GetFloorsListAsync(int buildingId)
    {
        return _floorRepository.ListFloorsAsync(buildingId);
    }

    /// <summary>
    /// Retrieves a paginated list of floors in a specific building.
    /// </summary>
    /// <param name="buildingId">The internal Id of the building.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <param name="pageIndex">The index of the current page (1-based).</param>
    /// <returns>
    /// A paginated list of <see cref="Floor"/> entities for the specified building.
    /// </returns>
    public Task<PaginatedList<Floor>?> GetFloorsListPaginatedAsync(int buildingId, int pageSize, int pageIndex)
    {
        return _floorRepository.ListFloorsPaginatedAsync(buildingId, pageSize, pageIndex);
    }
}
