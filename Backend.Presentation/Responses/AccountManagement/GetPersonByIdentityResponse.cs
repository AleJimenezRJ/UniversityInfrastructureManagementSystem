using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response returned when retrieving a person by their identity number.
/// </summary>
public class GetPersonByIdentityResponse
{
    /// <summary>
    /// Gets the data transfer object (DTO) containing the person's details.
    /// </summary>
    public PersonDto Person { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPersonByIdentityResponse"/> class.
    /// </summary>
    /// <param name="person">The DTO containing the person's details.</param>
    public GetPersonByIdentityResponse(PersonDto person)
    {
        Person = person;
    }
}
