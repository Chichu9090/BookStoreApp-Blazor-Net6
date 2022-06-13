using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services.Authentication
{
    public interface IAuthenticationServices
    {
        Task<bool> AuthenticateAsync(LoginUserDto loginModel);

        public Task Logout();
    }
}
