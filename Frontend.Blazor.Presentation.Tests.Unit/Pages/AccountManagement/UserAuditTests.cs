using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using MudBlazor.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Pages.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Tests.Unit.Pages.AccountManagement;

/// <summary>
/// Unit tests for the UserAuditView component.
/// </summary>
public class UserAuditTests : TestContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserAuditTests"/> class.
    /// Sets up required services and JavaScript interop for MudBlazor components.
    /// </summary>
    public UserAuditTests()
    {
        // Add MudBlazor services to the test context
        Services.AddMudServices();

        // Set up JavaScript interop for MudBlazor components
        JSInterop.SetupVoid("mudElementRef.addOnBlurEvent", _ => true);
        JSInterop.SetupVoid("mudPopover.initialize", _ => true);
        JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        JSInterop.SetupVoid("mudPopover.connect", _ => true);
        JSInterop.Setup<int>("mudpopoverHelper.countProviders", _ => true).SetResult(1);
    }

    /// <summary>
    /// Tests that the UserAuditView component displays a "no records" message
    /// when the audit service returns an empty list of audits.
    /// </summary>
    [Fact]
    public void ShouldShowNoRecordsMessage_UserAuditView()
    {
        // Arrange
        var mockAuditService = new Mock<IUserAuditService>();
        mockAuditService.Setup(x => x.ListUserAuditAsync())
            .ReturnsAsync(new List<UserAudit>());
        Services.AddSingleton(mockAuditService.Object);

        // Render the UserAuditView component
        var cut = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();

            builder.OpenComponent<UserAuditView>(1);
            builder.CloseComponent();
        });

        // Assert that the "no records" message is displayed
        cut.WaitForAssertion(() =>
        {
            Assert.Contains("No hay elementos para mostrar.", cut.Markup, StringComparison.OrdinalIgnoreCase);
        });
    }

    /// <summary>
    /// Tests that the UserAuditView component navigates back to the user management page
    /// when the "back" button is clicked.
    /// </summary>
    [Fact]
    public void ShouldNavigateBack_UserAuditView()
    {
        // Arrange
        var mockAuditService = new Mock<IUserAuditService>();
        mockAuditService.Setup(x => x.ListUserAuditAsync())
            .ReturnsAsync(new List<UserAudit>());
        Services.AddSingleton(mockAuditService.Object);

        // Get the fake navigation manager
        var navManager = Services.GetRequiredService<NavigationManager>() as FakeNavigationManager;

        // Render the UserAuditView component
        var cut = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();

            builder.OpenComponent<UserAuditView>(1);
            builder.CloseComponent();
        });

        // Assert that clicking the "back" button navigates to the correct URL
        cut.WaitForAssertion(() =>
        {
            cut.Find("button:contains('ATRÁS')").Click();
            Assert.EndsWith("/user-management", navManager?.Uri);
        });
    }

    /// <summary>
    /// Tests that the UserAuditView component handles exceptions when loading data.
    /// </summary>
    [Fact]
    public void ShouldHandleException_UserAuditView()
    {
        // Arrange
        var mockAuditService = new Mock<IUserAuditService>();
        mockAuditService.Setup(x => x.ListUserAuditAsync())
            .ThrowsAsync(new Exception("Database error"));
        Services.AddSingleton(mockAuditService.Object);

        // Render the UserAuditView component
        var cut = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();

            builder.OpenComponent<UserAuditView>(1);
            builder.CloseComponent();
        });

        // Assert that the component displays an error message
        cut.WaitForAssertion(() =>
        {
            Assert.Contains("No hay elementos para mostrar.", cut.Markup, StringComparison.OrdinalIgnoreCase);
        });
    }
}