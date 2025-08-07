using FluentAssertions;
using UCR.ECCI.PI.ThemePark.Backend.Domain.Entities.ComponentsManagement;

namespace UCR.ECCI.PI.ThemePark.Backend.Domain.Tests.Unit.Entities.ComponentsManagement;

/// <summary>
/// Unit tests for the <see cref="LearningComponentAudit"/> entity,
/// with each property tested in isolation.
/// </summary>
public class LearningComponentAuditTests
{
    private readonly LearningComponentAudit _audit;

    public LearningComponentAuditTests()
    {
        _audit = new LearningComponentAudit
        {
            LearningComponentAuditId = 1,
            ComponentId = 99,
            Width = 2.5m,
            Height = 1.5m,
            Depth = 0.5m,
            X = 1.1m,
            Y = 2.2m,
            Z = 3.3m,
            Orientation = "North",
            IsDeleted = true,
            ComponentType = "Projector",
            MarkerColor = "Red",
            ProjectedContent = "Math Class",
            ProjectedHeight = 1.2m,
            ProjectedWidth = 2.4m,
            Action = "Updated",
            ModifiedAt = new DateTime(2024, 5, 20, 10, 30, 0)
        };
    }

    [Fact]
    public void LearningComponentAuditId_ShouldBeAssignedCorrectly()
        => _audit.LearningComponentAuditId.Should().Be(1);

    [Fact]
    public void ComponentId_ShouldBeAssignedCorrectly()
        => _audit.ComponentId.Should().Be(99);

    [Fact]
    public void Width_ShouldBeAssignedCorrectly()
        => _audit.Width.Should().Be(2.5m);

    [Fact]
    public void Height_ShouldBeAssignedCorrectly()
        => _audit.Height.Should().Be(1.5m);

    [Fact]
    public void Depth_ShouldBeAssignedCorrectly()
        => _audit.Depth.Should().Be(0.5m);

    [Fact]
    public void X_ShouldBeAssignedCorrectly()
        => _audit.X.Should().Be(1.1m);

    [Fact]
    public void Y_ShouldBeAssignedCorrectly()
        => _audit.Y.Should().Be(2.2m);

    [Fact]
    public void Z_ShouldBeAssignedCorrectly()
        => _audit.Z.Should().Be(3.3m);

    [Fact]
    public void Orientation_ShouldBeAssignedCorrectly()
        => _audit.Orientation.Should().Be("North");

    [Fact]
    public void IsDeleted_ShouldBeAssignedCorrectly()
        => _audit.IsDeleted.Should().BeTrue();

    [Fact]
    public void ComponentType_ShouldBeAssignedCorrectly()
        => _audit.ComponentType.Should().Be("Projector");

    [Fact]
    public void MarkerColor_ShouldBeAssignedCorrectly()
        => _audit.MarkerColor.Should().Be("Red");

    [Fact]
    public void ProjectedContent_ShouldBeAssignedCorrectly()
        => _audit.ProjectedContent.Should().Be("Math Class");

    [Fact]
    public void ProjectedHeight_ShouldBeAssignedCorrectly()
        => _audit.ProjectedHeight.Should().Be(1.2m);

    [Fact]
    public void ProjectedWidth_ShouldBeAssignedCorrectly()
        => _audit.ProjectedWidth.Should().Be(2.4m);

    [Fact]
    public void Action_ShouldBeAssignedCorrectly()
        => _audit.Action.Should().Be("Updated");

    [Fact]
    public void ModifiedAt_ShouldBeAssignedCorrectly()
        => _audit.ModifiedAt.Should().Be(new DateTime(2024, 5, 20, 10, 30, 0));
}
