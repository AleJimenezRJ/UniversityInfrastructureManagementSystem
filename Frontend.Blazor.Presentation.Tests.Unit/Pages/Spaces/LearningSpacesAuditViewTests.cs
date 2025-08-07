using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using MudBlazor.Services;
using System.Reflection;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Dtos.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.Spaces;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.UniversityManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Components;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Pages.Spaces;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Tests.Unit.Pages.Spaces;

/// <summary>
/// Tests for the ListLearningSpacesAuditView component.
/// </summary>
public class LearningSpacesAuditViewTests : TestContext
{
    /// <summary>
    /// Mock for the building services.
    /// </summary>
    private readonly Mock<IBuildingsServices> _buildingServiceMock;

    /// <summary>
    /// Mock for the floor services.
    /// </summary>
    private readonly Mock<IFloorServices> _floorServiceMock;

    /// <summary>
    /// Mock for the learning spaces log services.
    /// </summary>
    private readonly Mock<ILearningSpaceLogServices> _learningSpacesLogServiceMock;

    /// <summary>
    /// Mock for the permission context.
    /// </summary>
    private readonly Mock<IPermissionContext> _permissionContextMock;

    /// <summary>
    /// Initializes a new instance of the <see cref="LearningSpacesAuditViewTests"/> class.
    /// </summary>
    public LearningSpacesAuditViewTests()
    {
        // Set up the test context with necessary services and mocks
        _buildingServiceMock = new Mock<IBuildingsServices>();
        _floorServiceMock = new Mock<IFloorServices>();
        _learningSpacesLogServiceMock = new Mock<ILearningSpaceLogServices>();
        _permissionContextMock = new Mock<IPermissionContext>();
        // Register services and mocks
        Services.AddMudServices();
        Services.AddSingleton(_buildingServiceMock.Object);
        Services.AddSingleton(_floorServiceMock.Object);
        Services.AddSingleton(_learningSpacesLogServiceMock.Object);
        Services.AddSingleton(_permissionContextMock.Object);
        // Set up authentication and authorization
        var authPolicyProvider = new Mock<Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider>();
        Services.AddSingleton(authPolicyProvider.Object);
        var authService = new Mock<Microsoft.AspNetCore.Authorization.IAuthorizationService>();
        Services.AddSingleton(authService.Object);
        // Set up JSInterop for MudBlazor components
        JSInterop.Mode = JSRuntimeMode.Loose;
        JSInterop.SetupVoid("mudPopover.initialize", _ => true);
        JSInterop.SetupVoid("mudElementRef.addOnBlurEvent", _ => true);
        JSInterop.SetupVoid("mudPopover.connect", _ => true);
        JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        JSInterop.SetupVoid("mudPopover.disconnect", _ => true);
        JSInterop.SetupVoid("mudPopover.update", _ => true);
    }

