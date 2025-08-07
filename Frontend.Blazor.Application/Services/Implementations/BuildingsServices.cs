using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;

/// <summary>
/// Implements the <see cref="IBuildingsServices"/> interface, providing methods to manage building-related operations.
/// </summary>
internal class BuildingsServices : IBuildingsServices
{
    /// <summary>
    /// The repository used to perform operations related to buildings.
    /// </summary>
    private readonly IBuildingsRepository _buildingsRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="BuildingsServices"/> class.
    /// </summary>
    /// <param name="buildingRepository">The repository used to manage building data.</param>
    public BuildingsServices(IBuildingsRepository buildingRepository)
    {
        _buildingsRepository = buildingRepository;
    }

    /// <summary>
    /// Adds a new building to the data store asynchronously.
    /// </summary>
    /// <param name="building">The building to add.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result indicates whether the building was successfully added.
    /// </returns>
    public Task<bool> AddBuildingAsync(Building building)
    {
        return _buildingsRepository.AddBuildingAsync(building);
    }

    /// <summary>
    /// Updates the information of an existing building asynchronously.
    /// </summary>
    /// <param name="building">The building with updated data.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result indicates whether the update was successful.
    /// </returns>
    public Task<bool> UpdateBuildingAsync(Building building, int buildingId)
    {
        return _buildingsRepository.UpdateBuildingAsync(building, buildingId);
    }

    /// <summary>
    /// Retrieves the information of a specific building by its id.
    /// </summary>
    /// <param name="buildingId">The unique id of the building.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the building if found; otherwise, null.
    /// </returns>
    public Task<Building?> DisplayBuildingAsync(int buildingId)
    {
        return _buildingsRepository.DisplayBuildingAsync(buildingId);
    }

    /// <summary>
    /// Retrieves a list of all existing buildings asynchronously.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a collection of buildings.
    /// </returns>
    public Task<IEnumerable<Building>> ListBuildingAsync()
    {
        return _buildingsRepository.ListBuildingAsync();
    }

    /// <summary>
    /// Perfoms the operation of delete a building asynchronously.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result if true contains correct deletation message.
    /// </returns>
    public Task<bool> DeleteBuildingAsync(int buildingId)
    {
        return _buildingsRepository.DeleteBuildingAsync(buildingId);
    }

    /// <summary>
    /// Retrieves a paginated list of buildings based on the specified parameters.
    /// </summary>
    /// <remarks>This method is designed to support paginated retrieval of buildings. The <paramref
    /// <paramref name="pageSize"/> is set appropriately to balance performance and data transfer size.</remarks>
    /// <param name="pageSize">The maximum number of buildings to include in the result set. Must be a positive integer.</param>
    /// <param name="pageIndex">The page index for pagination.</param>    /// to retrieve the first page.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="PaginatedList{T}"/> of
    /// <see cref="Building"/> objects representing the paginated list of buildings.</returns>
    public Task<PaginatedList<Building>> GetBuildingsListPaginatedAsync(int pageSize, int pageIndex, string? search = null)
    {
        return _buildingsRepository.ListBuildingPaginatedAsync(pageSize, pageIndex, search);
    }
}