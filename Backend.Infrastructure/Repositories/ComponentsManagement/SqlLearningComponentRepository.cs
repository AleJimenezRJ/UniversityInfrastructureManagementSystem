using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.ComponentsManagement;

/// <summary>
/// SQL implementation of the <see cref="ILearningComponentRepository"/> interface.
/// </summary>
internal class SqlLearningComponentRepository : ILearningComponentRepository
{
    /// <summary>
    /// The database context used to interact with the database.
    /// </summary>
    private readonly ThemeParkDataBaseContext _dbContext;

    /// <summary>
    /// Object for display logging information and errors.
    /// </summary>
    private readonly ILogger<SqlLearningComponentRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlLearningComponentRepository"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    public SqlLearningComponentRepository(ThemeParkDataBaseContext dbContext, ILogger<SqlLearningComponentRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all learning components in the system that have not been logically deleted.
    /// </summary>
    /// <param name="pageSize">The maximum number of learning components to retrieve per page.</param>
    /// <param name="pageIndex">The page index for pagination.</param>
    /// <returns>A collection of all active learning components.</returns>
    public async Task<IEnumerable<LearningComponent>> GetAllAsync(int pageSize, int pageIndex)
    {
        var offset = (pageIndex) * pageSize;
        return await _dbContext.LearningComponents
            .OrderBy(c => c.ComponentId)
            .Where(c => !c.IsDeleted)
            .Skip(offset)
            .Take(pageSize)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves a single learning component by its unique identifier, only if it is not deleted.
    /// </summary>
    /// <param name="id">The unique identifier of the learning component.</param>
    /// <returns>The requested learning component if found and not deleted.</returns>
    /// <exception cref="NotFoundException">Thrown if the component does not exist or is deleted.</exception>
    public async Task<LearningComponent?> GetSingleLearningComponentAsync(int id)
    {
        Id componentId = Id.Create(id);
        LearningComponent? component = null;

        try
        {
            component = await _dbContext
                .LearningComponents
                .FirstOrDefaultAsync(c => c.ComponentId == componentId.ValueInt && !c.IsDeleted);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error retrieving component with '{Id}': {Message}",
                id, 
                ex.Message);
        }

        if (component is null)
        {
            string message = $"Component with ID '{id}' not found.";
            throw new NotFoundException(message);
        }
        
        return component;
    }

