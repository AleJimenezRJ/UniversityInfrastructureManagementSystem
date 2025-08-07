using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;

/// <summary>
/// Implementation of <see cref="IStaffRepository"/> that interacts with the SQL database using EF Core.
/// </summary>
internal class SqlStaffRepository : IStaffRepository
{
    private readonly ThemeParkDataBaseContext _dbContext;
    private readonly ILogger<SqlStaffRepository> _logger;

    /// <summary>
    /// Constructor for <see cref="SqlStaffRepository"/>.
    /// </summary>
    /// <param name="dbContext">Injected database context for accessing data.</param>
    /// <param name="logger">Logger for recording operational messages and errors.</param>
    public SqlStaffRepository(ThemeParkDataBaseContext dbContext, ILogger<SqlStaffRepository> logger)
    => (_dbContext, _logger) = (dbContext, logger);

    /// <summary>
    /// Creates a new <see cref="Staff"/> entry in the database.
    /// </summary>
    /// <param name="Staff">The Staff entity to insert.</param>
    /// <returns>True if creation was successful; false otherwise.</returns>
    public async Task<bool> CreateStaffAsync(Staff staff)
    {
        try
        {
            bool personExists = await _dbContext.Persons
                .AnyAsync(p => p.IdentityNumber == staff.Person.IdentityNumber ||
                               p.Email == staff.Person.Email);

            bool staffExists = await _dbContext.Staff
                .AnyAsync(s => s.InstitutionalEmail == staff.InstitutionalEmail);

            if (personExists || staffExists)
            {
                _logger.LogWarning("Duplicate Staff or Person detected. Email: {Email}, Identity: {IdentityNumber}",
                    staff.Person.Email, staff.Person.IdentityNumber);
                throw new DuplicatedEntityException("A staff or person with the same identity or email already exists.");
            }

            _dbContext.Persons.Add(staff.Person);
            await _dbContext.SaveChangesAsync();

            staff.PersonId = staff.Person.Id;

            _dbContext.Staff.Add(staff);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Staff created successfully with PersonId: {PersonId}", staff.PersonId);
            return true;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update error while creating staff with PersonId {PersonId}", staff.PersonId);
            throw new DomainException("A database error occurred while creating the staff member.");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Operation was canceled while creating staff with PersonId {PersonId}", staff.PersonId);
            throw new DomainException("An unexpected error occurred while creating the staff member.");
        }
    }

    /// <summary>
    /// Retrieves a list of all staff members from the database, including their associated person information.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a list of <see cref="Staff"/> objects, or null if an error occurs.
    /// </returns>
    public async Task<List<Staff>?> ListStaffAsync()
    {
        try
        {
            var staff = await _dbContext.Staff
                .Include(s => s.Person)
                .ToListAsync();

            return staff;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Database context is null when retrieving staff");
            throw new DomainException("Database context is null when retrieving staff members.");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Operation was canceled while retrieving staff from database");
            throw new DomainException("Operation was canceled while retrieving staff members.");
        }
    }
}
