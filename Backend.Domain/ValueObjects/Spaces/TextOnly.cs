using System.Text.RegularExpressions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.Spaces
{
    /// <summary>
    /// Value Object that represents a text-only string (letters and spaces only).
    /// </summary>
    public sealed class TextOnly
    {
        private static readonly Regex _regex = new Regex(@"^[A-Za-zÁÉÍÓÚáéíóúÑñÜü\s]*$", RegexOptions.Compiled);

        public string Value { get; }

        private TextOnly(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Tries to create a TextOnly value object. Returns true if valid, false otherwise.
        /// </summary>
        public static bool TryCreate(string? value, out TextOnly? result)
        {
            result = null;
            if (!_regex.IsMatch(value!))
            {
                return false;
            }

            result = new TextOnly(value!.Trim());
            return true;
        }

        /// <summary>
        /// Creates a TextOnly value object or throws ValidationException if invalid.
        /// </summary>
        public static TextOnly Create(string value)
        {
            if (!TryCreate(value, out var result))
                throw new ValidationException("The text only can contain letters and spaces and can not be empty.");
            return result!;
        }

        public override string ToString() => Value;
    }
}
