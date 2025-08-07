using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories;
namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;

/// <summary>
/// Implementation of <see cref="IPersonRepository"/> that interacts with the SQL database using EF Core.
/// </summary>
internal class SqlPersonRepository : IPersonRepository
{
    private readonly ThemeParkDataBaseContext _dbContext;
    private readonly ILogger<SqlPersonRepository> _logger;

    /// <summary>
    /// Constructor for <see cref="SqlPersonRepository"/>.
    /// </summary>
    /// <param name="dbContext">Injected database context for accessing data.</param>
    /// <param name="logger">Logger for recording operational messages and errors.</param>
    public SqlPersonRepository(ThemeParkDataBaseContext dbContext, ILogger<SqlPersonRepository> logger)
    => (_dbContext, _logger) = (dbContext, logger);

    /// <summary>
    /// Creates a new <see cref="Person"/> entry in the database.
    /// </summary>
    /// <param name="person">The person entity to insert.</param>
    /// <returns>True if creation was successful; false otherwise.</returns>
    public async Task<bool> CreatePersonAsync(Person person)
    {
        try
        {
            bool exists = await _dbContext.Persons.AnyAsync(p =>
              p.IdentityNumber == person.IdentityNumber ||
              p.Email == person.Email);


            if (exists)
            {
                _logger.LogWarning("Attempt to create person with existing identity number or email: {IdentityNumber}, {Email}", person.IdentityNumber, person.Email);
                throw new DuplicatedEntityException($"Attempt to create person with existing identity number or email");
            }

            // If the person does not exist, add it to the database
            _dbContext.Persons.Add(person);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully created person with Id: {Id}", person.Id);
            return true;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Error creating person: {Message}", ex.Message);
            throw new DomainException($"An unexpected error occurred while creating the person: {ex.Message}");
        }
        catch (DbUpdateException dbEx) // Handle database update issues
        {
            _logger.LogError(dbEx, "Database update error while creating person: {Message}", dbEx.Message);
            throw new DomainException($"A database update error occurred while creating the person: {dbEx.Message}");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Operation was canceled while creating person: {Message}", ex.Message);
            throw new DomainException($"The operation was canceled while creating the person: {ex.Message}");
        }
        catch (DuplicatedEntityException ex)
        {
            _logger.LogError(ex, "Duplicated entity error while creating person: {Message}", ex.Message);
            throw new DomainException($"A person with the same identity number or email already exists: {ex.Message}");
        }



    }

