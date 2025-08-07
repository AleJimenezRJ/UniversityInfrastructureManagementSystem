using Microsoft.AspNetCore.Components;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Validators;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Pages.Universities
{
    /// <summary>
    /// Base component for university-related Blazor pages.
    /// Handles validation and stores error messages for university form fields.
    /// </summary>
    public abstract class UniversityComponentBase : ComponentBase
    {
        /// <summary>
        /// Instance of the university validator used for input validation.
        /// </summary>
        protected UniversityValidator Validator { get; } = new UniversityValidator();

        protected string? NameError { get; set; }
        protected string? CountryError { get; set; }

        /// <summary>
        /// Validates the university name and updates the corresponding error message.
        /// </summary>
        /// <param name="universityName">The name of the university to validate.</param>
        protected void ValidateNameFormat(string? universityName)
        {
            var result = Validator.ValidateName(universityName);
            NameError = result.ErrorMessage;
        }

        /// <summary>
        /// Validates the university country and updates the corresponding error message.
        /// </summary>
        /// <param name="country">The country string to validate.</param>
        protected void ValidateCountryFormat(string? country)
        {
            var result = Validator.ValidateCountry(country);
            CountryError = result.ErrorMessage;
        }

        /// <summary>
        /// Determines whether there are any current validation errors for university fields.
        /// </summary>
        /// <returns>True if any validation error messages are not empty; otherwise, false.</returns>
        protected bool HasValidationErrors()
        {
            return !string.IsNullOrWhiteSpace(NameError) ||
                   !string.IsNullOrWhiteSpace(CountryError);
        }

        /// <summary>
        /// Clears all validation error messages for university fields.
        /// </summary>
        protected void ClearValidationErrors()
        {
            NameError = null;
            CountryError = null;
        }
    }
}