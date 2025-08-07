using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;

/// <summary>
/// Abstract base class for mapping between a <see cref="LearningComponent"/> entity
/// and a corresponding DTO that does not contain an identifier (e.g., used for creation).
/// </summary>
/// <typeparam name="T">The specific type of <see cref="LearningComponent"/> to map from.</typeparam>
/// <typeparam name="TDtoNoId">The DTO type without ID, used to map to/from the component.</typeparam>
public abstract class LearningComponentNoIdDtoMapper<T, TDtoNoId>
    where T : LearningComponent
    where TDtoNoId : LearningComponentNoIdDto
{
    /// <summary>
    /// Converts a domain entity of type <typeparamref name="T"/> to its corresponding no-ID DTO.
    /// </summary>
    /// <param name="component">The component to convert.</param>
    /// <returns>A <typeparamref name="TDtoNoId"/> DTO representing the component without an ID.</returns>
    public abstract TDtoNoId ToDto(T component);

    /// <summary>
    /// Converts a no-ID DTO of type <typeparamref name="TDtoNoId"/> to its corresponding domain entity.
    /// </summary>
    /// <param name="dto">The no-ID DTO to convert.</param>
    /// <returns>A <typeparamref name="T"/> entity created from the no-ID DTO.</returns>
    public abstract T ToEntity(TDtoNoId dto);
}
