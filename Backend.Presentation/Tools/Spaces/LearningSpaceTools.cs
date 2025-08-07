using ModelContextProtocol.Server;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tools.Spaces;

/// <summary>
/// Provides tools for managing and retrieving learning spaces and their associated data.
/// </summary>
[McpServerToolType]
public static class LearningSpaceTools
{
    /// <summary>
    /// Retrieves a learning space by its unique identifier.
    /// </summary>
    /// <param name="learningSpaceId">The unique identifier of the learning space to retrieve. Must be a valid integer format.</param>
    /// <param name="learningSpaceServices">An instance of <see cref="ILearningSpaceServices"/> used to perform operations related to learning spaces.</param>
    /// <returns>An object representing the learning space in Dto format if found, or an error object indicating the issue.
    /// Returns an error object if the <paramref name="learningSpaceId"/> is invalid, the learning space is not found, 
    /// or a concurrency conflict occurs.</returns>
    [McpServerTool, Description("Obtiene un espacio de aprendizaje por su Id")]
    public static async Task<object?> GetLearningSpaceTool(
        [Description("Id del espacio de aprendizaje")] int learningSpaceId,
        [FromServices] ILearningSpaceServices learningSpaceServices)
    {
        var errors = new List<object>();

        if (!Id.TryCreate(learningSpaceId, out var validId))
            errors.Add(new { Parameter = "LearningSpaceId", Message = "Formato de id de espacio de aprendizaje inválido." });

        if (errors.Count > 0)
            return errors;

        try
        {
            var learningSpace = await learningSpaceServices.GetLearningSpaceAsync(learningSpaceId);

            if (learningSpace == null)
                return new { NotFound = $"No se encontró un espacio de aprendizaje con id {learningSpaceId}." };

            var dto = LearningSpaceDtoMapper.ToDto(learningSpace);

            return dto;
        }
        catch (NotFoundException ex)
        {
            return new { NotFound = ex.Message };
        }
        catch (ConcurrencyConflictException ex)
        {
            return new { Conflict = ex.Message };
        }
    }

    /// <summary>
    /// Retrieves a list of learning spaces associated with the specified floor ID.
    /// </summary>
    /// <param name="floorId">The unique identifier of the floor for which learning spaces are to be retrieved. Must be a valid floor ID format.</param>
    /// <param name="learningSpaceServices">An instance of <see cref="ILearningSpaceServices"/> used to perform the retrieval operation.</param>
    /// <returns>A list of learning space DTOs if found, or an error object indicating the issue.</returns>
    [McpServerTool, Description("Obtiene una lista de los espacios de aprendizaje por el Id del piso.")]
    public static async Task<object?> GetLearningSpacesListTool(
        [Description("Id del piso")] int floorId,
        [FromServices] ILearningSpaceServices learningSpaceServices)
    {
        var errors = new List<object>();

        if (!Id.TryCreate(floorId, out var validId))
            errors.Add(new { Parameter = "FloorId", Message = "Formato de id de piso inválido." });

        if (errors.Count > 0)
            return errors;

        try
        {
            var learningSpaces = await learningSpaceServices.GetLearningSpacesListAsync(floorId);

            if (learningSpaces == null || learningSpaces.Count == 0)
                return new { NotFound = $"No se encontraron espacios de aprendizaje para el piso con id {floorId}." };

            var dtos = learningSpaces.Select(LearningSpaceDtoMapper.ToDto).ToList();

            return dtos;
        }
        catch (NotFoundException ex)
        {
            return new { NotFound = ex.Message };
        }
        catch (ConcurrencyConflictException ex)
        {
            return new { Conflict = ex.Message };
        }
    }

