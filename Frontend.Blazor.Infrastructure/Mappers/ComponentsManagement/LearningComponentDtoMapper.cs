using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Kiota.Models;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Infrastructure.Mappers.ComponentsManagement;


/// <summary>
/// Abstract base class for mapping between a <see cref="LearningComponent"/> entity
/// and its corresponding DTO that includes an identifier.
/// </summary>
/// <typeparam name="T">The specific type of <see cref="LearningComponent"/> to map from.</typeparam>
/// <typeparam name="TDto">The DTO type that includes an ID, mapped to/from the component.</typeparam>
public abstract class LearningComponentDtoMapper<T, TDto>
    where T : LearningComponent
    where TDto : LearningComponentDto
{
    /// <summary>
    /// Converts a domain entity of type <typeparamref name="T"/> to its corresponding DTO.
    /// </summary>
    /// <param name="component">The entity instance to convert.</param>
    /// <returns>A <typeparamref name="TDto"/> representing the entity.</returns>
    public abstract TDto ToDto(T component);

    /// <summary>
    /// Converts a DTO of type <typeparamref name="TDto"/> to its corresponding domain entity.
    /// </summary>
    /// <param name="dto">The DTO to convert.</param>
    /// <returns>A <typeparamref name="T"/> entity constructed from the DTO.</returns>
    public abstract T ToEntity(TDto dto);
}





