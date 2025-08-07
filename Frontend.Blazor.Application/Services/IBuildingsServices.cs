using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;

/// <summary>
/// Defines the contract for building-related services.
/// </summary>
public interface IBuildingsServices
{
    /// <summary>
    /// Retrieves a paginated list of buildings based on the specified parameters.
    /// </summary>
    /// <param name="pageSize">The maximum number of buildings to include in the result set. Must be greater than 0.</param>
    /// <param name="pageIndex">The page index for pagination.</param>    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="PaginatedList{T}"/> of
    /// <see cref="Building"/> objects representing the requested page of buildings.</returns>
    public Task<PaginatedList<Building>> GetBuildingsListPaginatedAsync(int pageSize, int pageIndex, string? search = null);


    /// <summary>
    /// Adds a new building asynchronously.
    /// </summary>
    /// <param name="building">The building to add.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a value indicating whether the building was successfully added.
    /// </returns>
    public Task<bool> AddBuildingAsync(Building building);

    /// <summary>
    /// Updates an existing building asynchronously.
    /// </summary>
    /// <param name="building">The building with updated information.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a value indicating whether the building was successfully updated.
    /// </returns>
    public Task<bool> UpdateBuildingAsync(Building building, int buildingId);

    /// <summary>
    /// Retrieves the information of a specific building by its id.
    /// </summary>
    /// <param name="buildingId">The unique id of the building.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the building if found; otherwise, null.
    /// </returns>
    public Task<Building?> DisplayBuildingAsync(int buildingId);

    /// <summary>
    /// Retrieves a list of all existing buildings asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection of buildings.
    /// </returns>
    public Task<IEnumerable<Building>> ListBuildingAsync();

    /// <summary>
    /// Deletes a building from the database.
    /// </summary>
    /// <param name="buildingName">The building to be deleted.</param>
    /// <returns>
    /// The task result contains a boolean indicating success or failure in the delete operation.
    /// </returns>
    public Task<bool> DeleteBuildingAsync(int buildingId);
}
