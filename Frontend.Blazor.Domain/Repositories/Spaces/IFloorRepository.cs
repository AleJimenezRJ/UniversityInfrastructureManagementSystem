using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.Spaces;

/// <summary>
/// Defines the contract for the Floor repository, which provides methods to manage and persist
/// <see cref="Floor"/> entities in the system.
/// </summary>
public interface IFloorRepository
{
    /// <summary>
    /// Adds a new floor to a building.
    /// </summary>
    /// <param name="buildingId">The internal Id of a building</param>
    /// <returns>A <see cref="Task"/> result that is true if the floor was successfully created;
    /// otherwise, false. </returns>
    public Task<bool> CreateFloorAsync(int buildingId);
    /// <summary>
    /// List all floors available in a building.
    /// </summary>
    /// <param name="buildingId">The internal Id of the building.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="Floor"/>
    /// entities in the specified building, or <c>null</c> if none are found.
    /// </returns>
    public Task<List<Floor>?> ListFloorsAsync(int buildingId);

    /// <summary>
    /// Lists floors available in a building with pagination.
    /// </summary>
    /// <param name="buildingId">The internal Id of the building.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <param name="pageIndex">The index of the current page (1-based).</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a paginated list of <see cref="Floor"/>
    /// entities in the specified building.
    /// </returns>
    public Task<PaginatedList<Floor>?> ListFloorsPaginatedAsync(int buildingId, int pageSize, int pageIndex);

    /// <summary>
    /// Deletes a specific floor in a building.
    /// </summary>
    /// <param name="floorId">The internal Id of the floor entity</param>
    /// <returns>
    /// A <see cref="Task"/> result that is true if the floor was successfully deleted;
    /// otherwise, false.
    /// </returns>
    public Task<bool> DeleteFloorAsync(int floorId);
}
