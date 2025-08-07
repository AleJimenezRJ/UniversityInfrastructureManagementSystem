using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;

/// <summary>
/// Provides implementation for person-related operations defined in <see cref="IPersonService"/>.
/// </summary>
internal class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;


    /// <summary>
    /// Initializes a new instance of the <see cref="PersonService"/> class.
    /// </summary>
    /// <param name="personRepository">The repository used to access person data.</param>
    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    /// <summary>
    /// Retrieves a person by their identity number.
    /// </summary>
    /// <param name="identityNumber">The unique identity number of the person.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="Person"/> object
    /// if found, or <c>null</c> if no person with the specified identity number exists.
    /// </returns>
    public Task<Person?> GetPersonByIdAsync(string identityNumber)
    {
        return _personRepository.GetByIdAsync(identityNumber);
    }

    /// <summary>
    /// Creates a new person in the system.
    /// </summary>
    /// <param name="person"> The person object containing the details of the person to be created.</param>
    /// <returns> A task that represents the asynchronous operation. 
    /// The task result contains a boolean indicating success or failure.</returns>
    public Task<bool> CreatePersonAsync(Person person)
    {
        return _personRepository.CreatePersonAsync(person);
    }

    /// <summary>
    /// Retrieves a list of all people in the database.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="Person"/> objects
    /// if found, or an empty list if no person exists in the database.
    /// </returns>
    public Task<List<Person>> GetAllPeopleAsync()
    {
        return _personRepository.GetAllAsync();
    }

    /// <summary>
    /// Deletes a person from the system by their identity number.
    /// </summary>
    /// <param name="identityNumber"></param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean indicating
    /// whether the deletion was successful or not.
    /// </returns>
    public Task<bool> DeletePersonAsync(string identityNumber)
    {
        return _personRepository.DeletePersonAsync(identityNumber);
    }

    /// <summary>
    /// Modifies the details of an existing person in the system.
    /// </summary>
    /// <param name="identityNumber">The unique identity number of the person to be modified.</param>
    /// <param name="updatedPerson">The updated <see cref="Person"/> object containing the new details.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean indicating 
    /// whether the modification was successful (<c>true</c>) or not (<c>false</c>).
    /// </returns>
    public async Task<bool> ModifyPersonAsync(string identityNumber, Person updatedPerson)
    {
        return await _personRepository.UpdatePersonAsync(identityNumber, updatedPerson);
    }
}
