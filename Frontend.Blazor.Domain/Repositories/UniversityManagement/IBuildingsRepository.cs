using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.UniversityManagement;

/// <summary>
/// This interface defines the operations performed related to Building entities in the system.
/// </summary>
public interface IBuildingsRepository
{
    /// <summary>
    ///  Asynchronously adds a new building to the data base
    /// </summary>
    /// <param name="building"> The building entity to be added </param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result bool indicates whether the building was successfully added.
    ///</returns>
    public Task<bool> AddBuildingAsync(Building building);

    /// <summary>
    /// Asynchronously update the information of an existing building in the data base
    /// </summary>
    /// <param name="building"> Building entity to be updated </param>
    /// <param name="buildingId"> Id of building entity to be updated </param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result bool indicates whether the building was successfully updated.
    ///</returns>
    public Task<bool> UpdateBuildingAsync(Building building, int buildingId);

    /// <summary>
    /// Display the details of a specific building by its unique identifier.
    /// </summary>
    /// <param name="buildingId"> Id of building entity to be displayed </param>
    /// <returns>
    /// A <see cref="Task"/> that completes to display the building if found.
    /// </returns>
    public Task<Building?> DisplayBuildingAsync(int buildingId);


    /// <summary>
    /// Retrieves a list of the existing buildings
    /// </summary>
    /// <returns>
    /// A <see cref="Task"/> that resolves to a collection of building entities.
    /// </returns>
    public Task<IEnumerable<Building>> ListBuildingAsync();

    /// <summary>
    /// Deletes a building in the data base
    /// </summary>
    /// <param name="buildingId"> ID of building entity to be deleted </param>
    /// <returns>
    /// A <see cref="Task"/> that represent the deleted building if true.
    ///</returns>
    public Task<bool> DeleteBuildingAsync(int buildingId);

    /// <summary>
    /// Retrieves a paginated list of buildings based on the specified parameters.
    /// </summary>
    /// <param name="pageSize">The maximum number of buildings to include in the result set. Must be greater than 0.</param>
    /// <param name="pageIndex">The page index for pagination.</param>    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="PaginatedList{T}"/> of
    /// <see cref="Building"/> objects representing the requested page of buildings.</returns>
    public Task<PaginatedList<Building>> ListBuildingPaginatedAsync(int pageSize, int pageIndex, string? search = null);
}