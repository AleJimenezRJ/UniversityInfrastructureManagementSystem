using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.UniversityManagement;

/// <summary>
/// Handles the HTTP GET request to retrieve all universities.
/// </summary>
public static class GetUniversityHandler
{
    public static async Task<Results<Ok<GetUniversityResponse>, ProblemHttpResult>> HandleAsync(
        [FromServices] IUniversityServices UniversityServices)
    {
        try
        {
            var university = await UniversityServices.ListUniversityAsync();

            var universityDtos = university
                .Select(UniversityDtoMappers.ToDto)
                .ToList();

            var response = new GetUniversityResponse(universityDtos);

            return TypedResults.Ok(response);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem($"An error occurred while retrieving universities: {ex.Message}");
        }
    }

}