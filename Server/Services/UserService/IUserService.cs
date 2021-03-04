using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Order.Server.Dto.Users;
using Order.Shared.Dto.Users;
using Order.Shared.Interfaces;

namespace Order.Server.Services
{
    public interface IUserService : IService
    {
        Task<SignUpResultDto> SignUp(UserSignUpDto userInfo, IUrlHelper url, string scheme);
        Task ConfirmEmail(EmailConfirmationDto confirmation);
        Task<SignInResultDto> SignIn(UserSignInDto userInfo);
        Task SignOut(int userId);
        Task<TokenPairDto> RefreshTokens(string userRefreshToken, int userId, IEnumerable<Claim> claims);
    }
}
