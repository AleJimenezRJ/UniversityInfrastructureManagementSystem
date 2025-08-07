
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.ComponentsManagement;

/// <summary>
/// Provides centralized mapping between <see cref="LearningComponent"/> entities and their corresponding DTOs,
/// using dynamic type resolution at runtime. Supports both ID-based and No-ID-based DTO transformations.
/// </summary>
public class GlobalMapper
{
    // Mappers para DTOs con ID
    private readonly Dictionary<Type, object> _entityMappersWithId = new();
    private readonly Dictionary<Type, object> _dtoMappersWithId = new();

    // Mappers para DTOs sin ID
    private readonly Dictionary<Type, object> _entityMappersNoId = new();
    private readonly Dictionary<Type, object> _dtoMappersNoId = new();

    /// <summary>
    /// Initializes the global mapper with all supported mappers registered.
    /// </summary>
    public GlobalMapper()
    {
        RegisterMapperWithId(new ProjectorDtoMapper());
        RegisterMapperWithId(new WhiteboardDtoMapper());
        RegisterMapperNoId(new ProjectorNoIdDtoMapper());
        RegisterMapperNoId(new WhiteboardNoIdDtoMapper());
    }

    private void RegisterMapperWithId<TComponent, TDto>(LearningComponentDtoMapper<TComponent, TDto> mapper)
        where TComponent : LearningComponent
        where TDto : LearningComponentDto
    {
        _entityMappersWithId[typeof(TComponent)] = mapper;
        _dtoMappersWithId[typeof(TDto)] = mapper;
    }

    private void RegisterMapperNoId<TComponent, TDto>(LearningComponentNoIdDtoMapper<TComponent, TDto> mapper)
        where TComponent : LearningComponent
        where TDto : LearningComponentNoIdDto
    {
        _entityMappersNoId[typeof(TComponent)] = mapper;
        _dtoMappersNoId[typeof(TDto)] = mapper;
    }

    /// <summary>
    /// Converts a LearningComponent instance to its corresponding DTO with ID.
    /// </summary>
    public LearningComponentDto ToDtoWithId(LearningComponent component)
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
    /// Converts a LearningComponent instance to its corresponding DTO without ID.
    /// </summary>
    public LearningComponentNoIdDto ToDtoNoId(LearningComponent component)
    {
        var type = component.GetType();
        if (!_entityMappersNoId.TryGetValue(type, out var mapperObj))
        {
            throw new NotSupportedException($"No No-ID mapper registered for type {type.Name}.");
        }

        dynamic mapper = mapperObj;
        dynamic specificComponent = component;

        var result = mapper.ToDto(specificComponent);

        if (result is not LearningComponentNoIdDto dto)
            throw new InvalidCastException($"Returned DTO type {result.GetType().Name} is not assignable to LearningComponentNoIdDto.");

        return dto;
    }

    /// <summary>
    /// Converts a DTO with ID to its corresponding LearningComponent.
    /// </summary>
    public TComponent ToEntityFromIdDto<TComponent>(object dto) where TComponent : LearningComponent
    {
        var type = dto.GetType();
        if (!_dtoMappersWithId.TryGetValue(type, out var mapperObj))
        {
            throw new NotSupportedException($"No ID-based mapper registered for DTO type {type.Name}.");
        }

        dynamic mapper = mapperObj;
        dynamic specificDto = dto;

        return mapper.ToEntity(specificDto);
    }

    /// <summary>
    /// Converts a DTO without ID to its corresponding LearningComponent.
    /// </summary>
    public TComponent ToEntityFromNoIdDto<TComponent>(object dto) where TComponent : LearningComponent
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
