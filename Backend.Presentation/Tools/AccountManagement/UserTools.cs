using ModelContextProtocol.Server;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tools.AccountManagement;

/// <summary>
/// Tools related to user management in the Theme Park application.
/// </summary>
[McpServerToolType]
public static class UserTools
{
    /// <summary>
    /// Obtains recent changes made by a user based on their identity number within the last X weeks.
    /// </summary>
    /// <param name="identityNumber">The identity number of the user.</param>
    /// <param name="weeks">The number of weeks to look back for changes.</param>
    /// <param name="userAuditService">The service to access user audit logs.</param>
    /// <returns>
    /// Returns a list of user audit logs filtered by the specified identity number and within the specified time frame.
    /// </returns>
    [McpServerTool, Description("Obtiene los cambios recientes de un usuario por su número de identificación en las últimas X semanas.")]
    public static async Task<object?> GetUserRecentChangesByIdNumberTool(
        [Description("Número de identificación del usuario (formato 1-1234-5678)")] string identityNumber,
        [Description("Cantidad de semanas hacia atrás")] int weeks,
        [FromServices] IUserAuditService userAuditService)
    {
        var errors = new List<object>();
        IdentityNumber? validIdentityNumber = null;

        if (string.IsNullOrWhiteSpace(identityNumber))
        {
            errors.Add(new
            {
                Parameter = "IdentityNumber",
                Message = "El número de identificación es requerido."
            });
        }
        else if (!IdentityNumber.TryCreate(identityNumber, out validIdentityNumber))
        {
            errors.Add(new
            {
                Parameter = "IdentityNumber",
                Message = "El formato del número de identificación es inválido. Debe ser como 1-1234-5678."
            });
        }

        if (weeks <= 0)
        {
            errors.Add(new
            {
                Parameter = "Weeks",
                Message = "La cantidad de semanas debe ser mayor a cero."
            });
        }

        if (errors.Count > 0)
            return errors;

        try
        {
            var allAudits = await userAuditService.ListUserAuditAsync();
            var cutoffDate = DateTime.UtcNow.AddDays(-7 * weeks);

            var filteredAudits = allAudits
                .Where(a => a.IdentityNumber == validIdentityNumber!.Value && a.ModifiedAt >= cutoffDate)
                .OrderByDescending(a => a.ModifiedAt)
                .Select(UserAuditDtoMapper.ToDto)
                .ToList();

            if (filteredAudits.Count == 0)
            {
                return new
                {
                    Message = $"No se encontraron cambios para el usuario {identityNumber} en las últimas {weeks} semanas."
                };
            }

            return filteredAudits;
        }
        catch (Exception ex)
        {
            return new { Error = $"Error inesperado: {ex.Message}" };
        }
    }
}
