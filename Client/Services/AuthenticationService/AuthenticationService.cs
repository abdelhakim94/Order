using System.Threading.Tasks;
using Order.Shared.Dto.Users;
using Order.Shared.Contracts;
using Order.Client.Components.Misc;
using Order.Shared.Dto;

namespace Order.Client.Services
{
    public class AuthenticationService : IAuthenticationService, IService
    {
        private readonly IHttpClientService httpClientService;
        private readonly IOrderAuthenticationStateProvider authenticationStateProvider;

        public AuthenticationService(
            IHttpClientService httpClientService,
            IOrderAuthenticationStateProvider authenticationStateProvider)
        {
            this.httpClientService = httpClientService;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public Task<SignUpResultDto> SignUp(
            SignUpDto userInfo,
            Toast toast = default(Toast))
        {
            return httpClientService.Post<SignUpDto, SignUpResultDto>(
                "api/user/SignUp",
                userInfo,
                toast);
        }

        public async Task<SignInResultDto> SignIn(
            SignInDto userInfo,
            Toast toast = default(Toast))
        {
            var result = await httpClientService.Post<SignInDto, SignInResultDto>(
                "api/user/SignIn",
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
            await httpClientService.Get("api/user/SignOut", toast);
            await authenticationStateProvider.MarkUserAsSignedOut();
        }

        public async Task RefreshTokens(
            ValueWrapperDto<string> refreshToken,
            Toast toast = default(Toast))
        {
            var result = await httpClientService.Post<ValueWrapperDto<string>, TokenPairDto>(
                "api/user/RefreshTokens",
                refreshToken,
                toast);
            if (result is not null)
            {
                await authenticationStateProvider.MarkUserAsSignedIn(result.AccessToken, result.RefreshToken);
            }
        }

        public Task<bool> RequestResetPassword(
            RequestResetPasswordDto userEmail,
            Toast toast = default(Toast))
        {
            return httpClientService.Post<RequestResetPasswordDto>(
                "api/user/RequestResetPassword",
                userEmail,
                toast);
        }

        public async Task<ResetPasswordResultDto> ResetPassword(
            ResetPasswordDto password,
            Toast toast)
        {
            return await httpClientService.Post<ResetPasswordDto, ResetPasswordResultDto>(
                "api/user/ResetPassword",
                password,
                toast);
        }

        public Task<ValueWrapperDto<string>> GetConsentScreenUrl(
            ValueWrapperDto<string> provider,
            Toast toast)
        {
            return httpClientService.Post<ValueWrapperDto<string>, ValueWrapperDto<string>>(
                "api/user/ExternalProviderSignIn",
                provider,
                toast);
        }

        public Task MarkUserAsSignedIn(string accessToken, string refreshToken)
        {
            return authenticationStateProvider.MarkUserAsSignedIn(accessToken, refreshToken);
        }
    }
}
