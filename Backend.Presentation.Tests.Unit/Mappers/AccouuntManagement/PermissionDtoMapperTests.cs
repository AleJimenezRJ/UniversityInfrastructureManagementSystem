using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="PermissionDtoMapper"/> class,  verifying the correct mapping of <see
/// cref="Permission"/> objects to <see cref="PermissionDto"/> objects.
/// </summary>
public class PermissionDtoMapperTests
{
    private static readonly Permission SamplePermission = new("ViewDashboard") { Id = 1 };
    private static readonly PermissionDto SampleDto = new(1, "ViewDashboard");

    /// <summary>
    /// Tests that the <see cref="PermissionDtoMapper.ToDto"/> method correctly maps the ID of a permission.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapIdCorrectly()
    {
        var dto = PermissionDtoMapper.ToDto(SamplePermission);
        dto.Id.Should().Be(1);
    }

    /// <summary>
    /// Tests that the <see cref="PermissionDtoMapper.ToDto"/> method correctly maps the description of a permission.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapDescriptionCorrectly()
    {
        var dto = PermissionDtoMapper.ToDto(SamplePermission);
        dto.Description.Should().Be("ViewDashboard");
    }

    /// <summary>
    /// Verifies that the <see cref="PermissionDtoMapper.ToDtoList"/> method returns a list of DTOs with the correct
    /// count.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldReturnCorrectCount()
    {
        var permissions = new List<Permission>
        {
            new("ViewDashboard") { Id = 1 },
            new("EditUsers") { Id = 2 },
            new("DeleteRecords") { Id = 3 }
        };

        var dtos = PermissionDtoMapper.ToDtoList(permissions);

        dtos.Should().HaveCount(3);
    }

    /// <summary>
    /// Tests that the <see cref="PermissionDtoMapper.ToDtoList"/> method correctly maps the first permission's ID.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapFirstPermissionIdCorrectly()
    {
        var permissions = new List<Permission>
        {
            new("ViewDashboard") { Id = 1 }
        };

        var dto = PermissionDtoMapper.ToDtoList(permissions)[0];

        dto.Id.Should().Be(1);
    }

    /// <summary>
    /// Tests that the <see cref="PermissionDtoMapper.ToDtoList"/> method correctly maps the description of the first
    /// <see cref="Permission"/> object in the list to the corresponding DTO.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapFirstPermissionDescriptionCorrectly()
    {
        var permissions = new List<Permission>
        {
            new("ViewDashboard") { Id = 1 }
        };

        var dto = PermissionDtoMapper.ToDtoList(permissions)[0];

        dto.Description.Should().Be("ViewDashboard");
    }

    /// <summary>
    /// Tests that the <see cref="PermissionDtoMapper.ToDtoList"/> method correctly maps the second permission's ID.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapSecondPermissionIdCorrectly()
    {
        var permissions = new List<Permission>
        {
            new("ViewDashboard") { Id = 1 },
            new("EditUsers") { Id = 2 }
        };

        var dto = PermissionDtoMapper.ToDtoList(permissions)[1];

        dto.Id.Should().Be(2);
    }

    /// <summary>
    /// Tests that the <see cref="PermissionDtoMapper.ToDtoList"/> method correctly maps the description of the second
    /// permission.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapSecondPermissionDescriptionCorrectly()
    {
        var permissions = new List<Permission>
        {
            new("ViewDashboard") { Id = 1 },
            new("EditUsers") { Id = 2 }
        };

        var dto = PermissionDtoMapper.ToDtoList(permissions)[1];

        dto.Description.Should().Be("EditUsers");
    }

    /// <summary>
    /// Tests that the <see cref="PermissionDtoMapper.ToDtoList"/> method correctly maps the third permission's ID.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapThirdPermissionIdCorrectly()
    {
        var permissions = new List<Permission>
        {
            new("ViewDashboard") { Id = 1 },
            new("EditUsers") { Id = 2 },
            new("DeleteRecords") { Id = 3 }
        };

        var dto = PermissionDtoMapper.ToDtoList(permissions)[2];

        dto.Id.Should().Be(3);
    }

    /// <summary>
    /// Tests that the third permission in the list is correctly mapped to its description.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapThirdPermissionDescriptionCorrectly()
    {
        var permissions = new List<Permission>
        {
            new("ViewDashboard") { Id = 1 },
            new("EditUsers") { Id = 2 },
            new("DeleteRecords") { Id = 3 }
        };

        var dto = PermissionDtoMapper.ToDtoList(permissions)[2];

        dto.Description.Should().Be("DeleteRecords");
    }

    /// <summary>
    /// Verifies that <see cref="PermissionDtoMapper.ToDtoList"/> returns an empty list when the input list is empty.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldReturnEmptyList_WhenInputIsEmpty()
    {
        var emptyPermissions = new List<Permission>();
        var dtos = PermissionDtoMapper.ToDtoList(emptyPermissions);
        dtos.Should().BeEmpty();
    }
}
