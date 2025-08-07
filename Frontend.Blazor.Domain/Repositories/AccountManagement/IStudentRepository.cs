using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.AccountManagement;

/// <summary>
/// Interface for the Student repository.
/// This interface defines the contract for the Student repository.
/// </summary>
public interface IStudentRepository
{
    /// <summary>
    /// Creates a new person in the system.
    /// </summary>
    /// <param name="student"> The person object containing the details of the person to be created.</param>
    /// <returns> A task that represents the asynchronous operation. 
    /// The task result contains a boolean indicating success or failure.</returns>
    Task<bool> CreateStudentAsync(Student student);

    /// <summary>
    /// Retrieves a list of all students registered in the system.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains a list of <see cref="Student"/> objects,
    /// or <c>null</c> if an error occurs during retrieval.
    /// </returns>
    Task<List<Student>?> ListStudentsAsync();
}