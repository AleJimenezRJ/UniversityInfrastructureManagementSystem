using ModelContextProtocol.Server;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tools.UniversityManagement;

[McpServerToolType]
public static class AreaTools
{
    /// <summary>
    /// Retrieves a list of areas and returns their details as DTOs.
    /// </summary>
    /// <param name="areaServices">The service used to fetch area data. This parameter is provided via dependency injection.</param>
    /// <returns>A collection of area DTOs if areas are found; otherwise, an object indicating that no areas were found. If a concurrency conflict occurs, an object containing the conflict message is returned.</returns>
    [McpServerTool, Description("Obtiene una lista de las áreas")]
    public static async Task<object?> GetAreasListTool(
        [FromServices] IAreaServices areaServices)
    {
        try
        {
            var areas = await areaServices.ListAreaAsync();
            if (areas == null)
                return new { NotFound = "No se encontraron áreas." };
            var dtos = areas
                .Select(AreaDtoMappers.ToDto)
                .ToList();
            if (dtos.Count == 0)
                return new { NotFound = "No se encontraron áreas." };
            return dtos;
        }
        catch (NotFoundException ex)
        {
            return new { NotFound = ex.Message };
        }
    }

    /// <summary>
    /// Adds a new area to the system using the MCP server tool.
    /// </summary>
    /// <param name="campusServices"></param>
    /// <param name="areaServices"></param>
    /// <param name="name"></param>
    /// <param name="campusName"></param>
    /// <returns></returns>
    /// 
    [McpServerTool, Description("agrega un área al sistema")]
    public static async Task<object?> AddAreasTool(
        [FromServices] ICampusServices campusServices,
        [FromServices] IAreaServices areaServices,
        [Description("Este es el nombre del área que el usuario desea agregar")] string name,
        [Description("Este es el nombre del campus ya existente al que pertenece el área")] string campusName)
    {
        try
        {
            var areaDto = new AddAreaDto
            (
                name,
                new AddAreaCampusDto
                (
                    campusName
                )
            );

            var campus = await campusServices.GetByNameAsync(campusName);
            if (campus == null)
            {
                return new
                {
                    Error = "ValidationError",
                    Message = $"El campus '{campusName}' no existe en el sistema."
                };
            }
            var area = AreaDtoMappers.ToEntity(areaDto, campus);
            var result = await areaServices.AddAreaAsync(area);
            if (result)
            {
                return AreaDtoMappers.ToDto(area);
            }
            else
            {
                return new { Error = "Error al convertir el área en entidad." };
            }
        }
        catch (NotFoundException ex)
        {
            return new { NotFound = ex.Message };
        }
        catch (DuplicatedEntityException ex)
        {
            return new { Duplicated = ex.Message };
        }
        catch(ValidationException ex)
        {
            return new { ValidationError = ex.Message };
        }
    }

    /// <summary>
    /// Retrieves an area by its name using the MCP server tool.
    /// </summary>
    /// <param name="areaServices"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    [McpServerTool, Description("Obtiene un área por su nombre")]
    public static async Task<object?> GetAreaByNameTool(
        [FromServices] IAreaServices areaServices,
        [Description("Nombre del área que se desea obtener")] string name)
    {
        try
        {
            var area = await areaServices.GetByNameAsync(name);
            if (area == null)
            {
                return new { NotFound = $"No se encontró un área con el nombre '{name}'." };
            }
            return AreaDtoMappers.ToDto(area);
        }
        catch (NotFoundException ex)
        {
            return new { NotFound = ex.Message };
        }
    }
}