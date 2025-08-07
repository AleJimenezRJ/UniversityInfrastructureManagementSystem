using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;

/// <summary>
/// Implementation of <see cref="IUserRepository"/> that interacts with the SQL database using EF Core.
/// </summary>
internal class SqlUserRepository : IUserRepository
{
    private readonly ThemeParkDataBaseContext _dbContext;
    private readonly ILogger<SqlUserRepository> _logger;

    /// <summary>
    /// Constructor for <see cref="SqlUserRepository"/>.
    /// </summary>
    /// <param name="dbContext">Injected database context for accessing data.</param>
    /// <param name="logger">Logger for recording operational messages and errors.</param>
    public SqlUserRepository(ThemeParkDataBaseContext dbContext, ILogger<SqlUserRepository> logger)
    => (_dbContext, _logger) = (dbContext, logger);

    /// <summary>
    /// Deletes a user from the database by their unique identifier.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>
    /// <c>true</c> if the user was successfully deleted; 
    /// <c>false</c> if the user was not found or an error occurred during deletion.
    /// </returns>
    public async Task<bool> DeleteUserAsync(int id)
    {
        var message = string.Empty;

        var user = await _dbContext.Users.FindAsync(id);
        if (user == null)
        {
            message = $"User with ID {id} was not found.";
            _logger.LogWarning(message);
            throw new NotFoundException(message);
        }
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        _logger.LogInformation("Successfully deleted User with UserName: {UserName}", user.UserName);

        return true;
        
    }

    /// <summary>
    /// Creates a new <see cref="User"/> entry in the database.
    /// </summary>
    /// <param name="User">The user entity to insert.</param>
    /// <returns>True if creation was successful; false otherwise.</returns>
    public async Task<bool> CreateUserAsync(User user)
    {
        var message = string.Empty;

        try
        {
            bool exists = await _dbContext.Users.AnyAsync(p =>
              p.UserName.Value == user.UserName.Value );
            // Check no other user exists with the same username

            if (exists)
            {
                message = $"A user with the username '{user.UserName.Value}' already exists.";
                _logger.LogWarning(message);
                throw new DuplicatedEntityException(message);
            }


            bool verified = await _dbContext.Persons.AnyAsync(p =>
              p.Id == user.PersonId);

            if (!verified)
            {
                message = $"The associated person with ID '{user.PersonId}' was not found.";
                _logger.LogWarning(message);
                throw new NotFoundException(message);
            }

            // If the User does not exist, add it to the database
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully created User with UserName: {UserName}", user.UserName.Value);
            return true;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Database context is null when creating user with UserName {UserName}", user.UserName.Value);
            throw new DomainException("The database context was unexpectedly null while creating the user.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update error while creating user with UserName {UserName}", user.UserName.Value);
            throw new DomainException("A database error occurred while creating the user.");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Operation was canceled while creating user with UserName {UserName}", user.UserName.Value);
            throw new DomainException("The operation was canceled while creating the user.");
        }
        catch (DuplicatedEntityException ex)
        {
            _logger.LogWarning(ex, "Attempt to create User with existing user name: {UserName}", user.UserName.Value);
            throw;
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Attempt to create User with invalid person id: {PersonId}", user.PersonId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while creating user with UserName {UserName}", user.UserName.Value);
            throw new DomainException("An unexpected error occurred while creating the user.");
        }
    }
    /// <summary>
    /// Retrieves all users registered in the database.
    /// </summary>
    /// <returns>A list of <see cref="Users"/> entries; empty list if an error occurs.</returns>
    public async Task<List<User>> GetAllUsersAsync()
    {
        try
        {
            var users = await _dbContext.Users.ToListAsync();
            _logger.LogInformation("Retrieved {Count} users from database", users.Count);
            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users from database");
            throw new DomainException($"An unexpected error occurred while retrieving users: {ex.Message}");
        }
    }

    /// <summary>
    /// Modifies the UserName of an existing <see cref="User"/>.
    /// </summary>
    /// <param name="id">The ID of the user to modify.</param>
    /// <param name="user">The user entity containing the new UserName.</param>
    /// <returns>True if modification was successful; false otherwise.</returns>
    public async Task<bool> ModifyUserAsync(int id, User user)
    {
        try
        {
            var existingUser = await _dbContext.Users.FindAsync(id);

            if (existingUser == null)
            {
                _logger.LogWarning("Attempt to modify non-existent User with ID: {UserId}", id);
                throw new NotFoundException($"User with ID {id} was not found.");
            }

            if (existingUser.UserName.Value != user.UserName.Value)
            {
                bool usernameTaken = await _dbContext.Users
                    .AnyAsync(u => u.UserName.Value == user.UserName.Value && u.Id != id);

                if (usernameTaken)
                {
                    _logger.LogWarning("Attempt to change UserName to one already in use: {UserName}", user.UserName.Value);
                    throw new DuplicatedEntityException($"A user with the username '{user.UserName.Value}' already exists.");
                }

                existingUser.UserName = user.UserName;
            }
           
            existingUser.PersonId = user.PersonId;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully modified User with ID: {UserId}", id);
            return true;
        }
        catch (DbUpdateException ex) when (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE"))
        {
            _logger.LogWarning(ex, "Attempt to change UserName to one already in use: {UserName}", user.UserName.Value);
            throw new DuplicatedEntityException($"A user with the username '{user.UserName.Value}' already exists.");
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Attempt to modify non-existent User with ID: {UserId}", id);
            throw;
        }
        catch (DuplicatedEntityException ex)
        {
            _logger.LogWarning(ex, "Attempt to change UserName to one already in use: {UserName}", user.UserName.Value);
            throw;
        }

    }
}
