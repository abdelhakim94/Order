using System.Threading.Tasks;
using Order.Client.Components;
using Order.Shared.Contracts;
using Order.Shared.Dto;
using Order.Shared.Dto.Account;

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
