namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

/// <summary>
/// Repository interface for managing staff entities.
/// This interface defines the contract for the staff repository.
/// </summary>
public interface IStaffRepository
{
    /// <summary>
    /// Creates a new person in the system.
    /// </summary>
    /// <param name="staff"> The person object containing the details of the person to be created.</param>
    /// <returns> A task that represents the asynchronous operation. 
    /// The task result contains a boolean indicating success or failure.</returns>
    Task<bool> CreateStaffAsync(Staff staff);


    /// <summary>
    /// Retrieves a list of all staff members in the system.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a list of <see cref="Staff"/> objects, or null if no staff members are found.
    /// </returns>
    Task<List<Staff>?> ListStaffAsync();
}