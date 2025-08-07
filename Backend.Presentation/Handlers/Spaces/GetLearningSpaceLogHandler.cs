using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.Spaces;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.Spaces;

/// <summary>
/// Provides functionality to handle requests for retrieving learning space logs.
/// </summary>
/// <remarks>
/// This handler retrieves a list of learning space logs and returns them as a collection of DTOs.
/// If no logs are found, a message indicating the absence of logs is returned.
/// </remarks>
public static class GetLearningSpaceLogHandler
{
    /// <summary>
    /// Handles the retrieval of learning space logs and returns the results as a list of DTOs.
    /// </summary>
    /// <param name="learningSpaceLogServices">
    /// The service used to retrieve learning space logs. This parameter is provided via dependency injection.
    /// </param>
    /// <returns>
    /// A <see cref="Results{T1, T2}"/> object containing either:
    /// <list type="bullet">
    ///   <item><description>An <see cref="Ok{T}"/> result with a list of <see cref="LearningSpaceLogDto"/> objects representing the logs.</description></item>
    ///   <item><description>An <see cref="NotFound{T}"/> result with a string message indicating that no learning space logs are available.</description></item>
    /// </list>
    /// </returns>
    public static async Task<Results<Ok<GetLearningSpaceLogResponse>, NotFound<string>>> HandleAsync(
        [FromServices] ILearningSpaceLogServices learningSpaceLogServices)
    {
        var logs = await learningSpaceLogServices.ListLearningSpaceLogsAsync();

        if (logs == null || logs.Count == 0)
        {
            return TypedResults.NotFound("No se encontraron registros de cambios en los espacios de aprendizaje.");
        }

        var dtoList = LearningSpaceLogDtoMappers.ToDto(logs);
        return TypedResults.Ok(new GetLearningSpaceLogResponse(dtoList));
    }
}
