using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using MudBlazor.Services;
using System.Security.Claims;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Pages.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Tests.Unit.Pages.AccountManagement;

/// <summary>
/// Unit tests for the UserDetails component.
/// </summary>
public class UserDetailsTests : TestContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserDetailsTests"/> class.
    /// </summary>
    private readonly Task<AuthenticationState> _authState;

    /// Constructor to set up the test context
    /// and mock services required for the UserDetails component.
    public UserDetailsTests()
    {
        Services.AddMudServices();
        JSInterop.SetupVoid("mudElementRef.addOnBlurEvent", _ => true);
        JSInterop.SetupVoid("mudPopover.initialize", _ => true);
        JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        JSInterop.SetupVoid("mudPopover.connect", _ => true);
        JSInterop.Setup<int>("mudpopoverHelper.countProviders", _ => true).SetResult(1);

        // Setup authentication state
        var identity = new ClaimsIdentity(new[]
        {
                new Claim(ClaimTypes.Name, "test@example.com"),
                new Claim(ClaimTypes.NameIdentifier, "123")
            }, "test");
        var user = new ClaimsPrincipal(identity);
        _authState = Task.FromResult(new AuthenticationState(user));

        // Add authorization for testing
        this.AddTestAuthorization().SetAuthorized("test@example.com");

        this.JSInterop.Mode = JSRuntimeMode.Loose;
    }

    /// <summary>
    /// Tests that the UserDetails component renders correctly with no initial data.
    /// </summary>
    [Fact]
    public void ShouldRenderUserDetails_WhenUserExists()
    {
        // Arrange
        var user = new UserWithPerson(
            userName: UserName.Create("ana.sanchez"),
            firstName: "Ana",
            lastName: "Sánchez",
            email: Email.Create("ana@test.com"),
            phone: Phone.Create("8888-8888"),
            identityNumber: IdentityNumber.Create("1-1111-1111"),
            birthDate: BirthDate.Create(new DateOnly(1990, 5, 10)),
            roles: new List<string> { "Admin", "Editor" },
            userId: 1,
            personId: 2
        );

        var userServiceMock = new Mock<IUserWithPersonService>();
        userServiceMock.Setup(u => u.GetAllUserWithPersonAsync()).ReturnsAsync(new List<UserWithPerson> { user });

        var navContext = new UserNavigationContext { SelectedUserId = 1 };
        var permissionMock = new Mock<IPermissionContext>();
        permissionMock.Setup(p => p.HasPermission(It.IsAny<string>())).Returns(true);

        Services.AddSingleton(userServiceMock.Object);
        Services.AddSingleton(navContext);
        Services.AddSingleton<IPermissionContext>(permissionMock.Object);
        Services.AddSingleton<IUserRoleContext>(Mock.Of<IUserRoleContext>());
        Services.AddSingleton<NavigationManager>(Mock.Of<NavigationManager>());
        Services.AddSingleton<ISnackbar>(Mock.Of<ISnackbar>());

        // Act
        var cut = RenderComponent<UserDetails>();

        // Assert
        Assert.Contains("Ana", cut.Markup);
        Assert.Contains("Sánchez", cut.Markup);
        Assert.Contains("ana@test.com", cut.Markup);
        Assert.Contains("8888-8888", cut.Markup);
        Assert.Contains("ana.sanchez", cut.Markup);
        Assert.Contains("Admin, Editor", cut.Markup);
        Assert.Contains("1-1111-1111", cut.Markup);
        Assert.Contains("5/10/1990", cut.Markup);
    }

    /// <summary>
    /// Tests that the UserDetails component shows a loading message when no user is found.
    /// </summary>
    [Fact]
    public void ShouldShowLoading_WhenNoUserFound()
    {
        // Arrange
        var userServiceMock = new Mock<IUserWithPersonService>();
        userServiceMock.Setup(u => u.GetAllUserWithPersonAsync()).ReturnsAsync(new List<UserWithPerson>());

        var navContext = new UserNavigationContext { SelectedUserId = 99 };

        Services.AddSingleton(userServiceMock.Object);
        Services.AddSingleton(navContext);
        Services.AddSingleton<IPermissionContext>(Mock.Of<IPermissionContext>());
        Services.AddSingleton<IUserRoleContext>(Mock.Of<IUserRoleContext>());
        Services.AddSingleton<NavigationManager>(Mock.Of<NavigationManager>());
        Services.AddSingleton<ISnackbar>(Mock.Of<ISnackbar>());

        // Act
        var cut = RenderComponent<UserDetails>();

        // Assert
        Assert.Contains("Loading user information...", cut.Markup);
    }
}
