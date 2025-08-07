using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Tests.Unit.Services.Implementations.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="StudentService"/> class, 
/// verifying the interaction with the <see cref="IStudentRepository"/>.
/// </summary>
public class StudentServiceTests
{
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly StudentService _serviceUnderTest;
    private readonly List<Student> _sampleStudents;

    /// <summary>
    /// Initializes a new instance of the <see cref="StudentServiceTests"/> class,
    /// setting up the mock repository and sample student data for test scenarios.
    /// </summary>
    public StudentServiceTests()
    {
        _studentRepositoryMock = new Mock<IStudentRepository>(MockBehavior.Strict);
        _serviceUnderTest = new StudentService(_studentRepositoryMock.Object);

        _sampleStudents = new List<Student>
        {
            new Student("S001", Email.Create("andres.murillo@ucr.ac.cr"), 1),
            new Student("S002", Email.Create("isabella.rodriguez@ucr.ac.cr"), 2)
        };
    }

    /// <summary>
    /// Tests the <see cref="StudentService.CreateStudentAsync"/> method to ensure it 
    /// returns the result from the repository correctly.
    /// </summary>
    /// <param name="expectedResult">The expected result returned by the repository (true or false).</param>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CreateStudentAsync_Always_ReturnsRepositoryResult(bool expectedResult)
    {
        // Arrange
        var student = new Student("S003", Email.Create("gael.alpizar@ucr.ac.cr"), 3);
        _studentRepositoryMock
            .Setup(repo => repo.CreateStudentAsync(student))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _serviceUnderTest.CreateStudentAsync(student);

        // Assert
        result.Should().Be(expectedResult);
    }

    /// <summary>
    /// Verifies that <see cref="StudentService.ListStudentsAsync"/> returns a list of students
    /// when there are students available in the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task ListStudentsAsync_WhenStudentsExist_ReturnsStudentList()
    {
        // Arrange
        _studentRepositoryMock
            .Setup(repo => repo.ListStudentsAsync())
            .ReturnsAsync(_sampleStudents);

        // Act
        var result = await _serviceUnderTest.ListStudentsAsync();

        // Assert
        result.Should().BeEquivalentTo(_sampleStudents);
    }

    /// <summary>
    /// Verifies that <see cref="StudentService.ListStudentsAsync"/> returns an empty list
    /// when no students are available in the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task ListStudentsAsync_WhenNoStudentsExist_ReturnsEmptyList()
    {
        // Arrange
        _studentRepositoryMock
            .Setup(repo => repo.ListStudentsAsync())
            .ReturnsAsync(new List<Student>());

        // Act
        var result = await _serviceUnderTest.ListStudentsAsync();

        // Assert
        result.Should().BeEmpty();
    }
}
