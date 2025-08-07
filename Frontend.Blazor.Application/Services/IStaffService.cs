using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;

/// <summary>
/// Defines the contract for Staff-related operations in the application.
/// </summary>
public interface IStaffService
{
    /// <summary>
    /// Creates a new Staff in the system.
    /// </summary>
    /// <param name="Staff"> The Staff object containing the details of the Staff to be created.</param>
    /// <returns> A task that represents the asynchronous operation. 
    /// The task result contains a boolean indicating success or failure.</returns>
    Task<bool> CreateStaffAsync(Staff Staff);

    /// <summary>
    /// Retrieves a list of all Staff members in the system.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a list of <see cref="Staff"/> objects.
    /// </returns>
    Task<List<Staff>> ListStaffAsync();

}
