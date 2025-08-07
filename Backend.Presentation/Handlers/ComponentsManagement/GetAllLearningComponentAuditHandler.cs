using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.ComponentsManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.ComponentsManagement;


/// <summary>
/// Handler for retrieving all learning component audit records.
/// </summary>
public static class GetAllLearningComponentAuditHandler
{
    /// <summary>
    /// Asynchronously handles the retrieval of all learning component audit records.
    /// </summary>
    /// <param name="learningComponentAuditService">The service used to access learning component audit data.</param>
    /// <returns>
    /// An <see cref="Ok{T}"/> result containing a list of <see cref="LearningComponentAuditDto"/> if records exist,
    /// or an <see cref="Ok{T}"/> result with a message if no records are found.
    /// </returns>
    public static async Task<Results<Ok<List<LearningComponentAuditDto>>, Ok<string>>> HandleAsync(
        [FromServices] ILearningComponentAuditServices learningComponentAuditService)
    {
        var learningComponentAudits = await learningComponentAuditService.ListLearningComponentAuditAsync();

        if (learningComponentAudits == null || learningComponentAudits.Count == 0)
        {
            return TypedResults.Ok("There are no registered learning component audits.");
        }

        var dtoList = LearningComponentAuditDtoMapper.ToDtoList(learningComponentAudits);
        return TypedResults.Ok(dtoList);
    }
}

