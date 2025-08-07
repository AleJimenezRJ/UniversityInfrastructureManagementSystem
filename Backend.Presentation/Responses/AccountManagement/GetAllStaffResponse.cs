using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Responses.AccountManagement;

/// <summary>
/// Represents the response returned when requesting the list of all registered staff.
/// </summary>
public class GetAllStaffResponse
{
    /// <summary>
    /// Gets or sets the list of staff members retrieved from the system.
    /// </summary>
    public List<StaffDto> Staff { get; set; } = new List<StaffDto>();
}