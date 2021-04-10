using System.Threading.Tasks;
using Order.Shared.Dto.Users;
using Order.Shared.Contracts;
using Order.Client.Components.Misc;
using Order.Shared.Dto;

namespace Order.Client.Services
{
    public interface IAuthenticationService : IScopedService
    {
        Task<SignUpResultDto> SignUp(SignUpDto userSignUpData, Toast toast = default(Toast));
        Task<SignInResultDto> SignIn(SignInDto userSignInData, Toast toast = default(Toast));
        Task SignOut(Toast toast = default(Toast));

        Task RefreshTokens(ValueWrapperDto<string> refreshToken, Toast toast = default(Toast));

        Task<bool> RequestResetPassword(RequestResetPasswordDto userEmail, Toast toast = default(Toast));
        Task<ResetPasswordResultDto> ResetPassword(ResetPasswordDto password, Toast toast = default(Toast));

        Task<ValueWrapperDto<string>> GetConsentScreenUrl(ValueWrapperDto<string> provider, Toast toast = default(Toast));

        Task MarkUserAsSignedIn(string accessToken, string refreshToken);
    }
}
