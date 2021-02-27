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
    }
}
