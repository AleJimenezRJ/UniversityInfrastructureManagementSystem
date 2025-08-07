using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;

/// <summary>
/// Service class responsible for handling operations related to Student entities.
/// </summary>
internal class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="StudentService"/> class.
    /// </summary>
    /// <param name="studentRepository">The repository used for student data persistence and retrieval.</param>
    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    /// <summary>
    /// Asynchronously creates a new student record in the data source.
    /// </summary>
    /// <param name="student">The student entity to be created.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean indicating success or failure.
    /// </returns>
    public Task<bool> CreateStudentAsync(Student student)
    {
        return _studentRepository.CreateStudentAsync(student);
    }

    /// <summary>
    /// Asynchronously creates a new list of students record in the data source.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of students.
    /// </returns>
    public Task<List<Student>?> ListStudentsAsync()
    {
        return _studentRepository.ListStudentsAsync();
    }
}