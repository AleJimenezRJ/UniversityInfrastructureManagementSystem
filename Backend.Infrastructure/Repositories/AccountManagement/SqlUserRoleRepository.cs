using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;

/// <summary>
/// Provides data access operations for user-role associations in the database.
/// Implements <see cref="IUserRoleRepository"/> for managing role assignments to users.
/// </summary>
internal class SqlUserRoleRepository : IUserRoleRepository
{
    private readonly ThemeParkDataBaseContext _dbContext;
    private readonly ILogger<SqlUserRoleRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlUserRoleRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context for data operations.</param>
    /// <param name="logger">The logger for tracking repository events.</param>
    public SqlUserRoleRepository(ThemeParkDataBaseContext dbContext, ILogger<SqlUserRoleRepository> logger) =>
        (_dbContext, _logger) = (dbContext, logger);

    /// <summary>
    /// Assigns a role to a user in the database.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<bool> AssignRoleAsync(int userId, int roleId)
    {
        // Check if the user-role association already exists
        var exists = await _dbContext.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        if (exists)
        {
            _logger.LogWarning("UserRole already exists for UserId {UserId} and RoleId {RoleId}", userId, roleId);
            throw new DuplicatedEntityException($"Attempt to create user role association that already exists");
        }

        var userRole = new UserRole(userId, roleId);
        _dbContext.UserRoles.Add(userRole);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    /// <summary>
    /// Retrieves a user-role association by user and role identifiers.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<UserRole?> GetByUserAndRoleAsync(int userId, int roleId)
    {
        return await _dbContext.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
    }

    /// <summary>
    /// Retrieves a user-role association by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<UserRole?> GetUserRoleAsync(int id)
    {
        return await _dbContext.UserRoles
            .FirstOrDefaultAsync(ur => ur.RoleId == id);
    }

    /// <summary>
    /// Adds a new user-role association to the database.
    /// </summary>
    /// <param name="userRole"></param>
    /// <returns></returns>
    public async Task<bool> AddAsync(UserRole userRole)
    {
        // Check if the user-role association already exists
        var exists = await _dbContext.UserRoles.AnyAsync(ur => ur.UserId == userRole.UserId && ur.RoleId == userRole.RoleId);
        if (exists)
        {
            _logger.LogWarning("UserRole already exists for UserId {UserId} and RoleId {RoleId}", userRole.UserId, userRole.RoleId);
            throw new DuplicatedEntityException($"Attempt to create user role association that already exists");
        }

        _dbContext.UserRoles.Add(userRole);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    /// <summary>
    /// Removes a user-role association from the database.
    /// </summary>
    /// <param name="userRole"></param>
    /// <returns></returns>
    public async Task<bool> RemoveAsync(UserRole userRole)
    {
        var existing = await _dbContext.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userRole.UserId && ur.RoleId == userRole.RoleId);
        if (existing == null)
        {
            _logger.LogWarning("UserRole not found for UserId {UserId} and RoleId {RoleId}", userRole.UserId, userRole.RoleId);
            throw new NotFoundException($"User Role with roleId {userRole.RoleId} and userId {userRole.UserId} and was not found.");
        }

        _dbContext.UserRoles.Remove(existing);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    /// <summary>
    /// Retrieves a list of roles associated with a specific user.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<List<Role>?> GetRolesByUserIdAsync(int userId)
    {
        var userRoles = await _dbContext.UserRoles
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.Role)
            .ToListAsync();
        return userRoles?.Select(ur => ur.Role).ToList();
    }
}
