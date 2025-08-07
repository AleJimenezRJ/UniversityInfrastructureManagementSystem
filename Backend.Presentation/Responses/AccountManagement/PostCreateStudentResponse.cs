using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response for creating a new Student.
/// </summary>
/// <param name="Student"> The details of the created Student.</param>
/// <param name="Message"> A message indicating the result of the operation.</param>
public record class PostCreateStudentResponse(CreateStudentDto Student, string Message);