using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Pages.Universities;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Tests.Unit.Pages.UniversityManagement;

public class ListAreasTests : TestContext
{
    private readonly Mock<IAreaServices> _areaServiceMock;
    private readonly Mock<IPermissionContext> _permissionContextMock;

    public ListAreasTests()
    {
        Services.AddMudServices();

        _areaServiceMock = new Mock<IAreaServices>();
        _permissionContextMock = new Mock<IPermissionContext>();

        Services.AddSingleton(_areaServiceMock.Object);
        Services.AddSingleton(_permissionContextMock.Object);
    }
    /// <summary>
    /// Tests that the add button is rendered when the user has the required permission.
    /// </summary>
    [Fact]
    public void ShouldRenderAddButton_WhenUserHasCreateAreaPermission()
    {
        var auth = this.AddTestAuthorization();
        auth.SetAuthorized("admin");
        auth.SetPolicies("Create Area");

        _areaServiceMock.Setup(s => s.ListAreaAsync())
            .ReturnsAsync(new List<Area>());

        var cut = RenderComponent<ListAreas>();

        Assert.Contains("AGREGAR", cut.Markup);
    }

    /// <summary>
    /// Tests that the add button is not rendered when the user lacks the required permission.
    /// </summary>
    [Fact]
    public void ShouldNotRenderAddButton_WhenUserLacksCreateAreaPermission()
    {
        var auth = this.AddTestAuthorization();
        auth.SetAuthorized("readonlyuser");

        _areaServiceMock.Setup(s => s.ListAreaAsync())
            .ReturnsAsync(new List<Area>());

        var cut = RenderComponent<ListAreas>();

        Assert.DoesNotContain("AGREGAR", cut.Markup);
    }

    /// <summary>
    /// Tests that clicking the "AGREGAR" button navigates to the add area page.
    /// </summary>
    [Fact]
    public void ShouldNavigateToAddArea_WhenAddButtonClicked()
    {
        var auth = this.AddTestAuthorization();
        auth.SetAuthorized("admin");
        auth.SetPolicies("Create Area");

        _areaServiceMock.Setup(s => s.ListAreaAsync())
            .ReturnsAsync(new List<Area>());

        var navMan = Services.GetRequiredService<NavigationManager>() as FakeNavigationManager;

        var cut = RenderComponent<ListAreas>();

        var button = cut.Find("button");
        button.Click();

        Assert.EndsWith("/areas/agregar-area", navMan!.Uri);
    }

    /// <summary>
    /// Tests that the area list is rendered correctly after loading areas.
    /// </summary>
    /// <returns></returns>

    [Fact]
    public async Task ShouldRenderAreaList_AfterLoading()
    {
        var areas = new List<Area>
        {
            new Area
            (
                EntityName.Create("Área1"),
                new Campus
                (
                    EntityName.Create("Sede1"),
                    EntityLocation.Create("Ubicación1"),
                    new University { Name = EntityName.Create("Universidad1") }
                )
            )
        };

        _areaServiceMock.Setup(s => s.ListAreaAsync())
            .ReturnsAsync(areas);

        var auth = this.AddTestAuthorization();
        auth.SetAuthorized("admin");
        auth.SetPolicies("Create Area");

        var cut = RenderComponent<ListAreas>();
        await cut.InvokeAsync(() => Task.CompletedTask);

        Assert.Contains("Área1", cut.Markup);
        Assert.Contains("Sede1", cut.Markup);
        Assert.Contains("Universidad1", cut.Markup);
    }
}
