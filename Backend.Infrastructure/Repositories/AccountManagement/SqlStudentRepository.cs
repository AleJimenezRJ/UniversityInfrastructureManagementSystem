using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Exceptions;
namespace UCR.ECCI.PI.ThemePark.Backend.Infrastructure.Repositories.AccountManagement;

/// <summary>
/// Implementation of <see cref="IStudentRepository"/> that interacts with the SQL database using EF Core.
/// </summary>
internal class SqlStudentRepository : IStudentRepository
{
    private readonly ThemeParkDataBaseContext _dbContext;
    private readonly ILogger<SqlStudentRepository> _logger;

    /// <summary>
    /// Constructor for <see cref="SqlStudentRepository"/>.
    /// </summary>
    /// <param name="dbContext">Injected database context for accessing data.</param>
    /// <param name="logger">Logger for recording operational messages and errors.</param>
    public SqlStudentRepository(ThemeParkDataBaseContext dbContext, ILogger<SqlStudentRepository> logger)
    => (_dbContext, _logger) = (dbContext, logger);

    /// <summary>
    /// Creates a new <see cref="Student"/> entry in the database.
    /// </summary>
    /// <param name="Student">The Student entity to insert.</param>
    /// <returns>True if creation was successful; false otherwise.</returns>
    public async Task<bool> CreateStudentAsync(Student student)
    {
        try
        {
            // Search for existing person by identity number
            var email = student.Person.Email;
            var identity = student.Person.IdentityNumber;

            var existingPerson = await _dbContext.Persons
                .FirstOrDefaultAsync(p =>
                    p.IdentityNumber == identity || p.Email == email
                    );
            // Verify if theres already a student with that StudentID
            bool studentExists = await _dbContext.Students
                .AnyAsync(s => s.StudentId == student.StudentId);

            if (studentExists)
            {
                _logger.LogWarning("Student already exists with ID: {StudentId}", student.StudentId);
                throw new DuplicatedEntityException($"Attempt to create student with existing StudentId");
            }

            if (existingPerson != null)
            {
                // Reuse existing person
                student.PersonId = existingPerson.Id;
                student.Person = existingPerson;
            }
            else
            {
                // Create new person
                _dbContext.Persons.Add(student.Person);
                await _dbContext.SaveChangesAsync();
                student.PersonId = student.Person.Id;
            }

            // Save student
            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Student created: {StudentId} linked to PersonId: {PersonId}", student.StudentId, student.PersonId);
            return true;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Database context is null when creating student with StudentId {StudentId}", student.StudentId);
            throw new DomainException($"An unexpected error occurred while creating the student: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Operation was canceled while creating student with StudentId {StudentId}", student.StudentId);
            throw new DomainException($"An unexpected error occurred while creating the student: {ex.Message}");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update error while creating student with StudentId {StudentId}", student.StudentId);
            throw new DomainException($"An unexpected error occurred while creating the student: {ex.Message}");
        }

        }

    /// <summary>
    /// Retrieves all students with their related person data.
    /// </summary>
    /// <returns>
    /// A list of students, or <c>null</c> if an error occurs.
    /// </returns>
    public async Task<List<Student>?> ListStudentsAsync()
    {
        try
        {
            var students = await _dbContext.Students
                .Include(s => s.Person)
                .ToListAsync();

            return students;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Database context is null when listing students");
            throw new DomainException($"An unexpected error occurred while listing students: {ex.Message}");
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Operation was canceled while listing students");
            throw new DomainException($"An unexpected error occurred while listing students: {ex.Message}");
        }

    }

}
