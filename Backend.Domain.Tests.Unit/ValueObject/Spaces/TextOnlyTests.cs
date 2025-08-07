using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.ValueObject.Spaces
{
    /// <summary>
    /// Unit tests for the TextOnly value object.
    /// </summary>
    public class TextOnlyTests
    {
        [Theory]
        [InlineData("Hola Mundo")]
        [InlineData("áéíóú ÁÉÍÓÚ ñ Ñ ü Ü")]
        [InlineData("")]
        [InlineData("SoloLetras")]

        /// <summary>
        /// Tests that valid inputs for TextOnly return true and create a valid value object.
        /// </summary>
        /// <param name="input">The input string to test.</param>
        public void TryCreate_WithValidInputs_ReturnsTrue(string input)
        {
            var result = TextOnly.TryCreate(input, out var vo);
            Assert.True(result);
            Assert.NotNull(vo);
            Assert.Equal(input.Trim(), vo!.Value);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("Hola123")]
        [InlineData("Hola!")]
        [InlineData("@#$%^")]
        [InlineData("con-numero-1")]
        /// <summary>
        /// Tests that invalid inputs for TextOnly return false and do not create a value object.
        /// </summary>
        /// <param name="input">The input string to test.</param>
        public void TryCreate_WithInvalidInputs_ReturnsFalse(string input)
        {
            var result = TextOnly.TryCreate(input, out var vo);
            Assert.False(result);
            Assert.Null(vo);
        }

        [Fact]
        /// <summary>
        /// Tests that invalid input for TextOnly throws a validation exception.
        /// </summary>
        /// <param name="input">The input string that causes the exception.</param>
        public void Create_WithInvalid_ThrowsValidationException()
        {
            Assert.ThrowsAny<Exception>(() => TextOnly.Create("1234!"));
        }

        [Fact]
        /// <summary>
        /// Tests that valid input for TextOnly returns a value object.
        /// </summary>
        /// 
        public void Create_WithValid_ReturnsValueObject()
        {
            var vo = TextOnly.Create("Prueba de Texto");
            Assert.Equal("Prueba de Texto", vo.Value);
        }
    }
}
