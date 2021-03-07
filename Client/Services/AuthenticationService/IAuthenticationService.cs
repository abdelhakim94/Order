using System.Threading.Tasks;
using Order.Shared.Dto.Users;
using Order.Shared.Contracts;
using Order.Client.Constants;

namespace Order.Client.Services
{
    public interface IAuthenticationService : IService
    {
        Task<SignUpResultDto> SignUp(SignUpDto userSignUpData);
        Task<SignInResultDto> SignIn(SignInDto userSignInData);
        Task SignOut();

        Task RefreshTokens(RefreshTokensDto refreshToken);

        Task<string> RequestResetPassword(RequestResetPasswordDto userEmail);
        Task<ResetPasswordResultDto> ResetPassword(ResetPasswordDto password);
    }
}
