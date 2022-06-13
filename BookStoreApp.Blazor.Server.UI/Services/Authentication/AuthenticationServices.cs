using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.UI.Providers;
using BookStoreApp.Blazor.Server.UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.Server.UI.Services.Authentication
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly IClient client;
        private readonly ILocalStorageService localStorage;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public AuthenticationServices(IClient client, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
        {
            this.client = client;
            this.localStorage = localStorage;
            this.authenticationStateProvider = authenticationStateProvider;
        }
        public async Task<bool> AuthenticateAsync(LoginUserDto loginModel)
        {
            var response = await client.LoginAsync(loginModel);

            //store token

            await localStorage.SetItemAsync("accessToken", response.Token);

            //Change auth state of an app

            await ((ApiAuthenticationStateProvider)authenticationStateProvider).LoggedIn();

            return true;
        }

        public async Task Logout()
        {
            await ((ApiAuthenticationStateProvider)authenticationStateProvider).LoggedOut();
        }
    }
}
