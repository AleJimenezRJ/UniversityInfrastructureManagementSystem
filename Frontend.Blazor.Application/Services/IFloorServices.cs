using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;

/// <summary>
/// Provides business operations related to floors within buildings,
/// including validation and orchestration of floor management use cases.
/// </summary>
public interface IFloorServices
{
    /// <summary>
    /// Retrieves a list with the floors in a specific building.
    /// </summary>
    /// <param name="buildingId">The internal Id of the building.</param>
    /// <returns>
    /// A list of <see cref="Floor"/> entities for the specified building,
    /// </returns>
    public Task<List<Floor>?> GetFloorsListAsync(int buildingId);

    /// <summary>
    /// Retrieves a paginated list of floors in a specific building.
    /// </summary>
    /// <param name="buildingId">The internal Id of the building.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <param name="pageIndex">The index of the current page (1-based).</param>
    /// <returns>
    /// A paginated list of <see cref="Floor"/> entities for the specified building.
    /// </returns>
    public Task<PaginatedList<Floor>?> GetFloorsListPaginatedAsync(int buildingId, int pageSize, int pageIndex);

    /// <summary>
    /// Deletes a specific floor in a building.
    /// </summary>
    /// <param name="floorId">The specific identifier of the floor that will be deleted.</param>
    /// <returns>
    /// A <see cref="Task"/> result that is true if the floor was successfully deleted;
    /// otherwise, false (for example, if the building or floor does not exist).
    /// </returns>
    public Task<bool> DeleteFloorAsync(int floorId);

    /// <summary>
    /// Creates a new floor in a building.
    /// </summary>
    /// <param name="buildingId">The internal Id of the building where the floor will be created.</param>
    /// <returns>
    /// A <see cref="Task"/> result that is true if the floor was successfully created;
    /// otherwise, false.
    /// </returns>
    public Task<bool> CreateFloorAsync(int buildingId);
}
