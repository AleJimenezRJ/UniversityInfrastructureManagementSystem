using System.Text.Json.Serialization;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents the request body for validating user account creation during signup.
/// </summary>
public sealed class PostValidateSignupRequest
{
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the given name of the user.
    /// </summary>
    [JsonPropertyName("givenName")]
    public string GivenName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the surname of the user.
    /// </summary>
    [JsonPropertyName("surname")]
    public string Surname { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the raw birth date of the user in string format (expected format: yyyy-MM-dd).
    /// </summary>
    [JsonPropertyName("extension_112354b1c5484559aad3e56ed715d247_Fechadenacimiento")]
    public string BirthDateRaw { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    [JsonPropertyName("extension_112354b1c5484559aad3e56ed715d247_UserName")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number of the user.
    /// </summary>
    [JsonPropertyName("extension_112354b1c5484559aad3e56ed715d247_Telefono")]
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identity number of the user, such as a national ID or passport number.
    /// </summary>
    [JsonPropertyName("extension_112354b1c5484559aad3e56ed715d247_Identificacion")]
    public string Identity { get; set; } = string.Empty;
}