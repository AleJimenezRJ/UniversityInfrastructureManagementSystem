using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using MudBlazor.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Application.Services.Implementations;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.Entities.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Domain.ValueObjects.AccountManagement;
using UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Pages.AccountManagement;

namespace UCR.ECCI.PI.ThemePark.Frontend.Blazor.Presentation.Tests.Unit.Pages.AccountManagement;

/// <summary>
/// Contains unit tests for the <see cref="UserManagement"/> Blazor page.
/// Uses bUnit and Moq to verify navigation and service interactions for user management features.
/// </summary>
public class UserManagementTests : TestContext
{
    /// <summary>
    /// Mocked service for user and person management operations.
    /// </summary>
    private readonly Mock<IUserWithPersonService> _userServiceMock;

    /// <summary>
    /// Stores the last navigation URI for test verification.
    /// </summary>
    private string _navigatedTo = "";

    /// <summary>
    /// Initializes a new instance of the <see cref="UserManagementTests"/> class.
    /// Sets up required services, JSInterop, and authorization context for testing.
    /// </summary>
    public UserManagementTests()
    {
        Services.AddMudServices();

        _userServiceMock = new Mock<IUserWithPersonService>();
        Services.AddSingleton(_userServiceMock.Object);
        Services.AddSingleton<UserNavigationContext>();

        JSInterop.SetupVoid("mudElementRef.addOnBlurEvent", _ => true);
        JSInterop.SetupVoid("mudPopover.initialize", _ => true);
        JSInterop.SetupVoid("mudKeyInterceptor.connect", _ => true);
        JSInterop.SetupVoid("mudPopover.connect", _ => true);
        JSInterop.Setup<int>("mudpopoverHelper.countProviders", _ => true).SetResult(1);

        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("testuser");
    }

    /// <summary>
    /// Verifies that clicking the "AGREGAR" button navigates to the add user page.
    /// </summary>
    [Fact]
    public void ShouldNavigateToAddUser_WhenAddButtonClicked()
    {
        // Arrange
        _userServiceMock.Setup(s => s.GetPaginatedUsersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                        .ReturnsAsync(new PaginatedList<UserWithPerson>(new List<UserWithPerson>(), 0, 10, 0));

        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("testuser");
        authContext.SetPolicies("Create Users");

        var cut = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();

            builder.OpenComponent<UserManagement>(1);
            builder.CloseComponent();
        });

        // Act
        var button = cut.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("AGREGAR"));
        Assert.NotNull(button);
        button.Click();

        // Assert
        var navMan = Services.GetRequiredService<NavigationManager>() as FakeNavigationManager;
        Assert.NotNull(navMan);
        Assert.EndsWith("/add-user", navMan.Uri);
    }

    /// <summary>
    /// Verifies that clicking the "BITÁCORA" button navigates to the user audit page.
    /// </summary>
    [Fact]
    public void ShouldNavigateToAuditPage_WhenAuditButtonClicked()
    {
        // Arrange
        _userServiceMock.Setup(s => s.GetPaginatedUsersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                        .ReturnsAsync(new PaginatedList<UserWithPerson>(new List<UserWithPerson>(), 0, 10, 0));

        var authContext = this.AddTestAuthorization();
        authContext.SetAuthorized("testuser");
        authContext.SetPolicies("View Audit");

        var cut = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();

            builder.OpenComponent<UserManagement>(1);
            builder.CloseComponent();
        });

        // Act
        var button = cut.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("BITÁCORA"));
        Assert.NotNull(button);
        button.Click();

        // Assert
        var navMan = Services.GetRequiredService<NavigationManager>() as FakeNavigationManager;
        Assert.NotNull(navMan);
        Assert.EndsWith("/user-audit", navMan.Uri);
    }

    /// <summary>
    /// Verifies that when the user service returns no users, the UserManagement page displays the "No hay elementos para mostrar" message.
    /// </summary>
    [Fact]
    public void ShouldDisplayNoRecordsMessage_WhenNoUsersReturned()
    {
        // Arrange
        _userServiceMock.Setup(s => s.GetPaginatedUsersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                        .ReturnsAsync(new PaginatedList<UserWithPerson>(new List<UserWithPerson>(), 0, 10, 0));

        var cut = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();

            builder.OpenComponent<UserManagement>(1);
            builder.CloseComponent();
        });

        // Assert
        Assert.Contains("No hay elementos para mostrar", cut.Markup);
    }

    /// <summary>
    /// Verifies that when the user service returns a list of users, the UserManagement page displays the user information in the table.
    /// Checks for the presence of user details such as full name, username, identity number, email, and role in the rendered markup.
    /// </summary>
    [Fact]
    public void ShouldDisplayUsersInTable_WhenUsersAreReturned()
    {
        // Arrange
        var users = new List<UserWithPerson>
        {
            new UserWithPerson(
                userName: UserName.Create("jdoe"),
                firstName: "John",
                lastName: "Doe",
                email: Email.Create("john.doe@example.com"),
                phone: Phone.Create("8888-8888"),
                identityNumber: IdentityNumber.Create("1-2345-6789"),
                birthDate: null!,
                roles: new List<string> { "Admin" },
                userId: 1,
                personId: 1)
        };

        _userServiceMock.Setup(s => s.GetPaginatedUsersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                        .ReturnsAsync(new PaginatedList<UserWithPerson>(users, users.Count, 10, 1));

        var cut = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();

            builder.OpenComponent<UserManagement>(1);
            builder.CloseComponent();
        });

        // Assert
        Assert.Contains("John Doe", cut.Markup);
        Assert.Contains("jdoe", cut.Markup);
        Assert.Contains("1-2345-6789", cut.Markup);
        Assert.Contains("john.doe@example.com", cut.Markup);
        Assert.Contains("Admin", cut.Markup);
    }

    /// <summary>
    /// Verifies that clicking on a user's username in the UserManagement table sets the selected user ID in the navigation context
    /// and navigates to the user details page.
    /// </summary>
    [Fact]
    public void ShouldNavigateToUserDetails_WhenUserNameClicked()
    {
        // Arrange
        var users = new List<UserWithPerson>
        {
            new UserWithPerson(
                userName: UserName.Create("jdoe"),
                firstName: "John",
                lastName: "Doe",
                email: Email.Create("john.doe@example.com"),
                phone: Phone.Create("8888-8888"),
                identityNumber: IdentityNumber.Create("1-2345-6789"),
                birthDate: null!,
                roles: new List<string> { "Admin" },
                userId: 42,
                personId: 1)
        };

        _userServiceMock.Setup(s => s.GetPaginatedUsersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                        .ReturnsAsync(new PaginatedList<UserWithPerson>(users, users.Count, 10, 1));

        var navContext = Services.GetRequiredService<UserNavigationContext>();

        var cut = Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();

            builder.OpenComponent<UserManagement>(1);
            builder.CloseComponent();
        });

        // Act
        var userNameElement = cut.Find("span.mud-link");
        userNameElement.Click();

        // Assert
        Assert.Equal(42, navContext.SelectedUserId);

        var navMan = Services.GetRequiredService<NavigationManager>() as FakeNavigationManager;
        Assert.EndsWith("/user-details", navMan!.Uri);
    }
}
