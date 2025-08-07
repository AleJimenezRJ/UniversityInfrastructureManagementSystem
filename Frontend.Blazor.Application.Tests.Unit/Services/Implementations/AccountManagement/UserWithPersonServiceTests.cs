using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Tests.Unit.Services.Implementations.AccountManagement;

/// <summary>
/// Unit tests for the <see cref="UserWithPersonService"/> class.
/// </summary>
/// The PBI that this test class is related to is: #214
/// Manage UserWithPerson Entity
/// Technical tasks to complete:
/// – Implement backend logic to manage combined user and person data
/// – Add validations for null and existing user entries
/// – Write unit and integration tests
/// Participants: Andres Murillo & Tatiana Paramo
public class UserWithPersonServiceTests
{
    /// <summary>
    /// Mock repository for user-with-person operations.
    /// </summary>
    private readonly Mock<IUserWithPersonRepository> _userWithPersonRepositoryMock;

    /// <summary>
    /// Mock service for user role operations.
    /// </summary>
    private readonly Mock<IUserRoleService> _userRoleServiceMock;

    /// <summary>
    /// Service under test, which uses the mocked repository and role service.
    /// </summary>
    private readonly UserWithPersonService _serviceUnderTest;

    /// <summary>
    /// A valid user with person data used for testing various methods in the service.
    /// </summary>
    private readonly UserWithPerson _validUser;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserWithPersonServiceTests"/> class.
    /// </summary>
    public UserWithPersonServiceTests()
    {
        _userWithPersonRepositoryMock = new(MockBehavior.Strict);
        _userRoleServiceMock = new(MockBehavior.Strict);
        _serviceUnderTest = new UserWithPersonService(_userWithPersonRepositoryMock.Object, _userRoleServiceMock.Object);

        _validUser = new UserWithPerson(
            userName: UserName.Create("juan.perez"),
            firstName: "Juan",
            lastName: "Perez",
            email: Email.Create("juanperez@gmail.com"),
            phone: Phone.Create("8888-9999"),
            identityNumber: IdentityNumber.Create("1-1765-0932"),
            birthDate: BirthDate.Create(new DateOnly(2000, 4, 29)),
            roles: new List<string> { "Admin" },
            userId: 1,
            personId: 10
        );

    }

    /// <summary>
    /// Tests that CreateUserWithPersonAsync method invokes the repository correctly.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task CreateUserWithPersonAsync_WhenCalled_InvokesRepository()
    {
        // Arrange
        _userWithPersonRepositoryMock
            .Setup(repo => repo.CreateUserWithPersonAsync(_validUser))
            .ReturnsAsync(true);

        // Act
        var result = await _serviceUnderTest.CreateUserWithPersonAsync(_validUser);

        // Assert
        result.Should().BeTrue(because: "repository should return true if creation succeeds");
        _userWithPersonRepositoryMock.Verify(repo => repo.CreateUserWithPersonAsync(_validUser), Times.Once);
    }

    /// <summary>
    /// Tests that DeleteUserWithPersonAsync method invokes the repository correctly.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task DeleteUserWithPersonAsync_WhenCalled_InvokesRepository()
    {
        // Arrange
        _userWithPersonRepositoryMock
            .Setup(repo => repo.DeleteUserWithPersonAsync(_validUser.UserId, _validUser.PersonId))
            .ReturnsAsync(true);

        // Act
        var result = await _serviceUnderTest.DeleteUserWithPersonAsync(_validUser.UserId, _validUser.PersonId);

        // Assert
        result.Should().BeTrue();
        _userWithPersonRepositoryMock.Verify(repo => repo.DeleteUserWithPersonAsync(_validUser.UserId, _validUser.PersonId), Times.Once);
    }

    /// <summary>
    /// Tests that UpdateUserWithPersonAsync method throws ArgumentNullException when user is null.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task UpdateUserWithPersonAsync_WhenUserIsNull_ThrowsArgumentNullException()
    {
        // Act
        Func<Task> act = async () => await _serviceUnderTest.UpdateUserWithPersonAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>()
            .WithMessage("User cannot be null*");
    }

    /// <summary>
    /// Tests that GetAllUserWithPersonAsync method returns the same list as the repository.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetAllUserWithPersonAsync_WhenRepositoryReturnsList_ReturnsSameList()
    {
        // Arrange
        var expectedList = new List<UserWithPerson> { _validUser };
        _userWithPersonRepositoryMock
            .Setup(repo => repo.GetAllUserWithPersonAsync())
            .ReturnsAsync(expectedList);

        // Act
        var result = await _serviceUnderTest.GetAllUserWithPersonAsync();

        // Assert
        result.Should().BeSameAs(expectedList,
            because: "service should forward whatever the repository returns");
        _userWithPersonRepositoryMock.Verify(repo => repo.GetAllUserWithPersonAsync(), Times.Once);
    }

    /// <summary>
    /// Tests that UpdateUserWithPersonAsync method invokes the repository with a valid user.
    /// </summary>
    /// <returns> A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task UpdateUserWithPersonAsync_WhenCalledWithValidUser_InvokesRepository()
    {
        // Arrange
        _userWithPersonRepositoryMock
            .Setup(repo => repo.UpdateUserWithPersonAsync(_validUser))
            .ReturnsAsync(true);

        // Act
        var result = await _serviceUnderTest.UpdateUserWithPersonAsync(_validUser);

        // Assert
        result.Should().BeTrue();
        _userWithPersonRepositoryMock.Verify(repo => repo.UpdateUserWithPersonAsync(_validUser), Times.Once);
    }
}