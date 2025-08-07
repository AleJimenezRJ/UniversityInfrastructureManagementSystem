using FluentAssertions;
using System.ComponentModel;
using System.Reflection.Metadata;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.AccountManagement;

// Participants: Andres Murillo and Tatiana Paramo
// Role rotation: Every 30 minutes we switch between Driver and Navigator
// User Story: SPT-SR-007 Add institutional email to user profile  
//
// Technical Tasks
// - Implement staff and student person type
// - Update user creation form to support two email inputs	
// - Modify backend user model to store both emails  
// - Implement validation for both emails(format and uniqueness)    
// - Ensure emails are saved and retrieved properly	
// - Update API documentation and admin interface guidelines 
// - Write tests for creation with one or both emails    
// - Handle errors(duplicate emails, invalid formats, etc.) 
// - Code review and PR 
//
// Acceptance Criteria
// - A student can be successfully saved with both personal and institutional emails.
// - Invalid email formats are rejected.
// - Duplicate institutional emails are not allowed.

/// <summary>
/// Unit tests for the <see cref="Student"/> class.
/// </summary>
public class StudentTests
{
    /// <summary>
    /// Tests that the <see cref="Student"/> constructor initializes all properties correctly
    /// when valid values are provided.
    /// </summary>
    [Fact]
    public void Constructor_WithValidInputs_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var studentId = "B12345";
        var email =  Email.Create("estudiante@ucr.ac.cr");
        var personId = 10;

        // Act
        var student = new Student(studentId, email, personId);

        // Assert
        student.StudentId.Should().Be(studentId);
        student.InstitutionalEmail.Should().Be(email);
        student.PersonId.Should().Be(personId);
    }

    /// <summary>
    /// Tests that the protected constructor of <see cref="Student"/> initializes
    /// properties to their expected default values. This constructor is typically
    /// used by ORMs or serializers and is not intended for direct use.
    /// </summary>
    [Fact]
    public void ProtectedConstructor_ShouldInitializePersonIdAndAllowNulls()
    {
        // Act
        var student = typeof(Student)
            .GetConstructors(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .First()
            .Invoke(null) as Student;

        // Assert
        student.Should().NotBeNull();
        student!.PersonId.Should().Be(0);
        student.StudentId.Should().BeNull();
        student.InstitutionalEmail.Should().BeNull();
    }

    /// <summary>
    /// Tests that the <see cref="Student"/> constructor throws a <see cref="ValidationException"/>
    /// when provided with an invalid institutional email format. Covers common malformed cases
    /// such as missing '@' or domain.
    /// </summary>
    /// <param name="invalidEmail">An invalid email string to test against the Email value object.</param>
    [Theory]
    [InlineData("")]
    [InlineData("no-arroba")]
    [InlineData("correo@")]
    [InlineData("correo@invalido.")]
    public void Constructor_WithInvalidEmailFormat_ShouldThrow(string invalidEmail)
    {
        // Arrange
        var studentId = "B12345";
        var personId = 1;

        // Act
        Action act = () => new Student(studentId, Email.Create(invalidEmail), personId);

        // Assert
        act.Should().Throw<ValidationException>().WithMessage("*email*");
    }


    /// <summary>
    /// Ensures that calling <see cref="Student"/> does not throw an exception
    /// and returns a non-empty string. This verifies default behavior even when the
    /// method is not explicitly overridden.
    /// </summary>
    [Fact]
    public void ToString_ShouldNotThrow()
    {
        var student = new Student("B67890", Email.Create("test@ucr.ac.cr"), 3);
        var result = student.ToString();
        result.Should().NotBeNullOrWhiteSpace();
    }
}
