using System.Threading.Tasks;
using Order.Shared.Dto.Users;
using Order.Shared.Contracts;
using Order.Client.Components.Misc;

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
            NotificationModal notificationModal = default(NotificationModal))
        {
            return httpClientService.Post<SignUpDto, SignUpResultDto>(
                "api/user/SignUp",
                userInfo,
                notificationModal);
        }

        public async Task<SignInResultDto> SignIn(
            SignInDto userInfo,
            NotificationModal notificationModal = default(NotificationModal))
        {
            var result = await httpClientService.Post<SignInDto, SignInResultDto>(
                "api/user/SignIn",
                userInfo,
                notificationModal);
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

        public async Task SignOut(NotificationModal notificationModal = default(NotificationModal))
        {
            await httpClientService.Get("api/user/SignOut", notificationModal);
            await authenticationStateProvider.MarkUserAsSignedOut();
        }

        public async Task RefreshTokens(
            RefreshTokensDto refreshToken,
            NotificationModal notificationModal = default(NotificationModal))
        {
            var result = await httpClientService.Post<RefreshTokensDto, TokenPairDto>(
                "api/user/RefreshTokens",
                refreshToken,
                notificationModal);
            if (result is not null)
            {
                try
                {
                    await authenticationStateProvider.MarkUserAsSignedIn(result.AccessToken, result.RefreshToken);
                }
                catch (System.Exception) { }
            }
        }

        public Task<bool> RequestResetPassword(
            RequestResetPasswordDto userEmail,
            NotificationModal notificationModal = default(NotificationModal))
        {
            return httpClientService.Post<RequestResetPasswordDto>(
                "api/user/RequestResetPassword",
                userEmail,
                notificationModal);
        }

        public async Task<ResetPasswordResultDto> ResetPassword(
            ResetPasswordDto password,
            NotificationModal notificationModal)
        {
            return await httpClientService.Post<ResetPasswordDto, ResetPasswordResultDto>(
                "api/user/ResetPassword",
                password,
                notificationModal);
        }

        public async Task<SignInResultDto> ExternalProvidersSignIn(
            ExternalProviderSignInDto provider,
            NotificationModal notificationModal)
        {
            var result = await httpClientService.Post<ExternalProviderSignInDto, SignInResultDto>(
                "api/user/ExternalProviderSignIn",
                provider,
                notificationModal);

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
    }
}
