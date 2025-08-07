using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;

/// <summary>
/// Provides data access operations for users with their associated person information.
/// Implements <see cref="IUserWithPersonRepository"/> for combined user-person management.
/// </summary>
internal class SqlUserWithPersonRepository : IUserWithPersonRepository
{
    private readonly ThemeParkDataBaseContext _dbContext;
    private readonly ILogger<SqlUserWithPersonRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlUserWithPersonRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context for data operations.</param>
    /// <param name="logger">The logger for tracking repository events.</param>
    public SqlUserWithPersonRepository(ThemeParkDataBaseContext dbContext, ILogger<SqlUserWithPersonRepository> logger)
        => (_dbContext, _logger) = (dbContext, logger);

    /// <summary>
    /// Retrieves all users and person registered in the database.
    /// </summary>
    /// <returns>A list of <see cref="Users"/> entries; empty list if an error occurs.</returns>
    public async Task<List<UserWithPerson>> GetAllUserWithPersonAsync()
    {
        return await _dbContext.Users
            .Include(u => u.Person)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Where(u => u.Person != null)
            .Select(u => new UserWithPerson(
                u.UserName,
                u.Person.FirstName,
                u.Person.LastName,
                u.Person.Email,
                u.Person.Phone,
                u.Person.IdentityNumber,
                u.Person.BirthDate,
                u.Id,
                u.Person.Id
            )
            {
                Roles = u.UserRoles
                    .Where(ur => ur.Role != null)
                    .Select(ur => ur.Role.Name)
                    .ToList()
            })
            .ToListAsync();
    }

    /// <summary>
    /// Creates a new user with associated person information and roles in a single atomic transaction.
    /// And update the user audit log with the changes made.
    /// </summary>
    /// <param name="userWithPerson">The user and person data to create.</param>
    /// <returns>
    /// <c>true</c> if the user and person were successfully created with all specified roles;
    /// <c>false</c> if any validation fails, the user/person already exists, or an error occurs.
    /// </returns>
    public async Task<bool> CreateUserWithPersonAsync(UserWithPerson userWithPerson)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // Create value objects with validation
            var email = Email.Create(userWithPerson.Email.Value);
            var phone = Phone.Create(userWithPerson.Phone.Value);
            var identityNumber = IdentityNumber.Create(userWithPerson.IdentityNumber.Value);
            var birthDate = BirthDate.Create(userWithPerson.BirthDate.Value);
            var userName = UserName.Create(userWithPerson.UserName.Value);

            // Check for existing person (optimized query)
            var existingPersons = await _dbContext.Persons
              .Select(p => new { Identity = p.IdentityNumber.Value, Email = p.Email.Value })
              .ToListAsync();

            bool personExists = existingPersons.Any(p =>
              p.Identity == identityNumber.Value || p.Email == email.Value);

            if (personExists)
            {
                _logger.LogWarning("Attempt to create person with existing identity number or email: {IdentityNumber}, {Email}", identityNumber, email);
                throw new DuplicatedEntityException($"A person with identity number {identityNumber} or email {email} already exists.");
            }

            // Create and save person
            var person = new Person(email, userWithPerson.FirstName, userWithPerson.LastName, phone, birthDate, identityNumber);
            _dbContext.Persons.Add(person);
            await _dbContext.SaveChangesAsync();

            if (person.Id == 0)
            {
                _logger.LogWarning("Person creation failed - no ID generated.");
                await transaction.RollbackAsync();
                throw new DomainException($"An unexpected error occurred while creating UserWithPerson.");
            }

            // Verifies the existence of a user (Avoiding ValueObjects in query)
            var existingUserNames = await _dbContext.Users
              .Select(u => u.UserName.Value)
              .ToListAsync();

            bool userExists = existingUserNames.Contains(userName.Value);

            if (userExists)
            {
                _logger.LogWarning("Attempt to create user with existing username: {UserName}", userName);
                await transaction.RollbackAsync();
                throw new DuplicatedEntityException($"A user with username {userName} already exists.");
            }

            // Verify that a person has been persisted correctly
            bool personVerified = await _dbContext.Persons.AnyAsync(p => p.Id == person.Id);

            if (!personVerified)
            {
                _logger.LogWarning("Attempt to create user with invalid person ID: {PersonId}", person.Id);
                await transaction.RollbackAsync();
                throw new ValidationException("An unexpected error occurred while validating person ID.");
            }

            // Create a user associated to a person
            var user = new User(userName, person.Id);
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            foreach (var roleName in userWithPerson.Roles.Distinct())
            {
                var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
                if (role != null)
                {
                    var userRole = new UserRole(user.Id, role.Id);
                    _dbContext.UserRoles.Add(userRole);
                }
                else
                {
                    _logger.LogWarning("Role '{RoleName}' not found in database. Skipping.", roleName);
                }
            }

