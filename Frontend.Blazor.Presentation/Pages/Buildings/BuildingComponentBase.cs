using Microsoft.AspNetCore.Components;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Validators;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Pages.Buildings
{
    /// <summary>
    /// Base component for building-related Blazor pages.
    /// Handles validation and stores error messages for building form fields.
    /// </summary>
    public abstract class BuildingComponentBase : ComponentBase
    {
        /// <summary>
        /// Instance of the building validator used for input validation.
        /// </summary>
        protected BuildingValidator Validator { get; } = new BuildingValidator();

        protected string? WidthError { get; set; }
        protected string? LengthError { get; set; }
        protected string? HeightError { get; set; }
        protected string? NameError { get; set; }
        protected string? AreaError { get; set; }
        protected string? CoordinateXError { get; set; }
        protected string? CoordinateYError { get; set; }
        protected string? ColorError { get; set; }

        /// <summary>
        /// Validates the building name and updates the corresponding error message.
        /// </summary>
        /// <param name="buildingName">The name of the building to validate.</param>
        protected void ValidateNameFormat(string? buildingName)
        {
            var result = Validator.ValidateName(buildingName);
            NameError = result.ErrorMessage;
        }

        /// <summary>
        /// Validates the area name and updates the corresponding error message.
        /// </summary>
        /// <param name="area">The area name to validate.</param>
        protected void ValidateAreaFormat(string? area)
        {
            var result = Validator.ValidateArea(area);
            AreaError = result.ErrorMessage;
        }

        /// <summary>
        /// Validates the building width and updates the corresponding error message.
        /// </summary>
        /// <param name="width">The width to validate.</param>
        protected void ValidateWidth(double? width)
        {
            var result = Validator.ValidateWidth(width);
            WidthError = result.ErrorMessage;
        }

        /// <summary>
        /// Validates the building length and updates the corresponding error message.
        /// </summary>
        /// <param name="length">The length to validate.</param>
        protected void ValidateLength(double? length)
        {
            var result = Validator.ValidateLength(length);
            LengthError = result.ErrorMessage;
        }

        /// <summary>
        /// Validates the building height and updates the corresponding error message.
        /// </summary>
        /// <param name="height">The height to validate.</param>
        protected void ValidateHeight(double? height)
        {
            var result = Validator.ValidateHeight(height);
            HeightError = result.ErrorMessage;
        }

        /// <summary>
        /// Validates the X coordinate and updates the corresponding error message.
        /// </summary>
        /// <param name="coordinateX">The X coordinate to validate.</param>
        protected void ValidateCoordinateX(double? coordinateX)
        {
            var result = Validator.ValidateCoordinateX(coordinateX);
            CoordinateXError = result.ErrorMessage;
        }

        /// <summary>
        /// Validates the Y coordinate and updates the corresponding error message.
        /// </summary>
        /// <param name="coordinateY">The Y coordinate to validate.</param>
        protected void ValidateCoordinateY(double? coordinateY)
        {
            var result = Validator.ValidateCoordinateY(coordinateY);
            CoordinateYError = result.ErrorMessage;
        }

        /// <summary>
        /// Validates the selected color and updates the corresponding error message.
        /// </summary>
        /// <param name="selectedColor">The color to validate.</param>
        protected void ValidateColorFormat(string? selectedColor)
        {
            var result = Validator.ValidateColor(selectedColor);
            ColorError = result.ErrorMessage;
        }

        /// <summary>
        /// Determines whether there are any current validation errors.
        /// </summary>
        /// <returns>True if any validation error messages are not empty; otherwise, false.</returns>
        protected bool HasValidationErrors()
        {
            return !string.IsNullOrWhiteSpace(NameError) ||
                   !string.IsNullOrWhiteSpace(AreaError) ||
                   !string.IsNullOrWhiteSpace(WidthError) ||
                   !string.IsNullOrWhiteSpace(LengthError) ||
                   !string.IsNullOrWhiteSpace(HeightError) ||
                   !string.IsNullOrWhiteSpace(CoordinateXError) ||
                   !string.IsNullOrWhiteSpace(CoordinateYError) ||
                   !string.IsNullOrWhiteSpace(ColorError);
        }

        /// <summary>
        /// Clears all validation error messages.
        /// </summary>
        protected void ClearValidationErrors()
        {
            NameError = null;
            AreaError = null;
            WidthError = null;
            LengthError = null;
            HeightError = null;
            CoordinateXError = null;
            CoordinateYError = null;
            ColorError = null;
        }
    }
}
