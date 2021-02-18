using System.Threading.Tasks;
using Order.Shared.Dto.Users;

namespace Order.Client.Services
{
    public interface IAuthenticationService
    {
        Task<SignUpResultDto> SignUp(UserSignUpDto userSignUpData);
        Task<SignInResultDto> SignIn(UserSignInDto userSignInData);
        Task SignOut();
    }
}
