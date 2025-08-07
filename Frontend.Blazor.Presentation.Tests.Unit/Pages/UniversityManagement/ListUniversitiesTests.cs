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
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Pages.Universities;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Tests.Unit.Pages.UniversityManagement;

/// <summary>
/// Contains unit tests for the <see cref="ListUniversities"/> page.
/// Verifies behavior such as loading universities, rendering results, and navigating to other pages.
/// </summary>
public class ListUniversitiesTests : TestContext
{
    private readonly Mock<IUniversityServices> _universityServiceMock;
    private readonly Mock<IPermissionContext> _permissionContextMock;

    public ListUniversitiesTests()
    {
        Services.AddMudServices();

        _universityServiceMock = new Mock<IUniversityServices>();
        _permissionContextMock = new Mock<IPermissionContext>();

        Services.AddSingleton(_universityServiceMock.Object);
        Services.AddSingleton(_permissionContextMock.Object);

        JSInterop.SetupVoid("mudElementRef.addOnBlurEvent", _ => true);
        JSInterop.SetupVoid("mudPopover.initialize", _ => true);
        JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        JSInterop.SetupVoid("mudPopover.connect", _ => true);
    }

    /// <summary>
    /// Tests that the "AGREGAR" button is rendered when the user has the "Create Universities" permission.
    /// </summary>
    [Fact]
    public void ShouldRenderAddButton_WhenUserHasCreatePermission()
    {
        // Arrange
        _universityServiceMock.Setup(s => s.ListUniversityAsync())
            .ReturnsAsync(new List<University>());

     
        _permissionContextMock.Setup(p => p.HasPermission("Create Universities")).Returns(true);
   
        var auth = this.AddTestAuthorization();
        auth.SetAuthorized("admin");
        auth.SetPolicies("Create Universities");

        // Act
        var cut = RenderComponent<ListUniversities>();

        // Assert
        Assert.Contains("AGREGAR", cut.Markup);
    }

    /// <summary>
    /// Tests that the "AGREGAR" button is not rendered when the user lacks the "Create Universities" permission.
    /// </summary>
    [Fact]
    public void ShouldNotRenderAddButton_WhenUserLacksCreatePermission()
    {
        // Arrange
        _universityServiceMock.Setup(s => s.ListUniversityAsync())
            .ReturnsAsync(new List<University>());

        _permissionContextMock.Setup(p => p.HasPermission("Create Universities")).Returns(false);

        var auth = this.AddTestAuthorization();
        auth.SetAuthorized("testuser");

        // Act
        var cut = RenderComponent<ListUniversities>();

        // Assert
        Assert.DoesNotContain("AGREGAR", cut.Markup);
    }

    /// <summary>
    /// Tests that clicking the "AGREGAR" button navigates to the add university page.
    /// </summary>
    [Fact]
    public void ShouldNavigateToAddUniversity_WhenAddButtonClicked()
    {
        // Arrange
        _universityServiceMock.Setup(s => s.ListUniversityAsync())
            .ReturnsAsync(new List<University>());

        _permissionContextMock.Setup(p => p.HasPermission("Create Universities")).Returns(true);

        var auth = this.AddTestAuthorization();
        auth.SetAuthorized("admin");
        auth.SetPolicies("Create Universities");

        var navMan = Services.GetRequiredService<NavigationManager>() as FakeNavigationManager;

        var cut = RenderComponent<ListUniversities>();

        // Act
        var button = cut.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("AGREGAR"));
        Assert.NotNull(button);
        button!.Click();

        // Assert
        Assert.EndsWith("/universidades/agregar-universidad", navMan!.Uri);
    }

    /// <summary>
    /// Tests that the universities are displayed in a table when data is loaded successfully.
    /// </summary>
    [Fact]
    public void ShouldDisplayUniversitiesInTable_WhenDataLoaded()
    {
        // Arrange
        var universities = new List<University>
        {
            new University(EntityName.Create("UCR"), EntityLocation.Create("Costa Rica"))
        };

        _universityServiceMock.Setup(s => s.ListUniversityAsync())
            .ReturnsAsync(universities);

        var auth = this.AddTestAuthorization();
        auth.SetAuthorized("testuser");

        // Act
        var cut = RenderComponent<ListUniversities>();

        // Assert
        Assert.Contains("UCR", cut.Markup);
        Assert.Contains("Costa Rica", cut.Markup);
        Assert.Contains("/sedes", cut.Markup);
        Assert.Contains("/areas", cut.Markup);
        Assert.Contains("/edificios", cut.Markup);
    }

    /// <summary>
    /// Tests that the "No hay elementos para mostrar" message is displayed when no universities exist.
    /// </summary>
    [Fact]
    public void ShouldShowNoRecordsMessage_WhenNoUniversitiesExist()
    {
        // Arrange
        _universityServiceMock.Setup(s => s.ListUniversityAsync())
            .ReturnsAsync(new List<University>());

        var auth = this.AddTestAuthorization();
        auth.SetAuthorized("testuser");

        // Act
        var cut = RenderComponent<ListUniversities>();

        // Assert
        Assert.Contains("No hay elementos para mostrar", cut.Markup);
    }
}
