using System.Threading.Tasks;
using Order.Shared.Dto.Users;
using Order.Shared.Contracts;
using Order.Client.Constants;
using Order.Client.Components.Misc;

namespace Order.Client.Services
{
    public interface IAuthenticationService : IService
    {
        Task<SignUpResultDto> SignUp(SignUpDto userSignUpData, NotificationModal notificationModal = default(NotificationModal));
        Task<SignInResultDto> SignIn(SignInDto userSignInData, NotificationModal notificationModal = default(NotificationModal));
        Task SignOut(NotificationModal notificationModal = default(NotificationModal));

        Task RefreshTokens(RefreshTokensDto refreshToken, NotificationModal notificationModal = default(NotificationModal));

        Task<bool> RequestResetPassword(RequestResetPasswordDto userEmail, NotificationModal notificationModal = default(NotificationModal));
        Task<ResetPasswordResultDto> ResetPassword(ResetPasswordDto password, NotificationModal notificationModal = default(NotificationModal));

        Task<SignInResultDto> ExternalProvidersSignIn(ExternalProviderSignInDto provider, NotificationModal notificationModal = default(NotificationModal));
    }
}
