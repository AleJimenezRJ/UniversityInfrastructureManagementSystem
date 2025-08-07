using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Pages.Buildings;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Tests.Unit.Pages.UniversityManagement;

/// <summary>
/// Contains unit tests for the <see cref="ListBuildings"/> page.
/// </summary>
public class ListBuildingsTests : TestContext
{
    private readonly Mock<IBuildingsServices> _buildingServiceMock;
    private readonly Mock<IPermissionContext> _permissionContextMock;

    public ListBuildingsTests()
    {
        Services.AddMudServices();

        _buildingServiceMock = new Mock<IBuildingsServices>();
        _permissionContextMock = new Mock<IPermissionContext>();

        Services.AddSingleton(_buildingServiceMock.Object);
        Services.AddSingleton(_permissionContextMock.Object);

        JSInterop.SetupVoid("mudElementRef.addOnBlurEvent", _ => true);
        JSInterop.SetupVoid("mudPopover.initialize", _ => true);
        JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        JSInterop.SetupVoid("mudPopover.connect", _ => true);
    }

    /// <summary>
    /// Tests that the add button is rendered when the user has the required permission.
    /// </summary>
    [Fact]
    public void ShouldRenderAddButton_WhenUserHasCreatePermission()
    {
        // Arrange
        _permissionContextMock.Setup(p => p.HasPermission("Create Buildings")).Returns(true);

        var auth = this.AddTestAuthorization();
        auth.SetAuthorized("admin");
        auth.SetPolicies("Create Buildings");

        // Act
        var cut = RenderComponent<ListBuildings>();

        // Assert
        Assert.Contains("AGREGAR", cut.Markup);
    }

    /// <summary>
    /// Tests that the add button is not rendered when the user lacks the required permission.
    /// </summary>
    [Fact]
    public void ShouldNotRenderAddButton_WhenUserLacksCreatePermission()
    {
        _permissionContextMock.Setup(p => p.HasPermission("Create Buildings")).Returns(false);

        var auth = this.AddTestAuthorization();
        auth.SetAuthorized("readonlyuser");

        var cut = RenderComponent<ListBuildings>();

        Assert.DoesNotContain("AGREGAR", cut.Markup);
    }

    /// <summary>
    /// Tests that the add button is navigating to the correct page when clicked.
    /// </summary>
    [Fact]
    public void ShouldNavigateToAddBuilding_WhenAddButtonClicked()
    {
        _permissionContextMock.Setup(p => p.HasPermission("Create Buildings")).Returns(true);

        var auth = this.AddTestAuthorization();
        auth.SetAuthorized("admin");
        auth.SetPolicies("Create Buildings");

        var navMan = Services.GetRequiredService<NavigationManager>() as FakeNavigationManager;

        var cut = RenderComponent<ListBuildings>();

        var button = cut.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("AGREGAR"));
        Assert.NotNull(button);
        button!.Click();

        Assert.EndsWith("/edificios/agregar-edificio", navMan!.Uri);
    }


    /// <summary>
    /// Tests that the Bitacora button is not rendered when the user lacks the required permission.
    /// </summary>
    [Fact]
    public void ShouldNotRenderBitacoraButton_WhenUserLacksBitacoraPermission()
    {
        // Arrange
        _permissionContextMock.Setup(p => p.HasPermission("List Buildings")).Returns(false);

        var auth = this.AddTestAuthorization();
        auth.SetAuthorized("readonlyuser");
        // No policy set here because user lacks permission

        // Act
        var cut = RenderComponent<ListBuildings>();

        // Assert
        Assert.DoesNotContain("BITÁCORA", cut.Markup);
    }

}
