using System.Threading.Tasks;
using Order.Application.Shared.Dto.Users;

namespace Order.Application.Client.Services
{
    public interface IAuthenticationService
    {
        Task<SignUpResultDto> SignUp(UserSignUpDto userSignUpData);
        Task<SignInResultDto> SignIn(UserSignInDto userSignInData);
        Task SignOut();
    }
}
