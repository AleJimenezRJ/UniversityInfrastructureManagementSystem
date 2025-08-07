using ModelContextProtocol.Server;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;
namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tools.UniversityManagement;

/// <summary>
/// Provides tools for managing and retrieving university data.
/// </summary>
[McpServerToolType]
public static class UniversityTools
{
    /// <summary>
    /// Retrieves a list of Universities and returns their details as DTOs.
    /// </summary>
    [McpServerTool, Description("Obtener una lista de las universidades")]
    public static async Task<object?> GetUniversityListTool(
        [FromServices] IUniversityServices universityServices)
    {
        try
        {
            // Create List of Universities
            var university = await universityServices.ListUniversityAsync();

            if (university == null)
                return new { NotFound = $"No se encontró la universidad/es {university}." };

            var dtos = university
                .Select(UniversityDtoMappers.ToDto)
                .ToList();

            if (dtos.Count == 0)
                return new { NotFound = "No se han encontrado universidades." };

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
    /// Gets an University by its name.
    /// </summary>
    [McpServerTool, Description("Obtiene una universidad específica por su Id.")]
    public static async Task<object?> GetUniversityByNameTool(
        [Description("Nombre de la universidad")] string universityName,
        [FromServices] IUniversityServices universityServices)
    {
        var errors = new List<object>();
        
        if (string.IsNullOrWhiteSpace(universityName))
            errors.Add(new { Parameter = "universityName", Message = "Universidad tiene que ser un valor válido" });

        if (errors.Count > 0)
            return errors;

        try
        {
            // Get Uni by name
            var university = await universityServices.GetByNameAsync(universityName);

            if (university == null)
                return new { NotFound = $"No se encontró la universidad con el nombre: {universityName}." };

            // Create Uni DTO
            var dto = UniversityDtoMappers.ToDto(university);

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
    /// Creates a new university in the system
    /// </summary>
    [McpServerTool, Description("Crea una nueva universidad en el sistema")]
    public static async Task<object?> CreateUniversityTool(
        [FromServices] IUniversityServices universityServices,
        [Description("Nombre de la universidad")] string name,
        [Description("País de la universidad")] string country)
    {
        try
        {
            // create University DTO
            var universityDto = new UniversityDto(name, country);

            // Map to a domain entity
            var universityEntity = UniversityDtoMappers.ToEntity(universityDto);

            // Add university
            var success = await universityServices.AddUniversityAsync(universityEntity);

            // Check if the university was successfully created

            if (!success)
            {
                return new
                {
                    Error = "CreationFailed",
                    Message = "No se pudo crear la universidad."
                };
            }

            return new
            {
                Success = true,
                Message = "Universidad creada exitosamente.",
                University = UniversityDtoMappers.ToDto(universityEntity)
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
        catch (ValidationException ex)
        {
            return new
            {
                Error = "ValidationError",
                Message = "Error de validación en los datos de la universidad.",
                Details = ex.Errors.Select(e => new { Field = e.Parameter, Error = e.Message })
            };
        }
        catch (Exception ex)
        {
            return new
            {
                Error = "UnexpectedError",
                Message = "Ocurrió un error inesperado al crear la universidad.",
                Details = ex.Message
            };
        }
    }
}