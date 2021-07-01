using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Order.Client.Components;
using Order.Shared.Contracts;
using Order.Shared.Dto;
using Order.Shared.Dto.Account;

namespace Order.Client.Services
{
    public class AuthenticationService : IAuthenticationService, IScopedService
    {
        private readonly IHttpClientService httpClientService;
        private readonly IOrderAuthenticationStateProvider authenticationStateProvider;
        private readonly NavigationManager navigationManager;

        public AuthenticationService(
            IHttpClientService httpClientService,
            IOrderAuthenticationStateProvider authenticationStateProvider,
            NavigationManager navigationManager)
        {
            this.httpClientService = httpClientService;
            this.authenticationStateProvider = authenticationStateProvider;
            this.navigationManager = navigationManager;
        }

        public Task<SignUpResultDto> SignUp(
            SignUpDto userInfo,
            Toast toast = default(Toast))
        {
            return httpClientService.Post<SignUpDto, SignUpResultDto>(
                "api/account/SignUp",
                userInfo,
                toast);
        }

        public async Task<SignInResultDto> SignIn(
            SignInDto userInfo,
            Toast toast = default(Toast))
        {
            var result = await httpClientService.Post<SignInDto, SignInResultDto>(
                "api/account/SignIn",
                userInfo,
                toast);
            if (result is not null && result.Successful)
            {
                try
                {
                    await authenticationStateProvider.MarkUserAsSignedIn(
                        result.TokenPair.AccessToken,
                        result.TokenPair.RefreshToken);
                }
                catch (System.Exception)
                {
                    result.Successful = false;
                }
            }
            return result;
        }

        public async Task SignOut(Toast toast = default(Toast))
        {
            await httpClientService.Get("api/account/SignOut", toast);
            await authenticationStateProvider.MarkUserAsSignedOut();
        }

        public async Task RefreshTokens(
            ValueWrapperDto<string> refreshToken,
            Toast toast = default(Toast))
        {
            var result = await httpClientService.Post<ValueWrapperDto<string>, TokenPairDto>(
                "api/account/RefreshTokens",
                refreshToken,
                toast);
            if (result is not null)
            {
                try
                {
                    await authenticationStateProvider.MarkUserAsSignedIn(result.AccessToken, result.RefreshToken);
                }
                catch (System.Exception)
                {
                    await authenticationStateProvider.MarkUserAsSignedOut();
                    navigationManager.NavigateTo("SignIn/");
                }
            }
        }

        public Task<bool> RequestResetPassword(
            RequestResetPasswordDto userEmail,
            Toast toast = default(Toast))
        {
            return httpClientService.Post<RequestResetPasswordDto>(
                "api/account/RequestResetPassword",
                userEmail,
                toast);
        }

        public async Task<ResetPasswordResultDto> ResetPassword(
            ResetPasswordDto password,
            Toast toast)
        {
            return await httpClientService.Post<ResetPasswordDto, ResetPasswordResultDto>(
                "api/account/ResetPassword",
                password,
                toast);
        }

        public Task<ValueWrapperDto<string>> GetConsentScreenUrl(
            ValueWrapperDto<string> provider,
            Toast toast)
        {
            return httpClientService.Post<ValueWrapperDto<string>, ValueWrapperDto<string>>(
                "api/account/ExternalProviderSignIn",
                provider,
                toast);
        }

        public Task MarkUserAsSignedIn(string accessToken, string refreshToken)
        {
            return authenticationStateProvider.MarkUserAsSignedIn(accessToken, refreshToken);
        }
    }
}
