using System.Threading.Tasks;
using Order.Shared.Dto.Users;
using Order.Shared.Interfaces;

namespace Order.Client.Services
{
    public interface IAuthenticationService : IService
    {
        Task<SignUpResultDto> SignUp(UserSignUpDto userSignUpData);
        Task<SignInResultDto> SignIn(UserSignInDto userSignInData);
        Task SignOut();
        Task RefreshTokens(string refreshToken);
        Task<bool> RequestRecoverPassword(string userEmail);
        Task<RecoverPasswordResultDto> RecoverPassword(RecoverPasswordDto password);
    }
}
