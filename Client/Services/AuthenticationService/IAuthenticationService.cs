using System.Threading.Tasks;
using Order.Shared.Dto.Users;
using Order.Shared.Contracts;
using Order.Client.Components.Misc;
using Order.Shared.Dto;

namespace Order.Client.Services
{
    public interface IAuthenticationService : IService
    {
        Task<SignUpResultDto> SignUp(SignUpDto userSignUpData, NotificationModal notificationModal = default(NotificationModal));
        Task<SignInResultDto> SignIn(SignInDto userSignInData, NotificationModal notificationModal = default(NotificationModal));
        Task SignOut(NotificationModal notificationModal = default(NotificationModal));

        Task RefreshTokens(ValueWrapperDto<string> refreshToken, NotificationModal notificationModal = default(NotificationModal));

        Task<bool> RequestResetPassword(RequestResetPasswordDto userEmail, NotificationModal notificationModal = default(NotificationModal));
        Task<ResetPasswordResultDto> ResetPassword(ResetPasswordDto password, NotificationModal notificationModal = default(NotificationModal));

        Task<ValueWrapperDto<string>> GetConsentScreenUrl(ValueWrapperDto<string> provider, NotificationModal notificationModal = default(NotificationModal));

        Task MarkUserAsSignedIn(string accessToken, string refreshToken);
    }
}
