using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handler for retrieving all registered students.
/// </summary>
public static class GetAllStudentHandler
{
    // <summary>
    /// Handles the request to retrieve all registered students.
    /// </summary>
    /// <param name="studentService">The service used to retrieve student data.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains:
    /// An <see cref="Ok{T}"/> result with a <see cref="GetAllStudentResponse"/> object, containing a list of students if any exist.
    /// An <see cref="Ok{T}"/> result with a message if no students are registered.
    /// </returns>
    public static async Task<Results<
        Ok<GetAllStudentResponse>,
        NotFound<string>,
        Conflict<string>>> HandleAsync(
        [FromServices] IStudentService studentService)
    {
        try
        {
            var students = await studentService.ListStudentsAsync();

            if (students == null || students.Count == 0)
            {
                return TypedResults.NotFound("There are no registered students.");
            }

            var dtoList = StudentDtoMapper.ToDtoList(students);

            var response = new GetAllStudentResponse
            {
                Students = dtoList
            };

            return TypedResults.Ok(response);
        }
        catch (DomainException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}
