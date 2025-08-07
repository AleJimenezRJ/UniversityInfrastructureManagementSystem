using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Validators
{
    /// <summary>
    /// Provides validation logic for university-related input fields.
    /// </summary>
    public class UniversityValidator
    {
        /// <summary>
        /// Validates the university name string.
        /// </summary>
        /// <param name="universityName">The string name of the university.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the name is valid.</returns>
        public ValidationResult ValidateName(string? universityName)
        {
            if (string.IsNullOrWhiteSpace(universityName?.Trim()))
            {
                return new ValidationResult(false, "El nombre de la universidad es obligatorio.");
            }

            if (universityName.Trim().Length > 100)
            {
                return new ValidationResult(false, "El nombre de la universidad debe tener como máximo 100 caracteres.");
            }

            return new ValidationResult(true, null);
        }

        /// <summary>
        /// Validates the university country string.
        /// </summary>
        /// <param name="country">The string name of the country where the university is located.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the country is valid.</returns>
        public ValidationResult ValidateCountry(string? country)
        {
            if (string.IsNullOrWhiteSpace(country?.Trim()))
            {
                return new ValidationResult(false, "El país de la universidad es obligatorio.");
            }

            if (country.Trim().Length > 100)
            {
                return new ValidationResult(false, "El país debe tener como máximo 100 caracteres.");
            }

            return new ValidationResult(true, null);
        }

        /// <summary>
        /// Validates all fields of the university form at once.
        /// </summary>
        /// <param name="universityName">The string name for the university.</param>
        /// <param name="country">The string name for the country.</param>
        /// <returns>The first <see cref="ValidationResult"/> that fails, or a successful result if all fields are valid.</returns>
        public ValidationResult ValidateAllFields(
            string? universityName,
            string? country)
        {
            var validations = new ValidationResult[]
            {
                ValidateName(universityName),
                ValidateCountry(country)
            };

            var failedValidation = Array.Find(validations, v => !v.IsValid);

            if (failedValidation != null)
            {
                return failedValidation;
            }

            return new ValidationResult(true, null);
        }
    }
}