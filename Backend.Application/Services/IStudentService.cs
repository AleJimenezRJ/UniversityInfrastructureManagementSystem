using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Services;

/// <summary>
/// Defines the contract for student-related operations in the application.
/// </summary>
public interface IStudentService
{
    /// <summary>
    /// Creates a new student in the system.
    /// </summary>
    /// <param name="student"> The student object containing the details of the student to be created.</param>
    /// <returns> A task that represents the asynchronous operation. 
    /// The task result contains a boolean indicating success or failure.</returns>
    Task<bool> CreateStudentAsync(Student student);

    /// <summary>
    /// Displays a list of students of the system.
    /// </summary>
    /// <returns> A task that represents the asynchronous operation. 
    /// The task result contains a list of students.</returns>
    Task<List<Student>?> ListStudentsAsync();
}
