using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.AccountManagement;
namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;

/// <summary>
/// Context for user navigation within the application.
/// </summary>
public class UserNavigationContext
{
    /// <summary>
    /// Gets or sets the ID of the selected user.
    /// </summary>
    public int? SelectedUserId { get; set; }
}