    /// <summary>
    /// Tests that the ListLearningSpacesAuditView component renders basic elements correctly.
    /// </summary>
    [Fact]
    public void Component_Renders_Basic_Elements()
    {
        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("testuser");

        var cut = Render(builder =>
        {
            builder.OpenComponent<CascadingAuthenticationState>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder =>
            {
                childBuilder.OpenComponent<MudPopoverProvider>(0);
                childBuilder.CloseComponent();

                childBuilder.OpenComponent<LearningSpacesAuditView>(1);
                childBuilder.AddAttribute(2, "buildingId", 1);
                childBuilder.AddAttribute(3, "floorId", 1);
                childBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        });

        Assert.Contains("Historial de Auditoría de Espacios de Aprendizaje", cut.Markup);
    }

    /// <summary>
    /// Tests that the ListLearningSpacesAuditView component shows an error message when the building is invalid.
    /// </summary>
    [Fact]
    public void Shows_Error_Message_When_Invalid_Building()
    {
        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("testuser");

        var cut = Render(builder =>
        {
            builder.OpenComponent<CascadingAuthenticationState>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder =>
            {
                childBuilder.OpenComponent<MudPopoverProvider>(0);
                childBuilder.CloseComponent();

                childBuilder.OpenComponent<LearningSpacesAuditView>(1);
                childBuilder.AddAttribute(2, "buildingId", 1);
                childBuilder.AddAttribute(3, "floorId", 1);
                childBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        });

        var listLearningSpaces = cut.FindComponent<LearningSpacesAuditView>().Instance;
        listLearningSpaces.GetType().GetField("_invalidBuilding", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(listLearningSpaces, true);
        listLearningSpaces.GetType().GetField("UserMessage", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(listLearningSpaces, "No se encontró el edificio solicitado.");
        Assert.Contains("No se encontró el edificio solicitado.", cut.Markup);
    }

    /// <summary>
    /// Tests that the ListLearningSpacesAuditView component shows an error message when the floor is invalid.
    /// </summary>
    [Fact]
    public void Shows_Error_Message_When_Invalid_Floor()
    {
        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("testuser");

        var cut = Render(builder =>
        {
            builder.OpenComponent<CascadingAuthenticationState>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder =>
            {
                childBuilder.OpenComponent<MudPopoverProvider>(0);
                childBuilder.CloseComponent();

                childBuilder.OpenComponent<LearningSpacesAuditView>(1);
                childBuilder.AddAttribute(2, "buildingId", 1);
                childBuilder.AddAttribute(3, "floorId", 1);
                childBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        });

        var listLearningSpaces = cut.FindComponent<LearningSpacesAuditView>().Instance;
        listLearningSpaces.GetType().GetField("_invalidFloor", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(listLearningSpaces, true);
        listLearningSpaces.GetType().GetField("UserMessage", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(listLearningSpaces, "No se encontró el piso solicitado.");

        cut.FindComponent<LearningSpacesAuditView>().Render();

        Assert.Contains("No se encontró el piso solicitado.", cut.Markup);
    }

    /// <summary>
    /// Verifies that clicking the back button navigates to the list of learning spaces for a specific building and
    /// floor.
    /// </summary>
    [Fact]
    public async Task Back_Button_Navigates_To_LearningSpaces_List()
    {
        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("testuser");

        var navMan = Services.GetRequiredService<FakeNavigationManager>();

        int buildingId = 1;
        int floorId = 2;

        _buildingServiceMock
            .Setup(x => x.DisplayBuildingAsync(It.IsAny<int>()))
            .ReturnsAsync(new Building(buildingId, new EntityName("Edificio A"), null, null, null, null));

        _floorServiceMock
            .Setup(x => x.GetFloorsListAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<Floor> { new Floor(floorId, FloorNumber.Create(floorId)) });

        _learningSpacesLogServiceMock
            .Setup(x => x.ListLearningSpaceLogsAsync())
            .ReturnsAsync(new List<LearningSpaceLog>());

        var cut = Render(builder =>
        {
            builder.OpenComponent<CascadingAuthenticationState>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder =>
            {
                childBuilder.OpenComponent<MudPopoverProvider>(0);
                childBuilder.CloseComponent();

                childBuilder.OpenComponent<LearningSpacesAuditView>(1);
                childBuilder.AddAttribute(2, "buildingId", buildingId);
                childBuilder.AddAttribute(3, "floorId", floorId);
                childBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        });

        var customButtons = cut.FindComponents<CustomButton>();
        Assert.NotEmpty(customButtons);
        var customButton = customButtons.Last();
        Assert.NotNull(customButton);

        await cut.InvokeAsync(() => customButton.Instance.OnClick.InvokeAsync(null));

        var actualPath = navMan.Uri.Replace(navMan.BaseUri, "");
        if (!actualPath.StartsWith("/"))
            actualPath = "/" + actualPath;

        Assert.Equal($"/edificios/{buildingId}/pisos/{floorId}/espacios-de-aprendizaje", actualPath);
    }

    /// <summary>
    /// Tests that the ListLearningSpacesAuditView component displays a message when there are no logs to show.
    /// </summary>
    [Fact]
    public void Shows_No_Elements_Message_When_No_Logs()
    {
        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("testuser");

        _buildingServiceMock
            .Setup(x => x.DisplayBuildingAsync(It.IsAny<int>()))
            .ReturnsAsync(new Building(1, new EntityName("Edificio A"), null, null, null, null));

        _floorServiceMock
            .Setup(x => x.GetFloorsListAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<Floor> { new Floor(1, FloorNumber.Create(1)) });

        _learningSpacesLogServiceMock
            .Setup(x => x.ListLearningSpaceLogsAsync())
            .ReturnsAsync(new List<LearningSpaceLog>());

        var cut = Render(builder =>
        {
            builder.OpenComponent<CascadingAuthenticationState>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder =>
            {
                childBuilder.OpenComponent<MudPopoverProvider>(0);
                childBuilder.CloseComponent();

                childBuilder.OpenComponent<LearningSpacesAuditView>(1);
                childBuilder.AddAttribute(2, "buildingId", 1);
                childBuilder.AddAttribute(3, "floorId", 1);
                childBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        });

        Assert.Contains("No hay elementos para mostrar.", cut.Markup);
    }
}
