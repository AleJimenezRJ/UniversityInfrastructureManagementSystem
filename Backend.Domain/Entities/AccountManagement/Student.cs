using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

/// <summary>
/// Represents a student in the system. Inherits from <see cref="Person"/> and adds student-specific properties.
/// </summary>
public class Student 
{
    /// <summary>
    /// Gets or sets the unique identifier for the Person associated with this student.
    /// </summary>
    public int PersonId { get; set; }

    /// <summary>
    /// Gets or sets the student's unique identification number or code.
    /// </summary>
    public string StudentId { get; set; }

    /// <summary>
    /// Gets or sets the student's institutional email address.
    /// </summary>
    public Email InstitutionalEmail { get; set; }

    /// <summary>
    /// Gets or sets the Person object associated with this student.
    /// This property is initialized as non-null (using null-forgiving operator).
    /// </summary>
    public Person Person { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the Student class with the specified parameters.
    /// </summary>
    /// <param name="studentId">The student's unique identification number or code.</param>
    /// <param name="institutionalEmail">The student's institutional email address.</param>
    /// <param name="personId">The unique identifier for the associated Person.</param>
    public Student(string studentId, Email institutionalEmail, int personId)
    {
        StudentId = studentId;
        InstitutionalEmail = institutionalEmail;
        PersonId = personId;
    }

    /// <summary>
    /// Private parameterless constructor to the entity
    /// </summary>
    protected Student()
    {
        StudentId = null!;
        InstitutionalEmail = null!;
        PersonId = 0;
    }
}
