using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Validators
{
    /// <summary>
    /// Provides validation logic for area-related input fields.
    /// </summary>
    public class AreaValidator
    {
        /// <summary>
        /// Validates the area name string.
        /// </summary>
        /// <param name="areaName">The string name of the area.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the name is valid.</returns>
        public ValidationResult ValidateName(string? areaName)
        {
            if (string.IsNullOrWhiteSpace(areaName?.Trim()))
            {
                return new ValidationResult(false, "El nombre del área es obligatorio.");
            }

            if (areaName.Trim().Length > 100)
            {
                return new ValidationResult(false, "El nombre del área debe tener como máximo 100 caracteres.");
            }

            return new ValidationResult(true, null);
        }

        /// <summary>
        /// Validates the campus name string associated with the area.
        /// </summary>
        /// <param name="campusName">The string name of the campus.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the campus name is valid.</returns>
        public ValidationResult ValidateCampusName(string? campusName)
        {
            if (string.IsNullOrWhiteSpace(campusName?.Trim()))
            {
                return new ValidationResult(false, "El nombre del campus es obligatorio.");
            }

            // Assuming campus name also has a max length, similar to building/area names
            if (campusName.Trim().Length > 100)
            {
                return new ValidationResult(false, "El nombre del campus debe tener como máximo 100 caracteres.");
            }

            return new ValidationResult(true, null);
        }

        /// <summary>
        /// Validates if a campus object has been selected or provided.
        /// This is for scenarios where the UI sends the Campus object directly.
        /// </summary>
        /// <param name="campus">The Campus entity associated with the area.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the campus is valid.</returns>
        public ValidationResult ValidateCampusObject(Campus? campus)
        {
            if (campus == null)
            {
                return new ValidationResult(false, "Debe seleccionar un campus.");
            }
            return new ValidationResult(true, null);
        }


        /// <summary>
        /// Validates all fields of the area form at once.
        /// </summary>
        /// <param name="areaName">The string name for the area.</param>
        /// <param name="campusName">The string name for the campus.</param>
        /// <returns>The first <see cref="ValidationResult"/> that fails, or a successful result if all fields are valid.</returns>
        public ValidationResult ValidateAllFields(
            string? areaName,
            string? campusName)
        {
            var validations = new ValidationResult[]
            {
                ValidateName(areaName),
                ValidateCampusName(campusName)
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