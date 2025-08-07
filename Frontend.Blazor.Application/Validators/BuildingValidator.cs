using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Validators
{
    /// <summary>
    /// Provides validation logic for building-related input fields.
    /// </summary>
    public class BuildingValidator
    {
        /// <summary>
        /// Validates the building name.
        /// </summary>
        /// <param name="buildingName">The name of the building.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the name is valid.</returns>
        public ValidationResult ValidateName(string? buildingName)
        {
            if (string.IsNullOrWhiteSpace(buildingName?.Trim()))
            {
                return new ValidationResult(false, "El nombre es obligatorio.");
            }

            if (buildingName.Trim().Length > 100)
            {
                return new ValidationResult(false, "El nombre debe tener como máximo 100 caracteres.");
            }

            return new ValidationResult(true, null);
        }

        /// <summary>
        /// Validates the area name.
        /// </summary>
        /// <param name="area">The name of the area.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the area is valid.</returns>
        public ValidationResult ValidateArea(string? area)
        {
            if (string.IsNullOrWhiteSpace(area?.Trim()))
            {
                return new ValidationResult(false, "El área es obligatoria.");
            }

            return new ValidationResult(true, null);
        }

        /// <summary>
        /// Validates the building width.
        /// </summary>
        /// <param name="width">The width of the building.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the width is valid.</returns>
        public ValidationResult ValidateWidth(double? width)
        {
            if (width is null)
            {
                return new ValidationResult(false, "El ancho es obligatorio.");
            }

            if (width <= 0)
            {
                return new ValidationResult(false, "El ancho debe ser un número positivo.");
            }

            return new ValidationResult(true, null);
        }

        /// <summary>
        /// Validates the building length.
        /// </summary>
        /// <param name="length">The length of the building.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the length is valid.</returns>
        public ValidationResult ValidateLength(double? length)
        {
            if (length is null)
            {
                return new ValidationResult(false, "El largo es obligatorio.");
            }

            if (length <= 0)
            {
                return new ValidationResult(false, "El largo debe ser un número positivo.");
            }

            return new ValidationResult(true, null);
        }

        /// <summary>
        /// Validates the building height.
        /// </summary>
        /// <param name="height">The height of the building.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the height is valid.</returns>
        public ValidationResult ValidateHeight(double? height)
        {
            if (height is null)
            {
                return new ValidationResult(false, "El alto es obligatorio.");
            }

            if (height <= 0)
            {
                return new ValidationResult(false, "El alto debe ser un número positivo.");
            }

            return new ValidationResult(true, null);
        }

        /// <summary>
        /// Validates the X coordinate of the building.
        /// </summary>
        /// <param name="coordinateX">The X coordinate.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the coordinate is valid.</returns>
        public ValidationResult ValidateCoordinateX(double? coordinateX)
        {
            if (coordinateX is null)
            {
                return new ValidationResult(false, "La coordenada X no ha sido seleccionada.");
            }

            return new ValidationResult(true, null);
        }

        /// <summary>
        /// Validates the Y coordinate of the building.
        /// </summary>
        /// <param name="coordinateY">The Y coordinate.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the coordinate is valid.</returns>
        public ValidationResult ValidateCoordinateY(double? coordinateY)
        {
            if (coordinateY is null)
            {
                return new ValidationResult(false, "La coordenada Y no ha sido seleccionada.");
            }

            return new ValidationResult(true, null);
        }

        /// <summary>
        /// Validates the selected color for the building.
        /// </summary>
        /// <param name="selectedColor">The selected color.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the color is valid.</returns>
        public ValidationResult ValidateColor(string? selectedColor)
        {
            if (string.IsNullOrWhiteSpace(selectedColor))
            {
                return new ValidationResult(false, "El color es obligatorio.");
            }

            return new ValidationResult(true, null);
        }

        /// <summary>
        /// Validates all fields of the building form at once.
        /// </summary>
        /// <param name="buildingName">The building name.</param>
        /// <param name="area">The area name.</param>
        /// <param name="width">The width of the building.</param>
        /// <param name="length">The length of the building.</param>
        /// <param name="height">The height of the building.</param>
        /// <param name="coordinateX">The X coordinate of the building.</param>
        /// <param name="coordinateY">The Y coordinate of the building.</param>
        /// <param name="selectedColor">The selected color.</param>
        /// <returns>The first <see cref="ValidationResult"/> that fails, or a successful result if all fields are valid.</returns>
        public ValidationResult ValidateAllFields(
            string? buildingName,
            string? area,
            double? width,
            double? length,
            double? height,
            double? coordinateX,
            double? coordinateY,
            string? selectedColor)
        {
            var validations = new ValidationResult[]
            {
                ValidateName(buildingName),
                ValidateArea(area),
                ValidateWidth(width),
                ValidateLength(length),
                ValidateHeight(height),
                ValidateCoordinateX(coordinateX),
                ValidateCoordinateY(coordinateY),
                ValidateColor(selectedColor)
            };

            var failedValidation = Array.Find(validations, v => !v.IsValid);

            if (failedValidation != null)
            {
                return failedValidation;
            }

            return new ValidationResult(true, null);
        }
    }

    /// <summary>
    /// Represents the result of a validation operation.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Indicates whether the validation passed.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// The error message associated with the validation result, if any.
        /// </summary>
        public string? ErrorMessage { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult"/> class.
        /// </summary>
        /// <param name="isValid">Indicates whether the validation was successful.</param>
        /// <param name="errorMessage">The error message, if the validation failed.</param>
        public ValidationResult(bool isValid, string? errorMessage)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }
    }
}
