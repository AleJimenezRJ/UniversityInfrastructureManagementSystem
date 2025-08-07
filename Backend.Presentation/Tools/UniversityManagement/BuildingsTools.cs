using ModelContextProtocol.Server;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tools.UniversityManagement;

/// <summary>
/// Provides tools for managing and retrieving building-related data.
/// </summary>
/// <remarks>This class contains methods for interacting with building services, such as retrieving a list of
/// buildings. It is designed to be used in server-side applications where building data needs to be accessed or
/// manipulated.</remarks>
[McpServerToolType]
public static class BuildingsTools
{
    /// <summary>
    /// Retrieves a list of buildings and returns their details as DTOs.
    /// </summary>
    /// <param name="buildingsServices">The service used to fetch building data. This parameter is provided via dependency injection.</param>
    /// <returns>A collection of building DTOs if buildings are found; otherwise, an object indicating that no buildings were
    /// found. If a concurrency conflict occurs, an object containing the conflict message is returned.</returns>
    [McpServerTool, Description("Obtiene una lista de los edificios")]
    public static async Task<object?> GetBuildingsListTool(
        [FromServices] IBuildingsServices buildingsServices)
    {
        try
        {
            var buildings = await buildingsServices.ListBuildingAsync();

            if (buildings == null)
                return new { NotFound = "No se encontraron edificios." };

            var dtos = buildings
                .Select(BuildingDtoMappers.ToDto)
                .ToList();

            if (dtos.Count == 0)
                return new { NotFound = "No se encontraron edificios." };

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
    /// Creates a new building in the system using the MCP server tool.
    /// </summary>
    /// <param name="buildingsServices">The service used to manage building data. This parameter is provided via dependency injection.</param>
    /// <param name="areaServices">The service used to validate and retrieve area data. This parameter is provided via dependency injection.</param>
    /// <param name="name">The name of the building to create.</param>
    /// <param name="x">The X coordinate of the building.</param>
    /// <param name="y">The Y coordinate of the building.</param>
    /// <param name="z">The Z coordinate of the building.</param>
    /// <param name="width">The width of the building.</param>
    /// <param name="length">The length of the building.</param>
    /// <param name="height">The height of the building.</param>
    /// <param name="color">The color of the building.</param>
    /// <param name="areaName">The name of the area where the building will be located.</param>
    /// <returns>A result object indicating success or failure, with the created building data if successful.</returns>
    [McpServerTool, Description("Crea un nuevo edificio en el sistema")]
    public static async Task<object?> CreateBuildingTool(
        [FromServices] IBuildingsServices buildingsServices,
        [FromServices] IAreaServices areaServices,
        string name,
        double x,
        double y,
        double z,
        double width,
        double length,
        double height,
        string color,
        string areaName)
    {
        try
        {
            // Validate that the area exists
            var areaEntity = await areaServices.GetByNameAsync(areaName);
            if (areaEntity == null)
            {
                return new
                {
                    Error = "ValidationError",
                    Message = $"El área '{areaName}' no existe en el sistema."
                };
            }

            // Create the DTO for the new building
            var buildingDto = new AddBuildingDto(
                name,
                x,
                y,
                z,
                width,
                length,
                height,
                color,
                new AddBuildingAreaDto(areaName)
            );

            // Map to entity
            var buildingEntity = BuildingDtoMappers.ToEntity(buildingDto, areaEntity);

            // Add the building
            var success = await buildingsServices.AddBuildingAsync(buildingEntity);

            if (!success)
            {
                return new
                {
                    Error = "CreationFailed",
                    Message = "No se pudo crear el edificio."
                };
            }

            return new
            {
                Success = true,
                Message = "Edificio creado exitosamente.",
                Building = BuildingDtoMappers.ToDto(buildingEntity)
            };
        }
        catch (ValidationException ex)
        {
            return new
            {
                Error = "ValidationError",
                Message = "Error de validación en los datos del edificio.",
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
    /// Retrieves a building by its unique identifier.
    /// </summary>
    /// <param name="buildingId">The ID of the building to retrieve.</param>
    /// <param name="buildingsServices">The service used to retrieve building data. Injected via dependency injection.</param>
    /// <returns>
    /// The building DTO if found; otherwise, an object with a not found or conflict message.
    /// </returns>
    [McpServerTool, Description("Obtiene un edificio específico por su Id.")]
    public static async Task<object?> GetBuildingByIdTool(
        [Description("Id del edificio")] int buildingId,
        [FromServices] IBuildingsServices buildingsServices)
    {
        var errors = new List<object>();

        if (buildingId <= 0)
            errors.Add(new { Parameter = "BuildingId", Message = "El Id debe ser un número entero positivo." });

        if (errors.Count > 0)
            return errors;

        try
        {
            var building = await buildingsServices.DisplayBuildingAsync(buildingId);

            if (building == null)
                return new { NotFound = $"No se encontró un edificio con id {buildingId}." };

            var dto = BuildingDtoMappers.ToDto(building);

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
    /// Updates an existing building in the system using the MCP server tool.
    /// </summary>
    /// <param name="buildingsServices">The service used to manage building data. This parameter is provided via dependency injection.</param>
    /// <param name="areaServices">The service used to validate and retrieve area data. This parameter is provided via dependency injection.</param>
    /// <param name="buildingId">The ID of the building to update.</param>
    /// <param name="name">The new name of the building.</param>
    /// <param name="x">The new X coordinate of the building.</param>
    /// <param name="y">The new Y coordinate of the building.</param>
    /// <param name="z">The new Z coordinate of the building.</param>
    /// <param name="width">The new width of the building.</param>
    /// <param name="length">The new length of the building.</param>
    /// <param name="height">The new height of the building.</param>
    /// <param name="color">The new color of the building.</param>
    /// <param name="areaName">The name of the area where the building will be located.</param>
    /// <returns>A result object indicating success or failure, with the updated building data if successful.</returns>
    [McpServerTool, Description("Actualiza un edificio existente en el sistema")]
    public static async Task<object?> UpdateBuildingTool(
        [FromServices] IBuildingsServices buildingsServices,
        [FromServices] IAreaServices areaServices,
        int buildingId,
        string name,
        double x,
        double y,
        double z,
        double width,
        double length,
        double height,
        string color,
        string areaName)
    {
        try
        {
            // Validate that the area exists
            var areaEntity = await areaServices.GetByNameAsync(areaName);
            if (areaEntity == null)
            {
                return new
                {
                    Error = "ValidationError",
                    Message = $"El área '{areaName}' no existe en el sistema."
                };
            }

            // Create the DTO for the updated building
            var buildingDto = new AddBuildingDto(
                name,
                x,
                y,
                z,
                width,
                length,
                height,
                color,
                new AddBuildingAreaDto(areaName)
            );

            // Map to entity
            var buildingEntity = BuildingDtoMappers.ToEntity(buildingDto, areaEntity);

            // Update the building
            var success = await buildingsServices.UpdateBuildingAsync(buildingEntity, buildingId);

            if (!success)
            {
                return new
                {
                    Error = "UpdateFailed",
                    Message = "No se pudo actualizar el edificio."
                };
            }

            return new
            {
                Success = true,
                Message = "Edificio actualizado exitosamente.",
                Building = BuildingDtoMappers.ToDto(buildingEntity)
            };
        }
        catch (ValidationException ex)
        {
            return new
            {
                Error = "ValidationError",
                Message = "Error de validación en los datos del edificio.",
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
                Message = "Ocurrió un error inesperado al actualizar el edificio.",
                Details = ex.Message
            };
        }
    }

    /// <summary>
    /// Deletes an existing building from the system using the MCP server tool.
    /// </summary>
    /// <param name="buildingsServices">The service used to manage building data. This parameter is provided via dependency injection.</param>
    /// <param name="buildingId">The ID of the building to delete.</param>
    /// <returns>A result object indicating success or failure of the deletion operation.</returns>
    [McpServerTool, Description("Elimina un edificio existente del sistema")]
    public static async Task<object?> DeleteBuildingTool(
        [FromServices] IBuildingsServices buildingsServices,
        [Description("Id del edificio a eliminar")] int buildingId)
    {
        var errors = new List<object>();

        if (buildingId <= 0)
            errors.Add(new { Parameter = "BuildingId", Message = "El Id debe ser un número entero positivo." });

        if (errors.Count > 0)
            return errors;

        try
        {
            var success = await buildingsServices.DeleteBuildingAsync(buildingId);

            if (!success)
            {
                return new
                {
                    Error = "NotFound",
                    Message = $"No se pudo eliminar el edificio con id {buildingId}. Verifique que el id sea correcto."
                };
            }

            return new
            {
                Success = true,
                Message = $"El edificio con ID {buildingId} ha sido eliminado del sistema exitosamente."
            };
        }
        catch (NotFoundException ex)
        {
            return new
            {
                Error = "NotFound",
                Message = ex.Message
            };
        }
        catch (ConcurrencyConflictException ex)
        {
            return new
            {
                Error = "ConcurrencyConflict",
                Message = $"El edificio con ID {buildingId} no puede ser eliminado debido a un conflicto de concurrencia: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            return new
            {
                Error = "UnexpectedError",
                Message = "Ocurrió un error inesperado al eliminar el edificio.",
                Details = ex.Message
            };
        }
    }
}