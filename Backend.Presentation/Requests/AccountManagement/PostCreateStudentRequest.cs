using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents a request to create a new Student.
/// </summary>
/// <param name="Student"> The details of the Student to be created.</param>
public record class PostCreateStudentRequest(CreateStudentDto Student);