    /// <summary>
    /// Retrieves all learning components associated with a specific learning space, excluding deleted ones.
    /// </summary>
    /// <param name="learningSpaceId">The unique identifier of the learning space.</param>
    /// <param name="pageSize">The maximum number of learning components to retrieve per page.</param>
    /// <param name="pageIndex">The page index for pagination.</param>
    /// <param name="stringSearch">The string to filter the results on.</param>
    /// <returns>A collection of learning components for the specified space.</returns>
    public async Task<PaginatedList<LearningComponent>> GetLearningComponentsByIdAsync(int learningSpaceId, int pageSize, int pageIndex, string stringSearch)
    {
        try
        {
            // Validate that the learning space exists and belongs to the floor
            bool learningSpaceExists = await _dbContext.LearningSpaces
                .AnyAsync(ls =>
                    EF.Property<int>(ls, "LearningSpaceId") == learningSpaceId);

            if (!learningSpaceExists)
            {
                _logger.LogWarning("Cannot add component: Learning space with ID {LearningSpaceId}", learningSpaceId);
                return PaginatedList<LearningComponent>.Empty(pageSize, pageIndex);
            }
            // Retrieve learning components that belong to the specified learning space
            var query = _dbContext.LearningComponents
                .AsNoTracking()
                .OrderBy(c => c.ComponentId)
                .Where(c => EF.Property<int>(c, "LearningSpaceId") == learningSpaceId && !c.IsDeleted);
                
            var offset = (pageIndex) * pageSize;
                
            var components = await query.ToListAsync();
            var totalCount = components.Count;

            if (!string.IsNullOrEmpty(stringSearch))
            {
                components = components
                    .Where(x =>
                        x.DisplayId.Contains(stringSearch, StringComparison.InvariantCultureIgnoreCase) ||
                        x.Orientation.Value.Contains(stringSearch, StringComparison.InvariantCultureIgnoreCase))
                    .ToList();
                totalCount = components.Count;
            }
            components = components
                .Skip(offset)
                .Take(pageSize)
                .ToList();
            
            return new PaginatedList<LearningComponent>(components, totalCount, pageSize, pageIndex);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error retrieving components for learning space '{LearningSpaceId}'': {Message}",
                learningSpaceId, 
                ex.Message);

            return PaginatedList<LearningComponent>.Empty(pageSize, pageIndex);
        }
    }
    
    /// <summary>
    /// Adds a new learning component to a specified learning space.
    /// </summary>
    /// <param name="learningSpaceId">The unique identifier of the learning space.</param>
    /// <param name="learningComponent">The learning component to add.</param>
    /// <returns>True if the component was added successfully; otherwise, false.</returns>
    public async Task<bool> AddComponentAsync(int learningSpaceId, LearningComponent learningComponent)
    {

        // Validate that the learning space exists and belongs to the floor
        bool learningSpaceExists = await _dbContext.LearningSpaces
            .AnyAsync(ls =>
                EF.Property<int>(ls, "LearningSpaceId") == learningSpaceId);

        if (!learningSpaceExists)
        {
            _logger.LogWarning("Cannot add component: Learning space with ID {LearningSpaceId} not found on Floor", learningSpaceId);
            throw new NotFoundException($"Learning space with ID {learningSpaceId} not found.");
        }
        try
        {
            // Add the LearningComponent to the context
            await _dbContext.LearningComponents.AddAsync(learningComponent);

            // Set shadow properties
            _dbContext.Entry(learningComponent).Property("LearningSpaceId").CurrentValue = learningSpaceId;

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Component added successfully to Learning Space ID {LearningSpaceId}", learningSpaceId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, 
                "Error adding component to Learning Space ID {LearningSpaceId}: {Message}",
                learningSpaceId,
                ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Updates an existing learning component in a specific learning space.
    /// </summary>
    /// <param name="learningSpaceId">The unique identifier of the learning space.</param>
    /// <param name="learningComponentId">The unique identifier of the learning component.</param>
    /// <param name="learningComponent">The updated learning component data.</param>
    /// <returns>True if the update was successful; otherwise, false.</returns>
    public async Task<bool> UpdateAsync(int learningSpaceId, int learningComponentId, LearningComponent learningComponent)
    {
        try
        {
            // Validate that the learning space exists and belongs to the floor
            bool learningSpaceExists = await _dbContext.LearningSpaces
                .AnyAsync(ls =>
                    EF.Property<int>(ls, "LearningSpaceId") == learningSpaceId);

            if (!learningSpaceExists)
            {
                _logger.LogWarning(
                    "Cannot add component: Learning space with ID {LearningSpaceId}",
                    learningSpaceId);
                return false;
            }

            //Find the existing learning component using only the component ID and learning space ID
            var existing = await _dbContext.LearningComponents.FirstOrDefaultAsync(lc =>
                lc.ComponentId == learningComponentId &&
                EF.Property<int>(lc, "LearningSpaceId") == learningSpaceId &&
                !lc.IsDeleted);

            // Detach existing entity and set the shadow property
            learningComponent.ComponentId = existing!.ComponentId;
            learningComponent.IsDeleted = existing.IsDeleted; // Preserve deletion state
            _dbContext.Entry(existing).State = EntityState.Detached;
            _dbContext.Entry(learningComponent).Property("LearningSpaceId").CurrentValue = learningSpaceId;

            _dbContext.Update(learningComponent);
            int result = await _dbContext.SaveChangesAsync();


            _logger.LogInformation(
                "Component ID {ComponentId} successfully updated in learning space '{LearningSpaceId}'.",
                existing.ComponentId, 
                learningSpaceId);

            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error updating component ID {ComponentId} in learning space '{LearningSpaceId}': {Message}", 
                learningComponent.ComponentId,
                learningSpaceId, 
                ex.Message);
            return false;
        }
    }



    /// <summary>
    /// Performs a soft delete of a learning component by its unique identifier.
    /// The component is not physically removed from the database, but marked as deleted.
    /// </summary>
    /// <param name="id">The unique identifier of the learning component to delete.</param>
    /// <returns>True if the component was found and marked as deleted; otherwise, false.</returns>
    public async Task<bool> DeleteComponentAsync(int id)
    {
        var component = await _dbContext.LearningComponents
            .FirstOrDefaultAsync(c => c.ComponentId == id && !c.IsDeleted);

        if (component is null)
            return false;

        component.IsDeleted = true;
        _dbContext.LearningComponents.Update(component);

        // Removed unused variable assignment for learningSpaceId.

        int r = await _dbContext.SaveChangesAsync();

        return r > 0;
    }
}
