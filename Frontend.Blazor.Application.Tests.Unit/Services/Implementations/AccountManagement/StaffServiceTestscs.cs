using FluentAssertions;
using Moq;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Repositories.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Tests.Unit.Services.Implementations.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="StaffService"/> class,
/// verifying the interaction with the <see cref="IStaffRepository"/>.
/// </summary>
public class StaffServiceTests
{
    private readonly Mock<IStaffRepository> _staffRepositoryMock;
    private readonly StaffService _serviceUnderTest;
    private readonly List<Staff> _sampleStaff;

    /// <summary>
    /// Initializes a new instance of the <see cref="StaffServiceTests"/> class,
    /// setting up the mock repository and sample staff data for test scenarios.
    /// </summary>
    public StaffServiceTests()
    {
        _staffRepositoryMock = new Mock<IStaffRepository>(MockBehavior.Strict);
        _serviceUnderTest = new StaffService(_staffRepositoryMock.Object);

        _sampleStaff = new List<Staff>
        {
            new Staff(Email.Create("juan.araya@ucr.ac.cr"), 1, "Operations"),
            new Staff(Email.Create("ana.zuniga@ucr.ac.cr"), 2, "Customer Service")
        };
    }

    /// <summary>
    /// Tests the <see cref="StaffService.CreateStaffAsync"/> method to ensure it
    /// returns the result from the repository correctly.
    /// </summary>
    /// <param name="expectedResult">The expected result returned by the repository (true or false).</param>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CreateStaffAsync_Always_ReturnsRepositoryResult(bool expectedResult)
    {
        // Arrange
        var staff = new Staff(Email.Create("carlos.ramirez@ucr.ac.cr"), 3, "Logistics");
        _staffRepositoryMock
            .Setup(repo => repo.CreateStaffAsync(staff))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _serviceUnderTest.CreateStaffAsync(staff);

        // Assert
        result.Should().Be(expectedResult);
    }

    /// <summary>
    /// Verifies that <see cref="StaffService.ListStaffAsync"/> returns a list of staff
    /// when there are staff members available in the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task ListStaffAsync_WhenStaffExist_ReturnsStaffList()
    {
        // Arrange
        _staffRepositoryMock
            .Setup(repo => repo.ListStaffAsync())
            .ReturnsAsync(_sampleStaff);

        // Act
        var result = await _serviceUnderTest.ListStaffAsync();

        // Assert
        result.Should().BeEquivalentTo(_sampleStaff);
    }

    /// <summary>
    /// Verifies that <see cref="StaffService.ListStaffAsync"/> returns an empty list
    /// when no staff members are available in the repository.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task ListStaffAsync_WhenNoStaffExist_ReturnsEmptyList()
    {
        // Arrange
        _staffRepositoryMock
            .Setup(repo => repo.ListStaffAsync())
            .ReturnsAsync(new List<Staff>());

        // Act
        var result = await _serviceUnderTest.ListStaffAsync();

        // Assert
        result.Should().BeEmpty();
    }

    /// <summary>
    /// Verifies that <see cref="StaffService.ListStaffAsync"/> returns null
    /// when the repository returns null (e.g. data source failure or unexpected result).
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Fact]
    public async Task ListStaffAsync_WhenRepositoryReturnsNull_ReturnsNull()
    {
        // Arrange
        _staffRepositoryMock
            .Setup(repo => repo.ListStaffAsync())
            .ReturnsAsync((List<Staff>?)null);

        // Act
        var result = await _serviceUnderTest.ListStaffAsync();

        // Assert
        result.Should().BeNull();
    }
}

