using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response for creating a new Staff.
/// </summary>
/// <param name="Staff"> The details of the created Staff.</param>
/// <param name="Message"> A message indicating the result of the operation.</param>
public record class PostCreateStaffResponse(CreateStaffDto Staff, string Message);