using ClubManagementSystem.Data.Entities;
using ClubManagementSystem.Extensions;
using ClubManagementSystem.Resources;
using ClubManagementSystem.Theme;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace ClubManagementSystem.Shared
{
    public class MainLayoutBase : LayoutComponentBase
    {
        protected MudThemeProvider MudThemeProvider { get; set; } = null!;
        protected DefaultTheme DefaultTheme { get; set; } = new();
        protected bool IsDrawerOpened { get; set; }
        protected bool IsDarkMode { get; set;}
        protected string? FullName { get; set; }
        protected string? Role { get; set; }
        protected char? FirstLetterOfFirstName { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

        [Inject]
        protected UserManager<User> UserManager { get; set; } = null!;

        [Inject]
        protected IStringLocalizer<Common> Localizer { get; set; } = null!;

        protected void DrawerToggle()
         => IsDrawerOpened = !IsDrawerOpened;

        protected void DarkModeToggle()
         => IsDarkMode = !IsDarkMode;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                IsDarkMode = await MudThemeProvider.GetSystemPreference();
                StateHasChanged();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userId = authState.User.GetUserId();
            var isAdmin = authState.User.IsInRole(Roles.Admin);
            if (userId != null)
            {
                var user = await UserManager.FindByIdAsync(userId);
                FirstLetterOfFirstName = user?.FirstName?[0];
                FullName = user?.FullName;
                Role = isAdmin ? Roles.Admin : string.Empty;
            }
        }
    }
}
