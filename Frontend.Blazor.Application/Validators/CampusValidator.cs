using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Validators
{
    /// <summary>
    /// Provides validation logic for campus-related input fields.
    /// </summary>
    public class CampusValidator
    {
        /// <summary>
        /// Validates the campus name string.
        /// </summary>
        /// <param name="campusName">The string name of the campus.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the name is valid.</returns>
        public ValidationResult ValidateName(string? campusName)
        {
            if (string.IsNullOrWhiteSpace(campusName?.Trim()))
            {
                return new ValidationResult(false, "El nombre de la sede es obligatorio.");
            }

            if (campusName.Trim().Length > 100)
            {
                return new ValidationResult(false, "El nombre de la sede debe tener como máximo 100 caracteres.");
            }

            return new ValidationResult(true, null);
        }

        /// <summary>
        /// Validates the university name string associated with the campus.
        /// </summary>
        /// <param name="universityName">The string name of the university.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the university name is valid.</returns>
        public ValidationResult ValidateUniversityName(string? universityName)
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
        /// Validates the campus location string.
        /// </summary>
        /// <param name="location">The string description of the campus location.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the location is valid.</returns>
        public ValidationResult ValidateLocation(string? location)
        {
            if (string.IsNullOrWhiteSpace(location?.Trim()))
            {
                return new ValidationResult(false, "La ubicación de la sede es obligatoria.");
            }

            if (location.Trim().Length > 200) // Assuming a reasonable max length for a location description
            {
                return new ValidationResult(false, "La ubicación debe tener como máximo 200 caracteres.");
            }

            return new ValidationResult(true, null);
        }
    }
}