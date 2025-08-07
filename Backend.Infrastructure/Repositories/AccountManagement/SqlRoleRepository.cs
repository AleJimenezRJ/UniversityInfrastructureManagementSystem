using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;

/// <summary>
/// Implementation of <see cref="IRoleRepository"/> that interacts with the SQL database using EF Core.
/// </summary>
internal class SqlRoleRepository : IRoleRepository
{
    private readonly ThemeParkDataBaseContext _dbContext;
    private readonly ILogger<SqlRoleRepository> _logger;

    /// <summary>
    /// Constructor for <see cref="SqlRoleRepository"/>.
    /// </summary>
    /// <param name="dbContext">Injected database context for accessing data.</param>
    /// <param name="logger">Logger for recording operational messages and errors.</param>
    public SqlRoleRepository(ThemeParkDataBaseContext dbContext, ILogger<SqlRoleRepository> logger)
        => (_dbContext, _logger) = (dbContext, logger);

    /// <summary>
    /// Removes a role from the database.
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<bool> DeleteRoleAsync(int roleId)
    {
        try
        {
            var existing = await _dbContext.Roles
            .FirstOrDefaultAsync(r => r.Id == roleId);
            if (existing == null)
            {
                _logger.LogWarning("Role not found for RoleId {RoleId}", roleId);
                throw new NotFoundException($"Role with ID {roleId} not found.");
            }

            // Remove the person entity
            _dbContext.Roles.Remove(existing);

            // Save changes to the database
            var result = await _dbContext.SaveChangesAsync();

            if (result <= 0)
            {
                _logger.LogError("Failed to delete role with RoleId {RoleId}.", roleId);
                throw new DomainException("Failed to delete the role.");
            }

            _logger.LogInformation("Role with Role Id {RoleId} deleted.", roleId);
            return true;
        }
        catch (DbUpdateConcurrencyException concurrencyEx) // Handle concurrency issues
        {
            _logger.LogError(concurrencyEx, "Concurrency error while deleting role with Role Id {roleId}", roleId);
            throw new ConcurrencyConflictException("Concurrency error while deleting the role.");
        }
        catch (DbUpdateException dbEx) // Handle database update issues
        {
            _logger.LogError(dbEx, "Database update error while deleting role with Role Id {roleId}", roleId);
            throw new DomainException("Database update error while deleting the role.");
        }
        catch (OperationCanceledException ex) // Handle operation cancellation
        {
            _logger.LogError(ex, "Operation was canceled while deleting role with Role Id {roleId}", roleId);
            throw new DomainException("Operation was canceled while deleting the role.");
        }
        catch (ArgumentNullException ex) // Handle null argument issues
        {
            _logger.LogError(ex, "Argument null error while deleting role with Role Id {roleId}", roleId);
            throw new DomainException("Argument null error while deleting the role.");
        }

    }

    /// <summary>
    /// Retrieves all roles registered in the database.
    /// </summary>
    /// <returns>A list of <see cref="Role"/> entries; empty list if an error occurs.</returns>
    public async Task<List<Role>> GetAllRolesAsync()
    {
        try
        {
            var roles = await _dbContext.Roles.ToListAsync();
            _logger.LogInformation("Retrieved {Count} roles from database", roles.Count);
            return roles;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Database context is null when retrieving roles");
            throw new DomainException("An error occurred while retrieving roles.");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Operation was canceled while retrieving roles from database");
            throw new DomainException("An error occurred while retrieving roles.");
        }
    }

}
