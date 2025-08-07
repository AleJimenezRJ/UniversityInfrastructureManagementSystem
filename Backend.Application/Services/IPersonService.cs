using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Services;

/// <summary>
/// Defines the contract for person-related operations in the application.
/// </summary>
public interface IPersonService
{
    /// <summary>
    /// Retrieves a person by their identity number.
    /// </summary>
    /// <param name="identityNumber">The unique identity number of the person.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="Person"/> object
    /// if found, or <c>null</c> if no person with the specified identity number exists.
    /// </returns>
    Task<Person?> GetPersonByIdAsync(string identityNumber);

    /// <summary>
    /// Creates a new person in the system.
    /// </summary>
    /// <param name="person"> The person object containing the details of the person to be created.</param>
    /// <returns> A task that represents the asynchronous operation. 
    /// The task result contains a boolean indicating success or failure.</returns>
    Task<bool> CreatePersonAsync(Person person);
   
    /// <summary>
    /// Retrieves a list of people
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task contains a list of <see cref="Person"/> objects if found,
    /// or an empty list if no person exists in the data base.
    /// </returns>
    Task<List<Person>> GetAllPeopleAsync();

    /// <summary>
    /// Deletes a person from the system by their identity number.
    /// </summary>
    /// <param name="identityNumber"></param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean indicating success or failure.
    /// </returns>
    Task<bool> DeletePersonAsync(string identityNumber);

    /// <summary>
    /// Modifies the details of an existing person in the system.
    /// </summary>
    /// <param name="identityNumber">The unique identity number of the person to be modified.</param>
    /// <param name="updatedPerson">The updated <see cref="Person"/> object containing the new details.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean indicating 
    /// whether the modification was successful (<c>true</c>) or not (<c>false</c>).
    /// </returns>
    Task<bool> ModifyPersonAsync(string identityNumber, Person updatedPerson);

}
