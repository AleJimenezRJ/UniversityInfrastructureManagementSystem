using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects.Spaces;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.ValueObject.Spaces;

public class IdTests
{
    [Fact]
    public void TryCreate_WithNullString_ReturnsFalse()
    {
        // Arrange
        string? input = null;

        // Act
        var result = Id.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "validation should fail when given a null string");
    }

    [Fact]
    public void TryCreate_WithNullString_OutVarIsNull()
    {
        // Arrange
        string? input = null;

        // Act
        var result = Id.TryCreate(input, out var id);

        // Assert
        id.Should().BeNull(because: "out variable should be null when given a null string");
    }

    [Fact]
    public void TryCreate_WithWithSpaceString_ReturnsFalse()
    {
        // Arrange
        string? input = " ";

        // Act
        var result = Id.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "validation should fail when given a white space string");
    }

    [Fact]
    public void TryCreate_WithEmptyString_OutVarIsNull()
    {
        // Arrange
        string? input = " ";

        // Act
        var result = Id.TryCreate(input, out var id);

        // Assert
        id.Should().BeNull(because: "out variable should be null when given a white space string");
    }

    [Fact]
    public void TryCreate_WithStringWithMoreThan5Digits_ReturnsFalse()
    {
        // Arrange
        string? input = "123456";

        // Act
        var result = Id.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "validation should fail when given a string with more than 5 digits");
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("0")]
    [InlineData("99999")]
    [InlineData("1")]
    public void TryCreate_WithStringFiveDigitsorLess_ReturnsTrue(string input)
    {
        // Arrange

        // Act
        var result = Id.TryCreate(input, out var output);

        // Assert
        result.Should().BeTrue(because: "validation should succeed for a string with 1 to 5 digits");
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("0")]
    [InlineData("99999")]
    [InlineData("1")]
    public void TryCreate_WithStringFiveDigitsorLess_CreatesId(string input)
    {
        // Arrange

        // Act
        Id.TryCreate(input, out var output);

        // Assert
        output!.Value.Should().Be(input, because: "the Id's value should match the input string when validation passes");
    }

    [Fact]
    public void TryCreate_WithNullInt_ReturnsFalse()
    {
        // Arrange
        int? input = null;

        // Act
        var result = Id.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "validation should fail when given a null integer");
    }

    [Fact]
    public void TryCreate_WithNullInt_OutVarIsNull()
    {
        // Arrange
        int? input = null;

        // Act
        Id.TryCreate(input, out var id);

        // Assert
        id.Should().BeNull(because: "out variable should be null when given a null integer");
    }

    [Fact]
    public void TryCreate_WithNegativeInt_ReturnsFalse()
    {
        // Arrange
        int? input = -1;

        // Act
        var result = Id.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "validation should fail when given a negative integer");
    }

    [Fact]
    public void TryCreate_WithNegativeInt_OutVarIsNull()
    {
        // Arrange
        int? input = -1;

        // Act
        Id.TryCreate(input, out var output);

        // Assert
        output.Should().BeNull(because: "out variable should be null when given a negative integer");
    }

    [Fact]
    public void TryCreate_WithIntGreaterThanMax_ReturnsFalse()
    {
        // Arrange
        int VALUE_GREATER_THAN_MAX = 100000;
        int? input = VALUE_GREATER_THAN_MAX;

        // Act
        var result = Id.TryCreate(input, out var _);

        // Assert
        result.Should().BeFalse(because: "validation should fail when given an integer greater than 99999");
    }

    [Fact]
    public void TryCreate_WithIntGreaterThanMax_OutVarIsNull()
    {
        // Arrange
        int VALUE_GREATER_THAN_MAX = 100000;
        int? input = VALUE_GREATER_THAN_MAX;

        // Act
        Id.TryCreate(input, out var output);

        // Assert
        output.Should().BeNull(because: "out variable should be null when given an integer greater than 99999");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(99999)]
    public void TryCreate_WithIntLessOrEqualThanMax_ReturnsTrue(int input)
    {
        // Arrange

        // Act
        var result = Id.TryCreate(input, out var id);

        // Assert
        result.Should().BeTrue(because: "validation should succeed for a valid integer");
    }

    [Theory]
    [InlineData(12345)]
    [InlineData(0)]
    [InlineData(99999)]
    [InlineData(1)]
    public void TryCreate_WithIntLessOrEqualThanMax_CreatesId(int input)
    {
        // Arrange

        // Act
        var result = Id.TryCreate(input, out var output);

        // Assert
        output!.ValueInt.Should().Be(input, because: "the Id's value should match the input integer when validation passes");
    }

    [Fact]
    public void Create_WithNullString_ThrowsValidationException()
    {
        // Arrange
        string? input = null;

        // Act
        Action act = () => Id.Create(input);

        // Assert
        act.Should().Throw<ValidationException>(because: "null string is an invalid input")
           .WithMessage("The Id is not valid. It must be 1 to 5 digits.");
    }

    [Fact]
    public void Create_WithWithSpaceString_ThrowsValidationException()
    {
        // Arrange
        string? input = " ";

        // Act
        Action act = () => Id.Create(input);

        // Assert
        act.Should().Throw<ValidationException>(because: "white space string is an invalid input")
           .WithMessage("The Id is not valid. It must be 1 to 5 digits.");
    }

    [Fact]
    public void Create_WithStringWithMoreThan5Digits_ThrowsValidationException()
    {
        // Arrange
        string? input = "123456";

        // Act
        Action act = () => Id.Create(input);

        // Assert
        act.Should().Throw<ValidationException>(because: "string with more than five digits is an invalid input")
           .WithMessage("The Id is not valid. It must be 1 to 5 digits.");
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("0")]
    [InlineData("99999")]
    [InlineData("1")]
    public void Create_WithStringFiveDigitsorLess_CreatesId(string input)
    {
        // Arrange
        
        // Act
        var output = Id.Create(input);

        // Assert
        output.Value.Should().Be(input, because: "the Id's value should match the input string when validation passes");
    }

    [Fact]
    public void Create_WithNullInt_ThrowsValidationException()
    {
        // Arrange
        int? input = null;

        // Act
        Action act = () => Id.Create(input);

        // Assert
        act.Should().Throw<ValidationException>(because: "null integer is an invalid input")
           .WithMessage("The Id must be a non-negative integer with up to 5 digits.");
    }

    [Fact]
    public void Create_WithNegativeInt_ThrowsValidationException()
    {
        // Arrange
        int input = -1;

        // Act
        Action act = () => Id.Create(input);

        // Assert
        act.Should().Throw<ValidationException>(because: "negative integer is an invalid input")
           .WithMessage("The Id must be a non-negative integer with up to 5 digits.");
    }

    [Fact]
    public void Create_WithIntGreaterThanMax_ThrowsValidationException()
    {
        // Arrange
        int VALUE_GREATER_THAN_MAX = 100000;
        int? input = VALUE_GREATER_THAN_MAX;

        // Act
        Action act = () => Id.Create(input);

        // Assert
        act.Should().Throw<ValidationException>(because: "integer greater than 99999 is an invalid input")
           .WithMessage("The Id must be a non-negative integer with up to 5 digits.");
    }

    [Theory]
    [InlineData(12345)]
    [InlineData(0)]
    [InlineData(99999)]
    [InlineData(1)]
    public void Create_WithIntLessOrEqualThanMax_CreatesId(int input)
    {
        // Arrange

        // Act
        var output = Id.Create(input);

        // Assert
        output.ValueInt.Should().Be(input, because: "the Id's value should match the input integer when validation passes");
    }

    [Fact]
    public void EqualityComparison_WithTwoIdsWithSameValue_ReturnsTrue()
    {
        // Arrange
        string input = "12345";

        // Act
        var id1 = Id.Create(input);
        var id2 = Id.Create(input);
        
        // Assert
        id1.Should().Be(id2, because: "value objects should be compared based on value and not reference");
    }

    [Fact]
    public void EqualityComparison_WithTwoIdsWithDifferentValues_ReturnsFalse()
    {
        // Arrange
        string input1 = "12345";
        string input2 = "54321";

        // Act
        var id1 = Id.Create(input1);
        var id2 = Id.Create(input2);

        // Assert
        id1.Should().NotBe(id2, because: "these value objects have different values");
    }
}
