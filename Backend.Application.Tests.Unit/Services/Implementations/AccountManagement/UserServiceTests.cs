using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Backend.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Domain.ValueObjects;

namespace UCR.ECCI.PI.ThemePark.Backend.Application.Tests.Unit.Services.Implementations.AccountManagement;


/// The PBI that this test class is related to is: #118
/// Create New Users

/// Technical tasks to complete for the UserRole entity:
/// - Implement backend logic to store user data
/// - Add validations for required fields
/// - Write unit and integration tests

/// Participants: Elizabeth Huang & Esteban Baires

/// <summary>
/// Unit tests for the <see cref="UserService"/> class. 
/// </summary>
public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _serviceUnderTest;
    private readonly User _userToAdd;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserServiceTests"/> class.
    /// </summary>
    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _serviceUnderTest = new UserService(_userRepositoryMock.Object);
        _userToAdd = new User(UserName.Create("testuser"), 1);
    }

    /// <summary>
    /// Tests the CreateUserAsync method of the UserService class to ensure it calls the repository's CreateUserAsync method.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task DeleteUserAsync_WhenGivenValidParameters_CallsDeleteOnRepository()
    {
        // Arrange
        _userRepositoryMock
            .Setup(userRepository => userRepository.DeleteUserAsync(_userToAdd.Id))
            .ReturnsAsync(true);
        // Act
        var result = await _serviceUnderTest.DeleteUserAsync(_userToAdd.Id);
        // Assert
        _userRepositoryMock.Verify(
            userRepository => userRepository.DeleteUserAsync(_userToAdd.Id),
            Times.AtLeastOnce(),
            failMessage: "Service should always call DeleteAsync on repository when deleting a User");
    }

    /// <summary>
    /// Tests the DeleteUserAsync method of the UserService class to ensure it returns the result from the repository.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task DeleteUserAsync_WhenGivenValidParameters_ReturnsResultFromRepository()
    {
        // Arrange
        _userRepositoryMock
            .Setup(userRepository => userRepository.DeleteUserAsync(_userToAdd.Id))
            .ReturnsAsync(true);
        // Act
        var result = await _serviceUnderTest.DeleteUserAsync(_userToAdd.Id);
        // Assert
        result.Should().BeTrue(because: "Service should return the result from the repository");
    }

    /// <summary>
    /// Tests the CreateUserAsync method of the UserService class to ensure it calls the repository's CreateUserAsync method.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task CreateUserAsync_WhenGivenValidParameters_CallsCreateOnRepository()
    {
        // Arrange
        _userRepositoryMock
            .Setup(userRepository => userRepository.CreateUserAsync(_userToAdd))
            .ReturnsAsync(true);
        // Act
        var result = await _serviceUnderTest.CreateUserAsync(_userToAdd);

        // Assert
        _userRepositoryMock.Verify(
            userRepository => userRepository.CreateUserAsync(_userToAdd),
            Times.AtLeastOnce(),
            failMessage: "Service should always call CreateAsync on repository when adding a new User");
    }

    /// <summary>
    /// Tests the CreateUserAsync method of the UserService class.
    /// </summary>
    /// <param name="expectedResult"></param>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CreateUserAsync_WhenGivenValidParameters_ReturnsResultFromRepository(bool expectedResult)
    {
        // Arrange
        _userRepositoryMock
            .Setup(userRepository => userRepository.CreateUserAsync(_userToAdd))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _serviceUnderTest.CreateUserAsync(_userToAdd);
        // Assert
        result.Should().Be(expectedResult, because: "Service should return the result from the repository");
    }

    /// <summary>
    /// Tests the GetAllUsersAsync method of the UserService class to ensure it returns a list of users from the repository.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task GetAllUsersAsync_WhenCalled_ReturnsListOfUsers()
    {
        // Arrange
        var expectedUsers = new List<User> { _userToAdd };
        _userRepositoryMock
            .Setup(repo => repo.GetAllUsersAsync())
            .ReturnsAsync(expectedUsers);
        // Act
        var result = await _serviceUnderTest.GetAllUsersAsync();
        // Assert
        result.Should().BeEquivalentTo(expectedUsers, because: "Service should return the list of users from the repository");
    }

    /// <summary>
    /// Tests the GetAllUsersAsync method of the UserService class to ensure it returns an empty list when no users exist in the repository.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task GetAllUsersAsync_WhenNoUsersExist_ReturnsEmptyList()
    {
        // Arrange
        _userRepositoryMock
            .Setup(repo => repo.GetAllUsersAsync())
            .ReturnsAsync(new List<User>());
        // Act
        var result = await _serviceUnderTest.GetAllUsersAsync();
        // Assert
        result.Should().BeEmpty(because: "Service should return an empty list when no users exist in the repository");
    }

    /// <summary>
    /// Tests the ModifyUserAsync method of the UserService class to ensure it calls the repository's ModifyUserAsync method with valid parameters.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task ModifyUserAsync_WhenGivenValidParameters_CallsModifyOnRepository()
    {
        // Arrange
        var userId = 1;
        _userRepositoryMock
            .Setup(userRepository => userRepository.ModifyUserAsync(userId, _userToAdd))
            .ReturnsAsync(true);
        // Act
        var result = await _serviceUnderTest.ModifyUserAsync(userId, _userToAdd);
        // Assert
        _userRepositoryMock.Verify(
            userRepository => userRepository.ModifyUserAsync(userId, _userToAdd),
            Times.AtLeastOnce(),
            failMessage: "Service should always call ModifyAsync on repository when modifying a User");
    }

    /// <summary>
    /// Tests the ModifyUserAsync method of the UserService class to ensure it returns the result from the repository when given valid parameters.
    /// </summary>
    /// <returns>
    /// Returns a task that represents the asynchronous operation.
    /// </returns>
    [Fact]
    public async Task ModifyUserAsync_WhenGivenValidParameters_ReturnsResultFromRepository()
    {
        // Arrange
        var userId = 1;
        _userRepositoryMock
            .Setup(userRepository => userRepository.ModifyUserAsync(userId, _userToAdd))
            .ReturnsAsync(true);
        // Act
        var result = await _serviceUnderTest.ModifyUserAsync(userId, _userToAdd);
        // Assert
        result.Should().BeTrue(because: "Service should return the result from the repository");
    }
}