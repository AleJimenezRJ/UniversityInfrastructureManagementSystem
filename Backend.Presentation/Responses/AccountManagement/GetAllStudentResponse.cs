using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response returned when requesting the list of all registered students.
/// </summary>
public class GetAllStudentResponse
{
    /// <summary>
    /// Gets or sets the list of students retrieved from the system.
    /// </summary>
    public List<StudentDto> Students { get; set; } = new List<StudentDto>();
}