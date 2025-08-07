using Microsoft.AspNetCore.Mvc;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Drawing;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tools.ComponentsManagement;

/// <summary>
/// Provides server tools for managing learning components in the system.
/// Includes methods for retrieving and creating learning components, such as projectors, using dependency-injected services and mappers.
/// </summary>
[McpServerToolType]
public static class LearningComponentTools
{
    /// <summary>
    /// Retrieves a single learning component by its unique identifier.
    /// </summary>
    /// <param name="learningComponentId">The ID of the learning component.</param>
    /// <param name="mapper">The global mapper for entity-DTO conversion.</param>
    /// <param name="learningComponentServices">The service for learning component operations.</param>
    /// <returns>The DTO of the learning component, or an error/NotFound object.</returns>
    [McpServerTool, Description("Obtiene un componente de aprendizaje por su Id.")]
    public static async Task<object?> GetSingleLearningComponentByIdTool(
       [Description("Id del componente de aprendizaje")] int learningComponentId,
       [FromServices] GlobalMapper mapper,
       [FromServices] ILearningComponentServices learningComponentServices)
    {
        var errors = new List<object>();

        if (!Id.TryCreate(learningComponentId, out var validId))
            errors.Add(new { Parameter = "LearningComponentId", Message = "Formato de id de componente de aprendizaje inválido." });

        if (errors.Count > 0)
            return errors;

        try
        {
            var learningComponent = await learningComponentServices.GetSingleLearningComponentByIdAsync(learningComponentId);

            if (learningComponent == null)
                return new { NotFound = $"No se encontró un componente de aprendizaje con id {learningComponentId}." };

            var dto = mapper.ToDto(learningComponent);

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
    /// Retrieves a paginated list of learning components for a given learning space.
    /// </summary>
    /// <param name="learningSpaceId">The ID of the learning space.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="pageIndex">The page index.</param>
    /// <param name="stringSearch">A search string to filter results.</param>
    /// <param name="mapper">The global mapper for entity-DTO conversion.</param>
    /// <param name="learningComponentServices">The service for learning component operations.</param>
    /// <returns>A list of DTOs or an error/NotFound object.</returns>
    [McpServerTool, Description("Obtiene un componente de aprendizaje por el ID de un espacio de aprendizaje.")]
    public static async Task<object?> GetLearningComponentsByIdTool(
       [Description("Id del espacio de aprendizaje")] int learningSpaceId,
       int pageSize,
       int pageIndex,
       string stringSearch,
       [FromServices] GlobalMapper mapper,
       [FromServices] ILearningComponentServices learningComponentServices)
    {
        var errors = new List<object>();

        if (!Id.TryCreate(learningSpaceId, out var validId))
            errors.Add(new { Parameter = "LearningSpaceId", Message = "Formato de id de espacio de aprendizaje inválido." });

        if (errors.Count > 0)
            return errors;

        try
        {
            var learningComponents = await learningComponentServices.GetLearningComponentsByIdAsync(learningSpaceId, pageSize, pageIndex, stringSearch);

            if (learningComponents == null || learningComponents.Count == 0)
                return new { NotFound = $"No se encontraron componentes de aprendizaje para el espacio de aprendizaje con id {learningSpaceId}." };

            var dtos = learningComponents.Select(mapper.ToDto).ToList();

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
    /// Creates a new projector in a learning space.
    /// </summary>
    /// <param name="projectorServices">The service for projector operations.</param>
    /// <param name="mapper">The global mapper for entity-DTO conversion.</param>
    /// <param name="learningSpaceId">The ID of the learning space.</param>
    /// <param name="orientation">The orientation of the projector.</param>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="z">Z coordinate.</param>
    /// <param name="width">Width of the projector.</param>
    /// <param name="length">Length of the projector.</param>
    /// <param name="height">Height of the projector.</param>
    /// <param name="projectedHeight">Projected height.</param>
    /// <param name="projectedWidth">Projected width.</param>
    /// <param name="projectedContent">Projected content.</param>
    /// <returns>A result object indicating success or failure, with the created projector data if successful.</returns>
    [McpServerTool, Description("Crea un nuevo proyector en un espacio de aprendizaje")]
    public static async Task<object?> CreateProjectorTool(
        [FromServices] IProjectorServices projectorServices,
        [FromServices] GlobalMapper mapper,
        [Description("Id del espacio de aprendizaje")] int learningSpaceId,
        [Description("Orientación del proyector")] string orientation,
        [Description("Coordenada X")] double x,
        [Description("Coordenada Y")] double y,
        [Description("Coordenada Z")] double z,
        [Description("Ancho")] double width,
        [Description("Largo")] double length,
        [Description("Alto")] double height,
        [Description("Alto proyectado")] double projectedHeight,
        [Description("Ancho proyectado")] double projectedWidth,
        [Description("Contenido proyectado")] string projectedContent
    )
    {
        var errors = new List<object>();

        if (!Id.TryCreate(learningSpaceId, out var validLearningSpaceId))
            errors.Add(new { Parameter = "LearningSpaceId", Message = "Id de espacio inválido" });

        if (errors.Count > 0)
            return errors;

        try
        {
            var projectorDto = new ProjectorNoIdDto(
                orientation,
                new PositionDto(x, y, z),
                new DimensionsDto(width, length, height),
                new ProjectionAreaDto(projectedHeight, projectedWidth),
                projectedContent
            );

            var projectorEntity = mapper.ToEntity(projectorDto);

            var success = await projectorServices.AddProjectorAsync(learningSpaceId, (Projector)projectorEntity);

            if (!success)
            {
                return new
                {
                    Error = "CreationFailed",
                    Message = "No se pudo crear el proyector."
                };
            }

            return new
            {
                Success = true,
                Message = "Proyector creado exitosamente.",
                Projector = mapper.ToDto(projectorEntity)
            };
        }
        catch (ValidationException ex)
        {
            return new
            {
                Error = "ValidationError",
                Message = "Error de validación en los datos del proyector.",
                Details = ex.Errors.Select(e => new { Field = e.Parameter, Error = e.Message })
            };
        }
        catch (DuplicatedEntityException ex)
        {
            return new
            {
                Error = "DuplicatedEntity",
                Message = ex.Message
            };
        }
        catch (Exception ex)
        {
            return new
            {
                Error = "UnexpectedError",
                Message = "Ocurrió un error inesperado al crear el proyector.",
                Details = ex.Message
            };
        }
    }

    /// <summary>
    /// Retrieves a list of all existing whiteboards.
    /// </summary>
    /// <param name="mapper">The mapper used for converting the projector entities to their corresponding DTO representations.</param>
    /// <param name="whiteboardServices">The service used to manage and retrieve whiteboard data.</param>
    /// <returns>A list of whiteboard DTOs, or an error object in case of NotFound or concurrency conflicts.</returns>
    [McpServerTool, Description("Obtiene la lista de todas las pizarras existentes")]
    public static async Task<object?> GetWhiteboards(
        [FromServices] GlobalMapper mapper,
        [FromServices] IWhiteboardServices whiteboardServices)
    {
        var errors = new List<object>();

        if (errors.Count > 0)
            return errors;

        try
        {
            var whiteboards = await whiteboardServices.GetWhiteboardAsync();
            var dto = whiteboards.Select(whiteboard => mapper.ToDto(whiteboard)).ToList();

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
    /// Edits an existing projector entity identified by its unique ID.
    /// </summary>
    /// <param name="projectorId">The ID of the projector to edit.</param>
    /// <param name="learningSpaceId">The ID of the learning space to which the projector belongs.</param>
    /// <param name="projectorServices">The service handling projector-related operations.</param>
    /// <param name="learningComponentServices">The service handling learning component operations.</param>
    /// <param name="learningSpaceServices">The service handling learning space operations.</param>
    /// <param name="projectedContent">The content to be projected by the projector. Default is null.</param>
    /// <param name="projectedWidth">The width of the projected content. Default is null.</param>
    /// <param name="projectedHeight">The height of the projected content. Default is null.</param>
    /// <param name="orientation">The orientation of the projector. Default is null.</param>
    /// <param name="X">The X-coordinate position of the projector. Default is null.</param>
    /// <param name="Y">The Y-coordinate position of the projector. Default is null.</param>
    /// <param name="Z">The Z-coordinate position of the projector. Default is null.</param>
    /// <param name="Length">The length of the projector. Default is null.</param>
    /// <param name="Height">The height of the projector. Default is null.</param>
    /// <param name="Width">The width of the projector. Default is null.</param>
    /// <returns>Returns an object containing the result of the edit operation, or a list of errors if validation fails.</returns>
    [McpServerTool, Description("Edita un proyector dado el id del proyector")]
    public static async Task<object?> PutProjector(
        [Description("Id del proyector a editar")] int projectorId,
        [Description("Id del espacio de aprendizaje al que pertenece el proyector")] int learningSpaceId,
        [FromServices] IProjectorServices projectorServices,
        [FromServices] ILearningComponentServices learningComponentServices,
        [FromServices] ILearningSpaceServices learningSpaceServices,
        [Description("Contenido proyectado")] string? projectedContent = null,
        [Description("Largo de proyección")] double? projectedWidth = null,
        [Description("Alto de proyección")] double? projectedHeight = null,
        [Description("Orientación del proyector")] string? orientation = null,
        [Description("Posición X")] double? X = null,
        [Description("Posición Y")] double? Y = null,
        [Description("Posición Z")] double? Z = null,
        [Description("Largo")] double? Length = null,
        [Description("Alto")] double? Height = null,
        [Description("Ancho")] double? Width = null)
    {
        var errors = new List<object>();

        if (errors.Count > 0)
            return errors;

        try
        {
            var existingProjector = await learningComponentServices.GetSingleLearningComponentByIdAsync(projectorId);
            if (existingProjector == null)
                return new { NotFound = $"No se encontró un proyector con id {projectorId}." };

            var learningSpace = await learningSpaceServices.GetLearningSpaceAsync(learningSpaceId);
            if (learningSpace == null)
                return new { NotFound = $"No se encontró un espacio de aprendizaje con id {learningSpaceId}." };

            // Convertir el componente existente a Projector
            var currentProjector = existingProjector as Projector;
            if (currentProjector == null)
                return new { BadRequest = "El componente no es un proyector válido." };

            // Usar valores nuevos si se proporcionan, sino mantener los existentes
            var updatedProjectorDto = new ProjectorDto(
                projectorId,
                orientation ?? currentProjector.Orientation.Value,
                new PositionDto(
                    X ?? currentProjector.Position.X, 
                    Y ?? currentProjector.Position.Y, 
                    Z ?? currentProjector.Position.Z),
                new DimensionsDto(
                    Width ?? currentProjector.Dimensions.Width, 
                    Length ?? currentProjector.Dimensions.Length, 
                    Height ?? currentProjector.Dimensions.Height),
                new ProjectionAreaDto(
                    projectedHeight ?? currentProjector.ProjectionArea.Height, 
                    projectedWidth ?? currentProjector.ProjectionArea.Length),
                projectedContent ?? currentProjector.ProjectedContent
            );

            var projectorMapper = new ProjectorDtoMapper();
            var updatedProjector = projectorMapper.ToEntity(updatedProjectorDto);
            var result = await projectorServices.UpdateProjectorAsync(
                projectorId,
                learningSpaceId,
                updatedProjector
            );

            if (result)
            {
                var mapper = new ProjectorDtoMapper();
                return mapper.ToDto(updatedProjector);
            }
            else
            {
                return new { Error = "No se pudo actualizar el proyector." };
            }
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
    /// Edits a whiteboard based on its unique identifier.
    /// </summary>
    /// <param name="whiteboardId">The ID of the whiteboard to be edited.</param>
    /// <param name="learningSpaceId">The ID of the learning space to which the whiteboard belongs.</param>
    /// <param name="whiteboardServices">The service responsible for whiteboard operations.</param>
    /// <param name="learningComponentServices">The service responsible for managing learning components.</param>
    /// <param name="learningSpaceServices">The service responsible for managing learning spaces.</param>
    /// <param name="orientation">The orientation of the whiteboard.</param>
    /// <param name="X">The X-coordinate position of the whiteboard.</param>
    /// <param name="Y">The Y-coordinate position of the whiteboard.</param>
    /// <param name="Z">The Z-coordinate position of the whiteboard.</param>
    /// <param name="Length">The length of the whiteboard.</param>
    /// <param name="Height">The height of the whiteboard.</param>
    /// <param name="Width">The width of the whiteboard.</param>
    /// <param name="Color">The color of the whiteboard.</param>
    /// <returns>An object representing the result of the whiteboard editing operation, or an error/NotFound object.</returns>
    [McpServerTool, Description("Edita un whiteboard dado el id del whiteboard")]
    public static async Task<object?> PutWhiteboard(
        [Description("Id de la pizarra a editar")] int whiteboardId,
        [Description("Id del espacio de aprendizaje al que pertenece la pizarra")] int learningSpaceId,
        [FromServices] IWhiteboardServices whiteboardServices,
        [FromServices] ILearningComponentServices learningComponentServices,
        [FromServices] ILearningSpaceServices learningSpaceServices,
        [Description("Orientación de la pizarra")] string? orientation = null,
        [Description("Posición X")] double? X = null,
        [Description("Posición Y")] double? Y = null,
        [Description("Posición Z")] double? Z = null,
        [Description("Largo")] double? Length = null,
        [Description("Alto")] double? Height = null,
        [Description("Ancho")] double? Width = null,
        [Description("Color")] string? Color = null)
    {   
        var errors = new List<object>();

        if (errors.Count > 0)
            return errors;

        try
        {
            var existingWhiteboard = await learningComponentServices.GetSingleLearningComponentByIdAsync(whiteboardId);
            if (existingWhiteboard == null)
                return new { NotFound = $"No se encontró un pizarra con id {whiteboardId}." };

            var learningSpace = await learningSpaceServices.GetLearningSpaceAsync(learningSpaceId);
            if (learningSpace == null)
                return new { NotFound = $"No se encontró un espacio de aprendizaje con id {learningSpaceId}." };

            // Convertir el componente existente a Whiteboard
            var currentWhiteboard = existingWhiteboard as Whiteboard;
            if (currentWhiteboard == null)
                return new { BadRequest = "El componente no es una pizarra válido." };

            // Usar valores nuevos si se proporcionan, sino mantener los existentes
            var updatedWhiteboardDto = new WhiteboardDto(
                whiteboardId,
                orientation ?? currentWhiteboard.Orientation.Value,
                new PositionDto(
                    X ?? currentWhiteboard.Position.X,
                    Y ?? currentWhiteboard.Position.Y,
                    Z ?? currentWhiteboard.Position.Z),
                new DimensionsDto(
                    Width ?? currentWhiteboard.Dimensions.Width,
                    Length ?? currentWhiteboard.Dimensions.Length,
                    Height ?? currentWhiteboard.Dimensions.Height),
                Color ?? currentWhiteboard.MarkerColor.Value
            );

            var whiteboardMapper = new WhiteboardDtoMapper();
            var updatedWhiteboard = whiteboardMapper.ToEntity(updatedWhiteboardDto);
            var result = await whiteboardServices.UpdateWhiteboardAsync(
                whiteboardId,
                learningSpaceId,
                updatedWhiteboard
            );

            if (result)
            {
                var mapper = new WhiteboardDtoMapper();
                return mapper.ToDto(updatedWhiteboard);
            }
            else
            {
                return new { Error = "No se pudo actualizar el pizarra." };
            }
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
    /// Retrieves a paginated list of learning components.
    /// </summary>
    /// <param name="pageSize">The number of items to be included in a single page.</param>
    /// <param name="pageIndex">The index of the page to retrieve.</param>
    /// <param name="mapper">The mapper used for converting the projector entities to their corresponding DTO representations.</param>
    /// <param name="learningComponentServices">The service responsible for handling learning component operations.</param>
    /// <returns>A paginated list of learning components in DTO form, or an error describing the issue.</returns>
    [McpServerTool, Description("Obtiene una lista de componentes de aprendizaje según su página en la tabla")]
    public static async Task<object?> GetLearningComponentTool(
        [Description("Tamaño de la página")] int pageSize,
        [Description("Indice de la página")] int pageIndex,
        [FromServices] GlobalMapper mapper,
        [FromServices] ILearningComponentServices learningComponentServices)
    {
        try
        {
            var learningComponents = await learningComponentServices.GetLearningComponentAsync(pageSize, pageIndex);
            if (learningComponents == null)
            {
                return new { NotFound = "No se encontraron componentes de aprendizaje." };
            }
            var dtoList = learningComponents.Select(component => 
            {
                return mapper.ToDto(component);
            }).ToList();

            return dtoList; ;

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
    /// Retrieves all projectors registered in the system, including information such as Id, orientation, projected content, projection area, projector dimensions, and the projector's position in the space.
    /// </summary>
    /// <param name="mapper">The mapper used for converting the projector entities to their corresponding DTO representations.</param>
    /// <param name="projectorServices">The service responsible for managing projector operations and fetching projector data.</param>
    /// <returns>A list of projectors as DTO objects, or a list of error messages if retrieval fails.</returns>
    [McpServerTool, Description( "Obtiene todos los proyectores registrados en el sistema. Incluyendo información como Id, orientación, contenido proyectado, área proyectada, dimensiones del proyector y posición del proyector en el espacio.")]
    public static async Task<object?> GetAllProjectorsTool(
        [FromServices] GlobalMapper mapper,
        [FromServices] IProjectorServices projectorServices)
    {
        List<string> errors = [];
        var dtoList = new List<ProjectorDto?>();

        try
        {
            var projectors = await projectorServices.GetProjectorAsync();
            var projectorsList = projectors.ToList();

            if (projectorsList.Count == 0)
            {
                errors.Add("No hay proyectores registrados en el sistema.");
            }

            dtoList = projectorsList.Select(x => mapper.ToDto(x) as ProjectorDto).ToList();
        }
        catch (Exception ex)
        {
            errors.Add($"Ocurrió un error inesperado: {ex.Message}");
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return dtoList;
    }

    /// <summary>
    /// Creates a whiteboard in the learning space with the specified configurations.
    /// </summary>
    /// <param name="whiteboardServices">The service responsible for managing whiteboard operations.</param>
    /// <param name="mapper">The mapper used to convert DTOs to entity objects.</param>
    /// <param name="learningSpaceId">The identifier of the learning space where the whiteboard will be created.</param>
    /// <param name="color">The marker color of the whiteboard.</param>
    /// <param name="orientation">The orientation of the whiteboard. Possible values: North, South, West, or East.</param>
    /// <param name="x">The X coordinate used to position the whiteboard in the learning space.</param>
    /// <param name="y">The Y coordinate used to position the whiteboard in the learning space.</param>
    /// <param name="z">The Z coordinate used to position the whiteboard in the learning space.</param>
    /// <param name="width">The width of the whiteboard.</param>
    /// <param name="length">The length of the whiteboard.</param>
    /// <param name="height">The height of the whiteboard.</param>
    /// <returns>An object representing the created whiteboard, or null if the creation failed.</returns>
    [McpServerTool, Description("Crea una pizarra en el sistema.")]
    public static async Task<object?> PostWhiteboardTool(
        [FromServices] IWhiteboardServices whiteboardServices,
        [FromServices] GlobalMapper mapper,
        [Description("El identificador (id) del espacio de aprendizaje en el que se va a crear la pizarra.")]
        int learningSpaceId,
        [Description("El color del marcador de la pizarra.")]
        string color,
        [Description("La orientación de la pizarra. Posibles valores: North, South, West o East.")] string orientation,
        [Description("La coordenada en x usada para posicionar la pizarra en el espacio de aprendizaje.")] double x,
        [Description("La coordenada en y usada para posicionar la pizarra en el espacio de aprendizaje.")] double y,
        [Description("La coordenada en z usada para posicionar la pizarra en el espacio de aprendizaje.")] double z,
        [Description("El ancho de la pizarra. No debe sobreponerse a otros componentes en el espacio.")] double width,
        [Description("El largo de la pizarra. No debe sobreponerse a otros componentes en el espacio.")] double length,
        [Description("El alto de la pizarra. No debe sobreponerse a otros componentes en el espacio.")] double height
        )
    {
        List<string> errors = [];
        
        var position = new PositionDto(x, y, z);
        var dimensions = new DimensionsDto(width, length, height);
        var whiteboardDto = new WhiteboardNoIdDto(orientation, position, dimensions, color);
        
        try
        {
            // The dto mapper takes care of the Value Objects validation.
            var whiteboardEntity = mapper.ToEntity(whiteboardDto) as Whiteboard;
            var success = await whiteboardServices.AddWhiteboardAsync(learningSpaceId, whiteboardEntity!);
            if (success is false)
            {
                errors.Add("Ocurrió un error al crear la pizarra.");
            }
        }
        catch (ValidationException ve)
        {
            errors.Add(ve.Message);
        }
        catch (NotFoundException nfe)
        {
            errors.Add(nfe.Message);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return whiteboardDto;
    }
}
