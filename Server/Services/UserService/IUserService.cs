using System;
using System.Threading.Tasks;
using Order.Shared.Dto.Users;
using Order.Shared.Interfaces;

namespace Order.Server.Services
{
    public interface IUserService : IService
    {
        Task<SignUpResultDto> SignUp(UserSignUpDto userInfo);
        Task<SignInResultDto> SignIn(UserSignInDto userInfo);
        Task SignOut(int userId);
        Task<TokenPairDto> RefreshTokens(TokenPairDto previousTokens);
    }
}
