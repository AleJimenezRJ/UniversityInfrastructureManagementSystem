using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;

/// <summary>
/// Provides centralized, dynamic mapping between <see cref="LearningComponent"/> entities and their corresponding DTOs.
/// Supports both ID-based and No-ID-based DTO transformations using runtime type resolution.
/// </summary>
/// <remarks>
/// This class is part of the learning component mapping refactor (see EPIC ID: CPD-LC-002).
/// It registers and resolves mappers for different component types, enabling polymorphic mapping logic.
/// </remarks>
public class GlobalMapper
{
    // Mappers for DTOs with ID
    private readonly Dictionary<Type, object> _entityMappersWithId = new();
    private readonly Dictionary<Type, object> _dtoMappersWithId = new();

    // Mappers for DTOs without ID
    private readonly Dictionary<Type, object> _entityMappersNoId = new();
    private readonly Dictionary<Type, object> _dtoMappersNoId = new();

    /// <summary>
    /// Initializes the global mapper and registers all supported mappers.
    /// </summary>
    public GlobalMapper()
    {
        RegisterMapperWithId(new ProjectorDtoMapper());
        RegisterMapperWithId(new WhiteboardDtoMapper());
        RegisterMapperNoId(new ProjectorNoIdDtoMapper());
        RegisterMapperNoId(new WhiteboardNoIdDtoMapper());
    }

    /// <summary>
    /// Registers a mapper for a component type and its DTO (with ID).
    /// </summary>
    /// <typeparam name="TComponent">The component type.</typeparam>
    /// <typeparam name="TDto">The DTO type with ID.</typeparam>
    /// <param name="mapper">The mapper instance.</param>
    private void RegisterMapperWithId<TComponent, TDto>(LearningComponentDtoMapper<TComponent, TDto> mapper)
        where TComponent : LearningComponent
        where TDto : LearningComponentDto
    {
        _entityMappersWithId[typeof(TComponent)] = mapper;
        _dtoMappersWithId[typeof(TDto)] = mapper;
    }

    /// <summary>
    /// Registers a mapper for a component type and its DTO (without ID).
    /// </summary>
    /// <typeparam name="TComponent">The component type.</typeparam>
    /// <typeparam name="TDto">The DTO type without ID.</typeparam>
    /// <param name="mapper">The mapper instance.</param>
    private void RegisterMapperNoId<TComponent, TDto>(LearningComponentNoIdDtoMapper<TComponent, TDto> mapper)
        where TComponent : LearningComponent
        where TDto : LearningComponentNoIdDto
    {
        _entityMappersNoId[typeof(TComponent)] = mapper;
        _dtoMappersNoId[typeof(TDto)] = mapper;
    }

    /// <summary>
    /// Converts a <see cref="LearningComponent"/> instance to its corresponding DTO with ID.
    /// </summary>
    /// <param name="component">The component instance to convert.</param>
    /// <returns>The mapped <see cref="LearningComponentDto"/>.</returns>
    /// <exception cref="NotSupportedException">Thrown if no mapper is registered for the component type.</exception>
    /// <exception cref="InvalidCastException">Thrown if the returned DTO is not assignable to <see cref="LearningComponentDto"/>.</exception>
    public LearningComponentDto ToDto(LearningComponent component)
    {
        var type = component.GetType();
        if (!_entityMappersWithId.TryGetValue(type, out var mapperObj))
        {
            throw new NotSupportedException($"No ID-based mapper registered for type {type.Name}.");
        }

        dynamic mapper = mapperObj;
        dynamic specificComponent = component;

        var result = mapper.ToDto(specificComponent);

        if (result is not LearningComponentDto dto)
            throw new InvalidCastException($"Returned DTO type {result.GetType().Name} is not assignable to LearningComponentDto.");

        return dto;
    }

    /// <summary>
    /// Converts a DTO without ID to its corresponding <see cref="LearningComponent"/> entity.
    /// </summary>
    /// <param name="dto">The DTO instance without ID.</param>
    /// <returns>The mapped <see cref="LearningComponent"/> entity.</returns>
    /// <exception cref="NotSupportedException">Thrown if no mapper is registered for the DTO type.</exception>
    public LearningComponent ToEntity(LearningComponentNoIdDto dto)
    {
        var type = dto.GetType();
        if (!_dtoMappersNoId.TryGetValue(type, out var mapperObj))
        {
            throw new NotSupportedException($"No No-ID mapper registered for DTO type {type.Name}.");
        }

        dynamic mapper = mapperObj;
        dynamic specificDto = dto;

        return mapper.ToEntity(specificDto);
    }
}
