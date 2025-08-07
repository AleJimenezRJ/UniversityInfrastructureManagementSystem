using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Dtos.AccountManagement;
using UCR.ECCI.PI.ThemePark.Backend.Presentation.Mappers.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Presentation.Tests.Unit.Mappers.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="UserAuditDtoMapper"/> class, verifying the correct mapping of <see
/// cref="UserAudit"/> objects to their corresponding DTO representations.
/// </summary>
public class UserAuditDtoMapperTests
{
    /// <summary>
    /// Represents a sample user audit record for testing purposes.
    /// </summary>
    private static readonly UserAudit SampleAudit = new UserAudit
    {
        AuditId = 1,
        UserName = "testuser",
        FirstName = "Test",
        LastName = "User",
        Email = "test.user@example.com",
        Phone = "7000-1234",
        IdentityNumber = "1-1111-1111",
        BirthDate = new DateTime(1990, 1, 1),
        ModifiedAt = DateTime.UtcNow,
        Action = "Created"
    };

    /// <summary>
    /// Tests that the <see cref="UserAuditDtoMapper.ToDto"/> method correctly maps the <c>AuditId</c> property from the
    /// source object.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapAuditIdCorrectly()
    {
        var dto = UserAuditDtoMapper.ToDto(SampleAudit);
        dto.AuditId.Should().Be(SampleAudit.AuditId);
    }

    /// <summary>
    /// Tests that the <see cref="UserAuditDtoMapper.ToDto"/> method correctly maps the UserName property.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapUserNameCorrectly()
    {
        var dto = UserAuditDtoMapper.ToDto(SampleAudit);
        dto.UserName.Should().Be(SampleAudit.UserName);
    }

    /// <summary>
    /// Tests that the <see cref="UserAuditDtoMapper.ToDto"/> method correctly maps the <c>FirstName</c> property from
    /// the source object.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapFirstNameCorrectly()
    {
        var dto = UserAuditDtoMapper.ToDto(SampleAudit);
        dto.FirstName.Should().Be(SampleAudit.FirstName);
    }

    /// <summary>
    /// Verifies that the <see cref="UserAuditDtoMapper.ToDto"/> method correctly maps the last name from the source
    /// object.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapLastNameCorrectly()
    {
        var dto = UserAuditDtoMapper.ToDto(SampleAudit);
        dto.LastName.Should().Be(SampleAudit.LastName);
    }

    /// <summary>
    /// Tests that the <see cref="UserAuditDtoMapper.ToDto"/> method correctly maps the email property from the source
    /// object.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapEmailCorrectly()
    {
        var dto = UserAuditDtoMapper.ToDto(SampleAudit);
        dto.Email.Should().Be(SampleAudit.Email);
    }

    /// <summary>
    /// Tests that the <see cref="UserAuditDtoMapper.ToDto"/> method correctly maps the phone number from the source
    /// object.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapPhoneCorrectly()
    {
        var dto = UserAuditDtoMapper.ToDto(SampleAudit);
        dto.Phone.Should().Be(SampleAudit.Phone);
    }

    /// <summary>
    /// Tests that the <see cref="UserAuditDtoMapper.ToDto"/> method correctly maps the identity number from the source
    /// object.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapIdentityNumberCorrectly()
    {
        var dto = UserAuditDtoMapper.ToDto(SampleAudit);
        dto.IdentityNumber.Should().Be(SampleAudit.IdentityNumber);
    }

    /// <summary>
    /// Verifies that the <see cref="UserAuditDtoMapper.ToDto"/> method correctly maps the <c>BirthDate</c> property
    /// from the source object.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapBirthDateCorrectly()
    {
        var dto = UserAuditDtoMapper.ToDto(SampleAudit);
        dto.BirthDate.Should().Be(SampleAudit.BirthDate);
    }

    /// <summary>
    /// Tests that the <see cref="UserAuditDtoMapper.ToDto"/> method correctly maps the <c>ModifiedAt</c> property.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapModifiedAtCorrectly()
    {
        var dto = UserAuditDtoMapper.ToDto(SampleAudit);
        dto.ModifiedAt.Should().BeCloseTo(SampleAudit.ModifiedAt, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Tests that the <see cref="UserAuditDtoMapper.ToDto"/> method correctly maps the action property from a <see
    /// cref="SampleAudit"/> object to a <see cref="UserAuditDto"/>.
    /// </summary>
    [Fact]
    public void ToDto_ShouldMapActionCorrectly()
    {
        var dto = UserAuditDtoMapper.ToDto(SampleAudit);
        dto.Action.Should().Be(SampleAudit.Action);
    }

    /// <summary>
    /// Tests that the <see cref="UserAuditDtoMapper.ToDtoList"/> method returns a list with the correct count of DTOs.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldReturnCorrectCount()
    {
        var auditList = new List<UserAudit> { SampleAudit };
        var dtoList = UserAuditDtoMapper.ToDtoList(auditList);

        dtoList.Should().HaveCount(1);
    }

    /// <summary>
    /// Tests that the <see cref="UserAuditDtoMapper.ToDtoList"/> method correctly maps the  <c>AuditId</c> of the first
    /// <see cref="UserAudit"/> object to the corresponding  <c>AuditId</c> in the resulting DTO list.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapFirstAuditIdCorrectly()
    {
        var auditList = new List<UserAudit> { SampleAudit };
        var dtoList = UserAuditDtoMapper.ToDtoList(auditList);

        dtoList[0].AuditId.Should().Be(SampleAudit.AuditId);
    }

    /// <summary>
    /// Tests that the <see cref="UserAuditDtoMapper.ToDtoList"/> method correctly maps the first action from a list of
    /// <see cref="UserAudit"/> objects to a list of DTOs.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldMapFirstActionCorrectly()
    {
        var auditList = new List<UserAudit> { SampleAudit };
        var dtoList = UserAuditDtoMapper.ToDtoList(auditList);

        dtoList[0].Action.Should().Be(SampleAudit.Action);
    }

    /// <summary>
    /// Verifies that the <see cref="UserAuditDtoMapper.ToDtoList"/> method returns an empty list when the input list is
    /// empty.
    /// </summary>
    [Fact]
    public void ToDtoList_ShouldReturnEmptyList_WhenInputIsEmpty()
    {
        var emptyList = new List<UserAudit>();
        var dtoList = UserAuditDtoMapper.ToDtoList(emptyList);

        dtoList.Should().BeEmpty();
    }
}
