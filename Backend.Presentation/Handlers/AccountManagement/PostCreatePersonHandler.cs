using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handles the creation of a new person.
/// </summary>
public static class PostCreatePersonHandler
{

    /// <summary>
    /// Handles the HTTP POST request to create a new person.
    /// </summary>
    /// <param name="personService"> The service used to manage person-related operations.</param>
    /// <param name="request"> The request containing the details of the person to be created.</param>
    /// <returns></returns>
      public static async Task<Results<Ok<PostCreatePersonResponse>, NotFound<string>,
        Conflict<string>,
        BadRequest<List<ValidationError>>>> HandleAsync(
        [FromServices] IPersonService personService,
        [FromBody] PostCreatePersonRequest request)
    {
        Person? personEntity = null;
        List<ValidationError> errorMessages = [];
        try
        {
            personEntity = PersonDtoMapper.ToEntity(request.Person);
        }
        catch (ValidationException exception)
        {
            errorMessages.Add(new ValidationError("Person", exception.Message));
        }
        if (errorMessages.Count > 0 || personEntity == null)
        {
            return TypedResults.BadRequest(errorMessages);
        }
        try
        {
            await personService.CreatePersonAsync(personEntity);
            return TypedResults.Ok(new PostCreatePersonResponse(request.Person, "Successful creation."));
        }
        catch (DuplicatedEntityException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
        catch (DomainException ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return TypedResults.Conflict(ex.Message);
        }
    }
}

