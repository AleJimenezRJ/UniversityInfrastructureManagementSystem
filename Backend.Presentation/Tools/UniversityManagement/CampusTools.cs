using ModelContextProtocol.Server;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tools.UniversityManagement;


[McpServerToolType]
public static class CampusTools
{
    /// <summary>
    /// Retrieves a list of campus entities from the <see cref="ICampusServices"/> service.
    /// Handles possible errors such as not found or concurrency conflicts.
    /// </summary>
    /// <param name="campusServices">The injected service used to retrieve campus data.</param>
    /// <returns>
    /// A list of campus DTOs if available; otherwise, an object indicating a not found or conflict error.
    /// </returns>
    [McpServerTool, Description("Obtiene una lista de los campus")]
    public static async Task<object?> GetCampusListTool(
        [FromServices] ICampusServices campusServices)
    {
        try
        {
            var campus = await campusServices.ListCampusAsync();

            if (campus == null)
                return new { NotFound = "No se encontraron campus." };

            var dtos = campus.Select(CampusDtoMappers.ToDto).ToList();

            if (dtos.Count == 0)
                return new { NotFound = "No se encontraron campus." };

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
    /// Creates a new campus in the system using the provided campus and university information.
    /// </summary>
    /// <param name="campusServices">Service used to handle campus operations.</param>
    /// <param name="universityServices">Service used to validate and retrieve the university entity.</param>
    /// <param name="name">The name of the campus.</param>
    /// <param name="location">The location of the campus.</param>
    /// <param name="universityName">The name of the university to which the campus belongs.</param>
    /// <returns>
    /// A success response with the created campus DTO if successful;
    /// otherwise, an error object detailing the reason for failure (validation, duplication, or system error).
    /// </returns>
    [McpServerTool, Description("Crea un nuevo campus en el sistema")]
    public static async Task<object?> CreateCampusTool(
        [FromServices] ICampusServices campusServices,
        [FromServices] IUniversityServices universityServices,
        [Description("Nombre del campus")] string name,
        [Description("Ubicación del campus")] string location,
        [Description("País de la universidad")] string universityName)
    {
        try
        {
            var universityEntity = await universityServices.GetByNameAsync(universityName);

            // Validate that the university exists
            if (universityEntity == null)
            {
                return new
                {
                    Error = "ValidationError",
                    Message = $"La universidad '{universityName}' no existe en el sistema."
                };
            }

            // Create the DTO for the new campus
            var CampusDto = new AddCampusDto(
                name,
                location,
                new AddCampusUniversityDto(universityName)
            );

            // Map to entity
            var campusEntity = CampusDtoMappers.ToEntity(CampusDto, universityEntity);

            // Add the campus
            var success = await campusServices.AddCampusAsync(campusEntity);

            if (!success)
            {
                return new
                {
                    Error = "CreationFailed",
                    Message = "No se pudo crear el campus."
                };
            }

            return new
            {
                Success = true,
                Message = "Campus creado exitosamente.",
                Campus = CampusDtoMappers.ToDto(campusEntity)
            };
        }
        catch (ValidationException ex)
        {
            return new
            {
                Error = "ValidationError",
                Message = "Error de validación en los datos del campus.",
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
                Message = "Ocurrió un error inesperado al crear el edificio.",
                Details = ex.Message
            };
        }
    }


    /// <summary>
    /// Retrieves a specific campus by its name.
    /// </summary>
    /// <param name="CampusName">The name of the campus to retrieve.</param>
    /// <param name="campusServices">Service used to access campus data.</param>
    /// <returns>
    /// The corresponding campus DTO if found;
    /// otherwise, an object indicating a not found or conflict error.
    /// </returns>
    [McpServerTool, Description("Obtiene un campus específico por su nombre.")]
    public static async Task<object?> GetCampusByNameTool(
        [Description("nombre del edificio")] string CampusName,
        [FromServices] ICampusServices campusServices)
    {
        try
        {
            var campus = await campusServices.GetByNameAsync(CampusName);

            if (campus == null)
                return new { NotFound = $"No se encontró un campus con nombre {CampusName}." };

            var dto = CampusDtoMappers.ToDto(campus);

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
}