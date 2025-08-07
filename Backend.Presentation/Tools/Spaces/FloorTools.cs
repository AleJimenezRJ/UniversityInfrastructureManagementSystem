using Microsoft.AspNetCore.Mvc;
using ModelContextProtocol.Server;
using System.ComponentModel;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tools.Spaces;

/// <summary>
/// Provides tools for managing and retrieving information related to floors.
/// </summary>
[McpServerToolType]
public static class FloorTools
{
    /// <summary>
    /// Retrieves a list of floors associated with the specified building ID.
    /// </summary>
    /// <param name="buildingId">The unique identifier of the building for which the floors are to be retrieved. Must be a valid building ID
    /// format.</param>
    /// <param name="floorServices">The service used to query floor data.</param>
    /// <returns>An object containing the list of floors as DTOs if found, or an error object indicating the issue. Returns an
    /// error object if the building ID is invalid, no floors are found, or a conflict occurs.</returns>
    [McpServerTool, Description("Obtiene una lista de los pisos por el Id del edificio.\n\nNota: El resultado debe mostrarse en formato Markdown, en columna, como una lista.")]
    public static async Task<object?> GetFloorListTool(
        [Description("Id del edifico")] int buildingId,
        [FromServices] IFloorServices floorServices)
    {
        var errors = new List<object>();

        if (!Id.TryCreate(buildingId, out var validId))
            errors.Add(new { Parameter = "BuildingId", Message = "Formato de id de edificio inválido." });

        if (errors.Count > 0)
            return errors;

        try
        {
            var floors = await floorServices.GetFloorsListAsync(buildingId);

            if (floors == null || floors.Count == 0)
                return new { NotFound = $"No se encontraron pisos para el edificio con id {buildingId}." };

            var dtos = floors.Select(FloorDtoMapper.ToDto).ToList();

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
    /// Creates a new floor associated with a building.
    /// </summary>
    /// <param name="buildingId">Id of the building to associate the floor with.</param>
    /// <param name="floorServices">Floor service.</param>
    /// <returns>True if creation was successful, or an error object if not.</returns>
    [McpServerTool, Description("Crea un nuevo piso en un edificio.")]
    public static async Task<object?> CreateFloorTool(
        [Description("Id del edificio")] int buildingId,
        [FromServices] IFloorServices floorServices)
    {
        var errors = new List<object>();

        // Validate buildingId
        if (!Id.TryCreate(buildingId, out var validBuildingId))
            errors.Add(new { Parameter = "BuildingId", Message = "Formato de id de edificio inválido." });

        if (errors.Count > 0)
            return errors;

        try
        {
            var result = await floorServices.CreateFloorAsync(buildingId);

            if (!result)
            {
                return new
                {
                    Error = "CreationFailed",
                    Message = "No se pudo crear el piso."
                };
            }
            return new
            {
                Success = true,
                Message = "Piso creado exitosamente."
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
}
