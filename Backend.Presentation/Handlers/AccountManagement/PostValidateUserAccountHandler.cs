using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Handlers.AccountManagement;

/// <summary>
/// Handles the validation of user account creation during signup.
/// </summary>
public static class PostValidateSignupHandler
{
    /// <summary>
    /// Handles the validation of user account creation requests.
    /// </summary>
    /// <param name="userWithPersonService"> The service to handle user and person creation.</param>
    /// <param name="context"> The HTTP context containing the request data.</param>
    /// <returns> An <see cref="IResult"/> indicating the outcome of the validation and creation process.</returns>
    public static async Task<IResult> HandleAsync(
    [FromBody] PostValidateSignupRequest request,
    [FromServices] IUserWithPersonService userWithPersonService,
    HttpContext context)
    {
        if (request is null)
        {
            return CreateErrorResponse(
                status: "400",
                message: "El cuerpo del request es obligatorio.");
        }

        try
        {
            // Validate and parse all fields
            var birthDate = ParseBirthDate(request.BirthDateRaw);
            var identityNumber = ParseIdentityNumber(request.Identity);
            var phoneNumber = ParsePhoneNumber(request.Phone);
            var userName = ParseUserName(request.Username);

            var createUserDto = new CreateUserWithPersonDto(
                request.Username,
                request.Email,
                request.GivenName,
                request.Surname,
                request.Phone,
                birthDate,
                request.Identity,
                new List<string>());

            var userWithPersonEntity = UserWithPersonDtoMapper.ToEntity(createUserDto);
            var creationSucceeded = await userWithPersonService.CreateUserWithPersonAsync(userWithPersonEntity);

            return creationSucceeded
                ? CreateSuccessResponse()
                : CreateErrorResponse("400","Ya existe un usuario con ese nombre, correo o número de identificación.");
        }
        catch (ValidationException ex)
        {
            return CreateErrorResponse("400", ex.Message);
        }
        catch (DomainException ex)
        {
            return CreateDomainErrorResponse(ex);
        }
    }

    /// <summary>
    /// Parses the birth date from a string in the format "yyyy-MM-dd".
    /// </summary>
    /// <param name="birthDateRaw"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    private static DateOnly ParseBirthDate(string birthDateRaw)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(birthDateRaw))
            {
                throw new ValidationException("La fecha de nacimiento es obligatoria.");
            }
            var cleaned = birthDateRaw.Trim();
            return DateOnly.ParseExact(cleaned, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
        catch
        {
            throw new ValidationException("La fecha de nacimiento debe tener el formato válido (ej: yyyy-MM-dd).");
        }
    }

    /// <summary>
    /// Parses the identity number from a string, ensuring it has a valid format.
    /// </summary>
    /// <param name="identity"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    private static IdentityNumber ParseIdentityNumber(string identity)
    {
        try
        {
            return IdentityNumber.Create(identity);
        }
        catch
        {
            throw new ValidationException("El número de identificación debe de tener el formato válido (ej: 1-1111-1111).");
        }
    }

    /// <summary>
    /// Parses the phone number from a string, ensuring it has a valid format.
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    private static Phone ParsePhoneNumber(string phone)
    {
        try
        {
            return Phone.Create(phone);
        }
        catch
        {
            throw new ValidationException("El número de telefono debe de tener el formato válido (ej: 1111-1111).");
        }
    }

    /// <summary>
    /// Parses the username from a string, ensuring it has a valid format and length.
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    private static UserName ParseUserName(string username)
    {
        try
        {
            return UserName.Create(username);
        }
        catch
        {
            throw new ValidationException("El nombre de usuario debe de tener el formato válido (no más de 50 caracteres).");
        }
    }

    /// <summary>
    /// Creates an error response with the specified status and message.
    /// </summary>
    /// <param name="status"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    private static IResult CreateErrorResponse(string status, string message)
    {
        return Results.Json(new PostValidateSignupResponse(
            Action: "ShowBlockPage",
            status: "400",
            userMessage: message
        ));
    }

    /// <summary>
    /// Creates a success response indicating that the user account validation was successful and the user can continue.
    /// </summary>
    /// <returns></returns>
    private static IResult CreateSuccessResponse()
    {
        return Results.Json(new PostValidateSignupResponse(
            Action: "Continue",
            status: "200"
        ));
    }

    /// <summary>
    /// Creates a domain error response with a user-friendly message based on the exception.
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    private static IResult CreateDomainErrorResponse(DomainException ex)
    {
        var friendlyMessage = ex.Message.Contains("ValueObjects.Email") ||
                             ex.Message.Contains("ValueObjects.IdentityNumber")
            ? "Ya existe una persona registrada con este correo o número de identificación."
            : ex.Message;

        return CreateErrorResponse(
            status: "409",
            message: friendlyMessage);
    }
}
