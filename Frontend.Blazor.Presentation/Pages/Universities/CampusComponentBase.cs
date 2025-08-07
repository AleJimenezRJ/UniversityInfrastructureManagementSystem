using Microsoft.AspNetCore.Components;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Validators;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Pages.Universities
{
    /// <summary>
    /// Base component for campus-related Blazor pages.
    /// Handles validation and stores error messages for campus form fields.
    /// </summary>
    public abstract class CampusComponentBase : ComponentBase
    {
        /// <summary>
        /// Instance of the campus validator used for input validation.
        /// </summary>
        protected CampusValidator Validator { get; } = new CampusValidator();

        protected string? NameError { get; set; }
        protected string? UniversityNameError { get; set; }
        protected string? LocationError { get; set; }
        // If your UI also works with the University object directly and needs validation for its presence:
        // protected string? UniversityObjectError { get; set; }

        /// <summary>
        /// Validates the campus name and updates the corresponding error message.
        /// </summary>
        /// <param name="campusName">The name of the campus to validate.</param>
        protected void ValidateNameFormat(string? campusName)
        {
            var result = Validator.ValidateName(campusName);
            NameError = result.ErrorMessage;
        }

        /// <summary>
        /// Validates the university name associated with the campus and updates the corresponding error message.
        /// </summary>
        /// <param name="universityName">The university name to validate.</param>
        protected void ValidateUniversityNameFormat(string? universityName)
        {
            var result = Validator.ValidateUniversityName(universityName);
            UniversityNameError = result.ErrorMessage;
        }

        /// <summary>
        /// Validates the campus location and updates the corresponding error message.
        /// </summary>
        /// <param name="location">The location string to validate.</param>
        protected void ValidateLocationFormat(string? location)
        {
            var result = Validator.ValidateLocation(location);
            LocationError = result.ErrorMessage;
        }

        /// <summary>
        /// Determines whether there are any current validation errors for campus fields.
        /// </summary>
        /// <returns>True if any validation error messages are not empty; otherwise, false.</returns>
        protected bool HasValidationErrors()
        {
            return !string.IsNullOrWhiteSpace(NameError) ||
                   !string.IsNullOrWhiteSpace(UniversityNameError) ||
                   !string.IsNullOrWhiteSpace(LocationError);
        }

        /// <summary>
        /// Clears all validation error messages for campus fields.
        /// </summary>
        protected void ClearValidationErrors()
        {
            NameError = null;
            UniversityNameError = null;
            LocationError = null;
        }
    }
}