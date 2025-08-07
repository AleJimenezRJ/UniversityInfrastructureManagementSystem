using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Requests.AccountManagement;

/// <summary>
/// Represents a request to create a new Staff.
/// </summary>
/// <param name="Staff"> The details of the Staff to be created.</param>
public record class PostCreateStaffRequest(CreateStaffDto Staff);
