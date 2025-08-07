using Microsoft.AspNetCore.Components;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Validators;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Pages.Universities
{
    /// <summary>
    /// Base component for area-related Blazor pages.
    /// Handles validation and stores error messages for area form fields.
    /// </summary>
    public abstract class AreaComponentBase : ComponentBase
    {
        /// <summary>
        /// Instance of the area validator used for input validation.
        /// </summary>
        protected AreaValidator Validator { get; } = new AreaValidator();

        protected string? NameError { get; set; }
        protected string? CampusNameError { get; set; }

        /// <summary>
        /// Validates the area name and updates the corresponding error message.
        /// </summary>
        /// <param name="areaName">The name of the area to validate.</param>
        protected void ValidateNameFormat(string? areaName)
        {
            var result = Validator.ValidateName(areaName);
            NameError = result.ErrorMessage;
        }

        /// <summary>
        /// Validates the campus name associated with the area and updates the corresponding error message.
        /// </summary>
        /// <param name="campusName">The campus name to validate.</param>
        protected void ValidateCampusNameFormat(string? campusName)
        {
            var result = Validator.ValidateCampusName(campusName);
            CampusNameError = result.ErrorMessage;
        }

        /// <summary>
        /// Determines whether there are any current validation errors for area fields.
        /// </summary>
        /// <returns>True if any validation error messages are not empty; otherwise, false.</returns>
        protected bool HasValidationErrors()
        {
            return !string.IsNullOrWhiteSpace(NameError) ||
                   !string.IsNullOrWhiteSpace(CampusNameError);
        }

        /// <summary>
        /// Clears all validation error messages for area fields.
        /// </summary>
        protected void ClearValidationErrors()
        {
            NameError = null;
            CampusNameError = null;
        }
    }
}