    /// <summary>
    /// Retrieves a person by their identity number.
    /// </summary>
    /// <param name="identityNumber">The identity number to search for.</param>
    /// <returns>The matched <see cref="Person"/> or null if not found.</returns>
    /// <exception cref="NotImplementedException">This method is not yet implemented.</exception>
    public async Task<Person?> GetByIdAsync(string identityNumber)
    {
        try
        {
            var person = await _dbContext.Persons
                .FirstOrDefaultAsync(p => p.IdentityNumber.Value == identityNumber);


            if (person is null)
            {
                _logger.LogWarning("Person with identity number {IdentityNumber} not found.", identityNumber);
                throw new NotFoundException($"Person with identity number {identityNumber} was not found.");
            }

            _logger.LogInformation("Person with identity number {IdentityNumber} retrieved.", identityNumber);
            return person;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Database context is null when retrieving person by identity number: {IdentityNumber}", identityNumber);
            throw new DomainException($"An unexpected error occurred while retrieving the person: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Operation was canceled while retrieving person by identity number: {IdentityNumber}", identityNumber);
            throw new DomainException($"The operation was canceled while retrieving the person: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves all people registered in the database.
    /// </summary>
    /// <returns>A list of <see cref="Person"/> entries; empty list if an error occurs.</returns>
    public async Task<List<Person>> GetAllAsync()
    {
        try
        {
            var people = await _dbContext.Persons.ToListAsync();
            if (people.Count == 0)
            {
                _logger.LogWarning("No people found in the database.");
                throw new NotFoundException("No people found in the database.");
            }
            _logger.LogInformation("Retrieved {Count} people from database", people.Count);
            return people;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving people from database");
            throw new DomainException($"An unexpected error occurred while retrieving people: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a person from the database by their identity number.
    /// </summary>
    /// <param name="identityNumber"></param>
    /// <returns>
    /// A boolean indicating whether the deletion was successful.
    /// </returns>
    public async Task<bool> DeletePersonAsync(string identityNumber)
    {
        try
        {
            var person = await _dbContext.Persons
                .FirstOrDefaultAsync(p => p.IdentityNumber == IdentityNumber.Create(identityNumber));

            if (person is null)
            {
                _logger.LogWarning("Person with identity number {IdentityNumber} not found.", identityNumber);
                throw new NotFoundException($"Person with identity number {identityNumber} was not found.");
            }

            // Remove the person entity
            _dbContext.Persons.Remove(person);

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Person with identity number {IdentityNumber} deleted.", identityNumber);
            return true;
        }
        catch (DbUpdateException dbEx) // Handle database update issues
        {
            _logger.LogError(dbEx, "Database update error while deleting person with identity number {IdentityNumber}", identityNumber);
            throw new DomainException($"A database update error occurred while deleting the person: {dbEx.Message}");
        }

    }

    /// <summary>
    /// Updates an existing person in the database.
    /// </summary>
    /// <param name="identityNumber"></param>
    /// <param name="updatedPerson"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="DuplicatedEntityException"></exception>
    private async Task<bool> UpdateExistingPersonAsync(string identityNumber, Person updatedPerson)
    {
        var existingPerson = await _dbContext.Persons
                .FirstOrDefaultAsync(p => p.IdentityNumber == IdentityNumber.Create(identityNumber));

        if (existingPerson == null)
        {
            _logger.LogWarning("Person with IdentityNumber {IdentityNumber} not found.", identityNumber);
            throw new NotFoundException($"Person with identity number {identityNumber} was not found.");
        }

        if (updatedPerson.FirstName != "string" && !string.IsNullOrWhiteSpace(updatedPerson.FirstName))
        {
            existingPerson.FirstName = updatedPerson.FirstName;
        }

        if (updatedPerson.LastName != "string" && !string.IsNullOrWhiteSpace(updatedPerson.LastName))
        {
            existingPerson.LastName = updatedPerson.LastName;
        }

        if (updatedPerson.Phone.Value != "string" && !string.IsNullOrWhiteSpace(updatedPerson.Phone.Value))
        {
            existingPerson.Phone = updatedPerson.Phone;
        }

        if (updatedPerson.BirthDate != default && updatedPerson.BirthDate.Value != DateOnly.FromDateTime(DateTime.Now))
        {
            existingPerson.BirthDate = updatedPerson.BirthDate;
        }

        if (updatedPerson.Email.Value != "string" && !string.IsNullOrWhiteSpace(updatedPerson.Email.Value))
        {
            bool emailExists = await _dbContext.Persons.AnyAsync(p => p.Email == updatedPerson.Email && p.IdentityNumber != existingPerson.IdentityNumber);
            if (emailExists)
            {
                _logger.LogWarning("Attempt to update person with duplicate email: {Email}", updatedPerson.Email.Value);
                throw new DuplicatedEntityException($"A person with the email '{updatedPerson.Email.Value}' already exists.");
            }
            existingPerson.Email = updatedPerson.Email;
        }

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Person with IdentityNumber {IdentityNumber} successfully updated.", identityNumber);

        return true;
    }

    /// <summary>
    /// Updates the details of an existing person in the database.
    /// </summary>
    /// <param name="identityNumber">
    /// The unique identity number of the person to be updated.
    /// </param>
    /// <param name="updatedPerson">
    /// A <see cref="Person"/> object containing the updated details of the person.
    /// Only non-default and non-empty values will be applied to the existing record.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean indicating
    /// whether the update was successful (<c>true</c>) or not (<c>false</c>).
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown if an unexpected error occurs during the update process.
    /// </exception>
    public async Task<bool> UpdatePersonAsync(string identityNumber, Person updatedPerson)
    {
        try
        {
            return await UpdateExistingPersonAsync(identityNumber, updatedPerson);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Database context is null when updating person with identity number: {IdentityNumber}", identityNumber);
            throw new DomainException($"An unexpected error occurred while updating the person: {ex.Message}");
        }
        catch (DbUpdateException dbEx) // Handle database update issues
        {
            _logger.LogError(dbEx, "Database update error while updating person with identity number {IdentityNumber}", identityNumber);
            throw new DomainException($"A database update error occurred while updating the person: {dbEx.Message}");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Operation was canceled while updating person with identity number: {IdentityNumber}", identityNumber);
            throw new DomainException($"The operation was canceled while updating the person: {ex.Message}");
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, "Person not found while updating: {Message}", ex.Message);
            throw new DomainException($"The person with identity number {identityNumber} was not found: {ex.Message}");
        }
        catch (DuplicatedEntityException ex)
        {
            _logger.LogError(ex, "Duplicated entity error while updating person: {Message}", ex.Message);
            throw new DomainException($"A person with the same email already exists: {ex.Message}");
        }
    }
}
