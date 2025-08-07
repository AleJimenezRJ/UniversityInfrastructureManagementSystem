using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.AccountManagement;

/// <summary>
/// Defines the contract for accessing and managing person-related data in the repository layer.
/// </summary>
public interface IPersonRepository
{
    /// <summary>
    /// Retrieves a person by their identity number.
    /// </summary>
    /// <param name="identityNumber">The unique identity number of the person.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="Person"/> object
    /// if found, or <c>null</c> if no person with the specified identity number exists.
    /// </returns>
    Task<Person?> GetByIdAsync(string identityNumber);

    /// <summary>
    /// Creates a new person in the system.
    /// </summary>
    /// <param name="person"> The person object containing the details of the person to be created.</param>
    /// <returns> A task that represents the asynchronous operation. 
    /// The task result contains a boolean indicating success or failure.</returns>
    Task<bool> CreatePersonAsync(Person person);

    /// Retrieves all persons from the database.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="Person"/> objects
    /// if any exist, or an empty list if no persons are found in the database.
    Task<List<Person>> GetAllAsync();

    /// <summary>
    /// Deletes a person from the system by their identity number.
    /// </summary>
    /// <param name="identityNumber"></param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean indicating
    /// whether the deletion was successful or not.
    /// </returns>
    Task<bool> DeletePersonAsync(string identityNumber);

    /// <summary>
    /// Retrieves a person by their ID.
    /// </summary>
    /// <param name="identityNumber"></param>
    /// <param name="updatedPerson"></param>
    /// <returns></returns>
    Task<bool> UpdatePersonAsync(string identityNumber, Person updatedPerson);

}