    /// <summary>
    /// Creates a new learning space associated with a floor.
    /// </summary>
    /// <param name="floorId">Id of the floor to associate the space with.</param>
    /// <param name="name">Name of the space (max 100 characters).</param>
    /// <param name="type">Type: Auditorium, Classroom, Laboratory.</param>
    /// <param name="capacity">Maximum capacity (positive integer).</param>
    /// <param name="height">Height (double > 0).</param>
    /// <param name="width">Width (double > 0).</param>
    /// <param name="length">Length (double > 0).</param>
    /// <param name="colorFloor">Floor color (see allowed colors list).</param>
    /// <param name="colorWalls">Wall color (see allowed colors list).</param>
    /// <param name="colorCeiling">Ceiling color (see allowed colors list).</param>
    /// <param name="learningSpaceServices">Learning space service.</param>
    /// <returns>Success object with created learning space if creation was successful, or an error object if not.</returns>
    [McpServerTool, Description(
        "Crea un nuevo espacio de aprendizaje en un piso. Tipos permitidos: Auditorium, Classroom, Laboratory. Colores permitidos: Red, Green, Blue, Yellow, Black, White, Orange, Purple, Gray, Brown, Pink, Cyan, Magenta, Lime, Teal, Olive, Navy, Maroon, Silver, Gold, DarkRed, Crimson, Salmon, DarkOrange, Peach, DarkGreen, Emerald, Aqua, Turquoise, Mint, DarkBlue, SteelBlue, SkyBlue, Indigo, Violet.")]
    public static async Task<object?> CreateLearningSpaceTool(
        [Description("Id del piso")] int floorId,
        [Description("Nombre del espacio (máximo 100 caracteres)")] string name,
        [Description("Tipo: Auditorium, Classroom, Laboratory")] string type,
        [Description("Capacidad máxima (entero positivo)")] int capacity,
        [Description("Altura (double > 0)")] double height,
        [Description("Ancho (double > 0)")] double width,
        [Description("Largo (double > 0)")] double length,
        [Description("Color del piso")] string colorFloor,
        [Description("Color de las paredes")] string colorWalls,
        [Description("Color del techo")] string colorCeiling,
        [FromServices] ILearningSpaceServices learningSpaceServices)
    {
        var errors = new List<object>();

        // Validar floorId
        if (!Id.TryCreate(floorId, out var validFloorId))
            errors.Add(new { Parameter = "FloorId", Message = "Formato de id de piso inválido." });

        // Validar nombre
        if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
            errors.Add(new { Parameter = "Name", Message = "El nombre es requerido y debe tener máximo 100 caracteres." });

        // Validar tipo
        var allowedTypes = new[] { "Auditorium", "Classroom", "Laboratory" };
        if (!allowedTypes.Contains(type))
            errors.Add(new { Parameter = "Type", Message = "El tipo debe ser Auditorium, Classroom o Laboratory." });

        // Validar capacidad
        if (capacity <= 0)
            errors.Add(new { Parameter = "Capacity", Message = "La capacidad debe ser un entero positivo." });

        // Validar dimensiones
        if (height <= 0)
            errors.Add(new { Parameter = "Height", Message = "La altura debe ser mayor que cero." });
        if (width <= 0)
            errors.Add(new { Parameter = "Width", Message = "El ancho debe ser mayor que cero." });
        if (length <= 0)
            errors.Add(new { Parameter = "Length", Message = "El largo debe ser mayor que cero." });

        // Validar colores
        if (!Colors.TryCreate(colorFloor, out var colorFloorObj))
            errors.Add(new { Parameter = "ColorFloor", Message = "Color de piso inválido." });
        if (!Colors.TryCreate(colorWalls, out var colorWallsObj))
            errors.Add(new { Parameter = "ColorWalls", Message = "Color de paredes inválido." });
        if (!Colors.TryCreate(colorCeiling, out var colorCeilingObj))
            errors.Add(new { Parameter = "ColorCeiling", Message = "Color de techo inválido." });

        if (errors.Count > 0)
            return errors;

        try
        {
            var entityName = EntityName.Create(name);
            var learningSpaceType = LearningSpaceType.Create(type);
            var capacityObj = Capacity.Create(capacity);
            var heightObj = Size.Create(height);
            var widthObj = Size.Create(width);
            var lengthObj = Size.Create(length);

            var learningSpace = new Domain.Entities.Spaces.LearningSpace(
                entityName,
                learningSpaceType,
                capacityObj,
                heightObj,
                widthObj,
                lengthObj,
                colorFloorObj!,
                colorWallsObj!,
                colorCeilingObj!
            );

            var result = await learningSpaceServices.CreateLearningSpaceAsync(floorId, learningSpace);

            if (!result)
            {
                return new
                {
                    Error = "CreationFailed",
                    Message = "No se pudo crear el espacio de aprendizaje."
                };
            }

            return new
            {
                Success = true,
                Message = "Espacio de aprendizaje creado exitosamente.",
                LearningSpace = LearningSpaceDtoMapper.ToDto(learningSpace)
            };
        }
        catch (ValidationException ex)
        {
            return new { ValidationError = ex.Message };
        }
        catch (DuplicatedEntityException ex)
        {
            return new { Duplicate = ex.Message };
        }
        catch (NotFoundException ex)
        {
            return new { NotFound = ex.Message };
        }
        catch (Exception ex)
        {
            return new { Error = ex.Message };
        }
    }

