using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Pages.Universities;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Tests.Unit.Pages.UniversityManagement;
public class ListCampusTests : TestContext
{
    private readonly Mock<ICampusServices> _campusServiceMock;
    private readonly Mock<IPermissionContext> _permissionContextMock;
    public ListCampusTests()
    {
        Services.AddMudServices();

        _campusServiceMock = new Mock<ICampusServices>();
        Services.AddSingleton(_campusServiceMock.Object);
        _permissionContextMock = new Mock<IPermissionContext>();

        Services.AddSingleton(_permissionContextMock.Object);
    }

    /// <summary>
    /// Tests that the add button is rendered when the user has the required permission.
    /// </summary>
    [Fact]
    public void ShouldRenderAddButton_WhenUserHasCreateCampusPermission()
    {
        // Arrange
        var auth = this.AddTestAuthorization();
        auth.SetAuthorized("admin");
        auth.SetPolicies("Create Campus");

        _permissionContextMock.Setup(p => p.HasPermission("Create Campus"))
            .Returns(true);
        _campusServiceMock.Setup(s => s.ListCampusAsync())
            .ReturnsAsync(new List<Campus>()); 

        // Act
        var cut = RenderComponent<ListCampus>();

        // Assert
        Assert.Contains("AGREGAR", cut.Markup);
    }

    /// <summary>
    /// Tests that the add button is not rendered when the user lacks the required permission.
    /// </summary>

    [Fact]
    public void ShouldNotRenderAddButton_WhenUserLacksCreateCampusPermission()
    {
        // Arrange
        var auth = this.AddTestAuthorization();
        auth.SetAuthorized("readonlyuser");

        _campusServiceMock.Setup(s => s.ListCampusAsync())
            .ReturnsAsync(new List<Campus>());

        // Act
        var cut = RenderComponent<ListCampus>();

        // Assert
        Assert.DoesNotContain("AGREGAR", cut.Markup);
    }

    /// <summary>
    /// Tests that clicking the "AGREGAR" button navigates to the add campus page.
    /// </summary>
    [Fact]
    public void ShouldNavigateToAddCampus_WhenAddButtonClicked()
    {
        // Arrange
        var auth = this.AddTestAuthorization();
        auth.SetAuthorized("admin");
        auth.SetPolicies("Create Campus");

        _campusServiceMock.Setup(s => s.ListCampusAsync())
            .ReturnsAsync(new List<Campus>());

        var navMan = Services.GetRequiredService<NavigationManager>() as FakeNavigationManager;

        var cut = RenderComponent<ListCampus>();

        // Act
        var button = cut.Find("button"); 
        button.Click();

        // Assert
        Assert.EndsWith("/sedes/agregar-sede", navMan.Uri);
    }
    /// <summary>
    /// Tests that the campus list is rendered correctly after loading data.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task ShouldRenderCampusList_AfterLoading()
    {
        // Arrange
        var campuses = new List<Campus>
        {
            new Campus
            (
                EntityName.Create("Sede1"),
                EntityLocation.Create("Ubicación1"),
                new University { Name = EntityName.Create("Universidad1") }
            )
        };

        _campusServiceMock.Setup(s => s.ListCampusAsync())
            .ReturnsAsync(campuses);

        var auth = this.AddTestAuthorization();
        auth.SetAuthorized("admin");
        auth.SetPolicies("Create Campus");

        // Act
        var cut = RenderComponent<ListCampus>();

        await cut.InvokeAsync(() => Task.CompletedTask);

        // Assert the campus name is rendered
        Assert.Contains("Sede1", cut.Markup);
        Assert.Contains("Ubicación1", cut.Markup);
        Assert.Contains("Universidad1", cut.Markup);
    }
}
