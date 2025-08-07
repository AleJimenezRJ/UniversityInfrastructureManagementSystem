using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;

/// <summary>
/// Repository implementation for managing <see cref="Permission"/> entities using SQL database via Entity Framework Core.
/// Provides methods for querying and manipulating permissions in the context of account management.
/// </summary>
internal class SqlPermissionRepository : IPermissionRepository
{
    private readonly ThemeParkDataBaseContext _dbContext;
    private readonly ILogger<SqlPermissionRepository> _logger;

    public SqlPermissionRepository(ThemeParkDataBaseContext dbContext, ILogger<SqlPermissionRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all permissions registered in the database.
    /// </summary>
    /// <returns>A list of <see cref="Permission"/> entries.</returns>
    /// <exception cref="DomainException">Thrown when permission retrieval fails.</exception>
    public async Task<List<Permission>> GetAllPermissionsAsync()
    {
        try
        {
            var permissions = await _dbContext.Permissions.ToListAsync();
            _logger.LogInformation("Retrieved {Count} permissions from database", permissions.Count);
            return permissions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve permissions from database");
            throw new DomainException("An error occurred while retrieving permissions.");
        }
    }
}