    /// <summary>
    /// Updates an existing learning space with new values for specified properties.
    /// </summary>
    /// <param name="learningSpaceId">The unique identifier of the learning space to update.</param>
    /// <param name="name">Optional new name for the space (max 100 characters).</param>
    /// <param name="type">Optional new type: Auditorium, Classroom, Laboratory.</param>
    /// <param name="capacity">Optional new maximum capacity (positive integer).</param>
    /// <param name="height">Optional new height (double > 0).</param>
    /// <param name="width">Optional new width (double > 0).</param>
    /// <param name="length">Optional new length (double > 0).</param>
    /// <param name="colorFloor">Optional new floor color.</param>
    /// <param name="colorWalls">Optional new wall color.</param>
    /// <param name="colorCeiling">Optional new ceiling color.</param>
    /// <param name="learningSpaceServices">Learning space service.</param>
    /// <returns>Success object with updated learning space if update was successful, or an error object if not.</returns>
    [McpServerTool, Description(
        "Actualiza un espacio de aprendizaje existente. Solo se actualizarán los parámetros proporcionados. Tipos permitidos: Auditorium, Classroom, Laboratory. Colores permitidos: Red, Green, Blue, Yellow, Black, White, Orange, Purple, Gray, Brown, Pink, Cyan, Magenta, Lime, Teal, Olive, Navy, Maroon, Silver, Gold, DarkRed, Crimson, Salmon, DarkOrange, Peach, DarkGreen, Emerald, Aqua, Turquoise, Mint, DarkBlue, SteelBlue, SkyBlue, Indigo, Violet.")]
    public static async Task<object?> UpdateLearningSpaceTool(
        [Description("Id del espacio de aprendizaje")] int learningSpaceId,
        [Description("Nuevo nombre (máximo 100 caracteres)")] string? name,
        [Description("Nuevo tipo: Auditorium, Classroom, Laboratory")] string? type,
        [Description("Nueva capacidad máxima (entero positivo)")] int? capacity,
        [Description("Nueva altura (double > 0)")] double? height,
        [Description("Nuevo ancho (double > 0)")] double? width,
        [Description("Nuevo largo (double > 0)")] double? length,
        [Description("Nuevo color del piso")] string? colorFloor,
        [Description("Nuevo color de las paredes")] string? colorWalls,
        [Description("Nuevo color del techo")] string? colorCeiling,
        [FromServices] ILearningSpaceServices learningSpaceServices)
    {
        var errors = new List<object>();

        // Validate learningSpaceId
        if (!Id.TryCreate(learningSpaceId, out var validId))
            errors.Add(new { Parameter = "LearningSpaceId", Message = "Formato de id de espacio de aprendizaje inválido." });

        // Validate optional parameters if provided
        if (!string.IsNullOrWhiteSpace(name) && name.Length > 100)
            errors.Add(new { Parameter = "Name", Message = "El nombre debe tener máximo 100 caracteres." });

        if (!string.IsNullOrWhiteSpace(type))
        {
            var allowedTypes = new[] { "Auditorium", "Classroom", "Laboratory" };
            if (!allowedTypes.Contains(type))
                errors.Add(new { Parameter = "Type", Message = "El tipo debe ser Auditorium, Classroom o Laboratory." });
        }

        if (capacity.HasValue && capacity.Value <= 0)
            errors.Add(new { Parameter = "Capacity", Message = "La capacidad debe ser un entero positivo." });

        if (height.HasValue && height.Value <= 0)
            errors.Add(new { Parameter = "Height", Message = "La altura debe ser mayor que cero." });

        if (width.HasValue && width.Value <= 0)
            errors.Add(new { Parameter = "Width", Message = "El ancho debe ser mayor que cero." });

        if (length.HasValue && length.Value <= 0)
            errors.Add(new { Parameter = "Length", Message = "El largo debe ser mayor que cero." });

        // Validate colors if provided
        if (!string.IsNullOrWhiteSpace(colorFloor) && !Colors.TryCreate(colorFloor, out _))
            errors.Add(new { Parameter = "ColorFloor", Message = "Color de piso inválido." });

        if (!string.IsNullOrWhiteSpace(colorWalls) && !Colors.TryCreate(colorWalls, out _))
            errors.Add(new { Parameter = "ColorWalls", Message = "Color de paredes inválido." });

        if (!string.IsNullOrWhiteSpace(colorCeiling) && !Colors.TryCreate(colorCeiling, out _))
            errors.Add(new { Parameter = "ColorCeiling", Message = "Color de techo inválido." });

        if (errors.Count > 0)
            return errors;

        try
        {
            var learningSpace = await learningSpaceServices.GetLearningSpaceAsync(learningSpaceId);

            if (learningSpace == null)
                return new { NotFound = $"No se encontró un espacio de aprendizaje con id {learningSpaceId}." };

            // Update properties if provided
            if (!string.IsNullOrWhiteSpace(name))
                learningSpace.Name = EntityName.Create(name);
            if (!string.IsNullOrWhiteSpace(type))
                learningSpace.Type = LearningSpaceType.Create(type);
            if (capacity.HasValue)
                learningSpace.MaxCapacity = Capacity.Create(capacity.Value);
            if (height.HasValue)
                learningSpace.Height = Size.Create(height.Value);
            if (width.HasValue)
                learningSpace.Width = Size.Create(width.Value);
            if (length.HasValue)
                learningSpace.Length = Size.Create(length.Value);
            if (!string.IsNullOrWhiteSpace(colorFloor))
                learningSpace.ColorFloor = Colors.Create(colorFloor);
            if (!string.IsNullOrWhiteSpace(colorWalls))
                learningSpace.ColorWalls = Colors.Create(colorWalls);
            if (!string.IsNullOrWhiteSpace(colorCeiling))
                learningSpace.ColorCeiling = Colors.Create(colorCeiling);

            // Save the updated learning space
            var result = await learningSpaceServices.UpdateLearningSpaceAsync(learningSpaceId, learningSpace);

            if (!result)
            {
                return new
                {
                    Error = "UpdateFailed",
                    Message = "No se pudo actualizar el espacio de aprendizaje."
                };
            }

            return new
            {
                Success = true,
                Message = "Espacio de aprendizaje actualizado exitosamente.",
                LearningSpace = LearningSpaceDtoMapper.ToDto(learningSpace)
            };

        }
        catch (ValidationException ex)
        {
            return new { ValidationError = ex.Message };
        }
        catch (NotFoundException ex)
        {
            return new { NotFound = ex.Message };
        }
        catch (ConcurrencyConflictException ex)
        {
            return new { Conflict = ex.Message };
        }
        catch (Exception ex)
        {
            return new { Error = ex.Message };
        }
    }
}
