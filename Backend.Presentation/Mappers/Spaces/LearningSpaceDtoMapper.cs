using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.Spaces;

/// <summary>
/// Mapper class for converting between LearningSpace domain entities and their corresponding DTOs.
/// </summary>
internal static class LearningSpaceDtoMapper
{
    /// <summary>
    /// Converts a LearningSpace entity to a detailed LearningSpaceDto.
    /// </summary>
    /// <param name="learningSpaceEntity">The LearningSpace domain entity to convert.</param>
    /// <returns>A LearningSpaceDto containing all relevant details.</returns>
    internal static LearningSpaceDto ToDto(LearningSpace learningSpaceEntity)
    {
        return new LearningSpaceDto(
            learningSpaceEntity.Name.Name,
            learningSpaceEntity.Type.Value,
            learningSpaceEntity.MaxCapacity.Value,
            learningSpaceEntity.Height.Value,
            learningSpaceEntity.Width.Value,
            learningSpaceEntity.Length.Value,
            learningSpaceEntity.ColorFloor.Value,
            learningSpaceEntity.ColorWalls.Value,
            learningSpaceEntity.ColorCeiling.Value

        );
    }

    /// <summary>
    /// Converts a LearningSpace entity to a simplified LearningSpaceListDto for listing purposes.
    /// </summary>
    /// <param name="learningSpaceEntity">The LearningSpace domain entity to convert.</param>
    /// <returns>A LearningSpaceListDto with minimal identifying information.</returns>
    internal static LearningSpaceListDto ToDtoList(LearningSpace learningSpaceEntity)
    {
        return new LearningSpaceListDto(
            learningSpaceEntity.LearningSpaceId,
            learningSpaceEntity.Name.Name,
            learningSpaceEntity.Type.Value
        );
    }

    /// <summary>
    /// Converts a LearningSpaceDto and floor ID into a LearningSpace domain entity.
    /// </summary>
    /// <param name="floorNumber">The floor ID as a string.</param>
    /// <param name="dto">The LearningSpaceDto containing input data from the user or UI.</param>
    /// <returns>A fully validated LearningSpace domain entity.</returns>
    internal static LearningSpace ToEntity(LearningSpaceDto dto)
    {
        var errors = new List<ValidationError>();

        if (!EntityName.TryCreate(dto.Name, out var validName))
            errors.Add(new ValidationError("Name", "El nombre no debe estar vacío y no debe de tener más de 100 caracteres."));

        if (!LearningSpaceType.TryCreate(dto.Type, out var validType))
            errors.Add(new ValidationError("Type", "Tipo de espacio de Aprendizaje Inválido."));

        if (!Capacity.TryCreate(dto.MaxCapacity, out var validMaxCapacity))
            errors.Add(new ValidationError("MaxCapacity", "Capacidad máxima inválida. Debe ser un número entero positivo."));

        if (!Size.TryCreate(dto.Height, out var validHeight))
            errors.Add(new ValidationError("Height", "Altura inválida. Debe ser un número entero positivo."));

        if (!Size.TryCreate(dto.Width, out var validWidth))
            errors.Add(new ValidationError("Width", "Ancho inválido. Debe ser un número entero positivo."));

        if (!Size.TryCreate(dto.Length, out var validLength))
            errors.Add(new ValidationError("Length", "Longitud inválida. Debe ser un número entero positivo."));

        if (!Colors.TryCreate(dto.ColorFloor, out var validColorFloor))
            errors.Add(new ValidationError("ColorFloor", "Color de piso inválido."));

        if (!Colors.TryCreate(dto.ColorWalls, out var validColorWalls))
            errors.Add(new ValidationError("ColorWalls", "Color de paredes inválido."));

        if (!Colors.TryCreate(dto.ColorCeiling, out var validColorCeiling))
            errors.Add(new ValidationError("ColorCeiling", "Color de cielo raso inválido."));

        if (errors.Count > 0)
            throw new ValidationException(errors);

        return new LearningSpace(
            validName!,
            validType!,
            validMaxCapacity!,
            validHeight!,
            validWidth!,
            validLength!,
            validColorFloor!,
            validColorWalls!,
            validColorCeiling!
        );
    }


}