            // Create log entry for user creation
            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC spInsertUserAudit @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7",
                user.UserName.Value,
                person.FirstName,
                person.LastName,
                person.Email.Value,
                person.Phone.Value,
                person.IdentityNumber.Value,
                person.BirthDate.Value.ToDateTime(TimeOnly.MinValue),
                "CREADO"
            );

            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
            _logger.LogInformation("Successfully created User and Person with IDs: User={UserId}, Person={PersonId}", user.Id, person.Id);
            return true;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Database context is null when creating UserWithPerson with UserName {UserName}", userWithPerson.UserName.Value);
            throw new DomainException($"An unexpected error occurred while creating UserWithPerson: {ex.Message}");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update error while creating UserWithPerson with UserName {UserName}", userWithPerson.UserName.Value);
            throw new DomainException($"An unexpected error occurred while creating UserWithPerson: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Operation was canceled while creating UserWithPerson with UserName {UserName}", userWithPerson.UserName.Value);
            throw new DomainException($"An unexpected error occurred while creating UserWithPerson: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a user and their associated person from the database.
    /// And update the user audit log with the changes made.
    /// </summary>
    /// <param name="userId"> The unique identifier for the user to be deleted.</param>
    /// <param name="personId"> The unique identifier for the person associated with the user to be deleted.</param>
    /// <returns> A task that represents the asynchronous operation. </returns>
    public async Task<bool> DeleteUserWithPersonAsync(int userId, int personId)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var user = await _dbContext.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == userId && u.PersonId == personId);

            if (user == null)
            {
                _logger.LogWarning("Delete failed: No user found with UserId={UserId} and PersonId={PersonId}", userId, personId);
                throw new NotFoundException($"UserWithPerson with person Id {personId} and user Id {userId} and was not found.");
            }

            var person = await _dbContext.Persons.FindAsync(personId);
            if (person == null)
            {
                _logger.LogWarning("Delete failed: Person with ID {PersonId} not found", personId);
                throw new NotFoundException($"Person with ID {personId} was not found.");
            }

            // Remove associated roles
            _dbContext.UserRoles.RemoveRange(user.UserRoles);

            // Remove user
            _dbContext.Users.Remove(user);

            // Remove person
            _dbContext.Persons.Remove(person);

            // Create log entry for user deletion
            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC spInsertUserAudit @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7",
                user.UserName.Value,
                person.FirstName,
                person.LastName,
                person.Email.Value,
                person.Phone.Value,
                person.IdentityNumber.Value,
                person.BirthDate.Value.ToDateTime(TimeOnly.MinValue),
                "ELIMINADO"
            );

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Successfully deleted User ID {UserId} and Person ID {PersonId}", userId, personId);
            return true;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Database context is null when deleting UserWithPerson with UserId={UserId} and PersonId={PersonId}", userId, personId);
            throw new DomainException($"An unexpected error occurred while deleting UserWithPerson: {ex.Message}");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update error while deleting UserWithPerson with UserId={UserId} and PersonId={PersonId}", userId, personId);
            throw new DomainException($"An unexpected error occurred while deleting UserWithPerson: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Operation was canceled while deleting UserWithPerson with UserId={UserId} and PersonId={PersonId}", userId, personId);
            throw new DomainException($"An unexpected error occurred while deleting UserWithPerson: {ex.Message}");
        }
    }

    /// <summary>
    ///  Modify a user and their associated person from the database.
    ///  And update the user audit log with the changes made.
    /// </summary>
    /// <param name="user"> The user object containing the details of the user to be updated.</param>
    /// <returns> A task that represents the asynchronous operation. </returns>
    public async Task<bool> UpdateUserWithPersonAsync(UserWithPerson user)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var existingUser = await _dbContext.Users.FindAsync(user.UserId);
            var existingPerson = await _dbContext.Persons.FindAsync(user.PersonId);

            if (existingUser == null || existingPerson == null)
            {
                _logger.LogWarning("Cannot update: User or Person not found. UserId={UserId}, PersonId={PersonId}", user.UserId, user.PersonId);
                throw new NotFoundException($"User with ID {user.UserId} or person with ID {user.PersonId} was not found.");
            }

            // verified if the username exists
            if (existingUser.UserName.Value != user.UserName.Value)
            {
                bool usernameExists = await _dbContext.Users
                    .AnyAsync(u => u.UserName == user.UserName && u.Id != user.UserId);

                if (usernameExists)
                {
                    _logger.LogWarning("Username already in use: {UserName}", user.UserName);
                    throw new DuplicatedEntityException($"A user with username {user.UserName} already exists.");
                }

                existingUser.UserName = user.UserName;
            }


            // update person
            existingPerson.FirstName = user.FirstName;
            existingPerson.LastName = user.LastName;
            existingPerson.Email = user.Email;
            existingPerson.Phone = user.Phone;
            existingPerson.IdentityNumber = user.IdentityNumber;
            existingPerson.BirthDate = user.BirthDate;

            // update user roles
            var existingUserRoles = _dbContext.UserRoles.Where(ur => ur.UserId == user.UserId);
            _dbContext.UserRoles.RemoveRange(existingUserRoles);

            foreach (var roleName in user.Roles.Distinct())
            {
                var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
                if (role != null)
                {
                    _dbContext.UserRoles.Add(new UserRole(user.UserId, role.Id));
                }
                else
                {
                    _logger.LogWarning("Role '{RoleName}' not found. Skipping assignment.", roleName);
                }
            }

            // Create log entry for user update
            await _dbContext.Database.ExecuteSqlRawAsync(
                "EXEC spInsertUserAudit @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7",
                user.UserName.Value,
                user.FirstName,
                user.LastName,
                user.Email.Value,
                user.Phone.Value,
                user.IdentityNumber.Value,
                user.BirthDate.Value.ToDateTime(TimeOnly.MinValue),
                "ACTUALIZADO"
            );

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            _logger.LogInformation("UserWithPerson updated successfully. UserId={UserId}, PersonId={PersonId}", user.UserId, user.PersonId);
            return true;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Database context is null when creating user with UserName {UserName}", user.UserName.Value);
            throw new DomainException($"An unexpected error occurred while updating UserWithPerson: {ex.Message}");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update error while creating user with UserName {UserName}", user.UserName.Value);
            throw new DomainException($"An unexpected error occurred while updating UserWithPerson: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Operation was canceled while creating user with UserName {UserName}", user.UserName.Value);
            throw new DomainException($"An unexpected error occurred while updating UserWithPerson: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of users with their associated person information and roles.
    /// </summary>
    /// <param name="pageSize">The number of users per page.</param>
    /// <param name="lastUserId">The ID of the last user from the previous page, used to retrieve the next set of results.</param>
    /// <returns>A paginated list of <see cref="UserWithPerson"/>.</returns>
    public async Task<PaginatedList<UserWithPerson>> GetPaginatedUsersAsync(int pageSize, int pageNumber, string searchText)
    {
        try
        {

            var query = _dbContext.Users
                .AsNoTracking()
                .Include(u => u.Person)
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .AsQueryable();

            var allUsers = await query.ToListAsync();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var lowerSearch = searchText.ToLower();

                allUsers = allUsers.Where(u =>
                    u.UserName.Value.ToLower().Contains(lowerSearch) ||
                    u.Person.FirstName.ToLower().Contains(lowerSearch) ||
                    u.Person.LastName.ToLower().Contains(lowerSearch) ||
                    u.Person.Email.Value.ToLower().Contains(lowerSearch) ||
                    u.Person.IdentityNumber.Value.ToLower().Contains(lowerSearch) ||
                    u.UserRoles.Any(ur => ur.Role != null && ur.Role.Name.ToLower().Contains(lowerSearch))
                ).ToList();
            }

            var totalCount = allUsers.Count;

            var paginatedUsers = allUsers
                .OrderBy(u => u.Id)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .Select(u => new UserWithPerson(
                    u.UserName,
                    u.Person.FirstName,
                    u.Person.LastName,
                    u.Person.Email,
                    u.Person.Phone,
                    u.Person.IdentityNumber,
                    u.Person.BirthDate,
                    u.Id,
                    u.Person.Id
                )
                {
                    Roles = u.UserRoles
                        .Where(ur => ur.Role != null)
                        .Select(ur => ur.Role.Name)
                        .ToList()
                })
                .ToList();

            return new PaginatedList<UserWithPerson>(paginatedUsers, totalCount, pageSize, pageNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving paginated users for page {PageNumber} with search '{SearchText}'", pageNumber, searchText);
            throw;
        }
    }



    /// <summary>
    /// Validates the uniqueness of a user's username, email, and identity number.
    /// Throws a <see cref="DuplicatedEntityException"/> if any of the values are already registered
    /// for a different user or person in the database.
    /// </summary>
    /// <param name="user">
    /// The <see cref="UserWithPerson"/> object containing the username, email, and identity number to validate.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result is <c>true</c> if all values are unique;
    /// otherwise, an exception is thrown.
    /// </returns>
    /// <exception cref="DuplicatedEntityException">
    /// Thrown if the username, email, or identity number is already registered for another user or person.
    /// </exception>
    public async Task<bool> ValidateUserUniquenessAsync(UserWithPerson user)
    {
        bool userNameExists = await _dbContext.Users
            .AnyAsync(u => u.UserName.Value == user.UserName.Value && u.Id != user.UserId);

        bool emailExists = await _dbContext.Persons
            .AnyAsync(p => p.Email.Value == user.Email.Value && p.Id != user.PersonId);

        bool identityExists = await _dbContext.Persons
            .AnyAsync(p => p.IdentityNumber.Value == user.IdentityNumber.Value && p.Id != user.PersonId);

        if (userNameExists)
            throw new DuplicatedEntityException("The username is already registered.");
        if (emailExists)
            throw new DuplicatedEntityException("The email address is already registered.");
        if (identityExists)
            throw new DuplicatedEntityException("The identity number is already registered.");

        return true;
    }

    /// <summary>
    ///  Retrieves the user ID associated with a given email address.
    /// </summary>
    /// <param name="email"> The email address to search for.</param>
    /// <returns> A task that represents the asynchronous operation. </returns>
    public async Task<int?> GetUserIdByEmailAsync(string email)
    {
        return await _dbContext.Users
            .Include(u => u.Person)
            .Where(u => u.Person.Email == Email.Create(email))
            .Select(u => (int?)u.Id)
            .FirstOrDefaultAsync();
    }
}