using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Order.Server.Dto.Users;
using Order.Shared.Dto.Users;
using Order.Shared.Contracts;

namespace Order.Server.Services
{
    public interface IUserService : IService
    {
        Task<SignUpResultDto> SignUp(SignUpDto userInfo, IUrlHelper url, string scheme);
        Task ConfirmEmail(EmailConfirmationDto confirmation, IUrlHelper url, string scheme);

        Task<SignInResultDto> SignIn(SignInDto userInfo);
        Task<TokenPairDto> RefreshTokens(string userRefreshToken, int userId, IEnumerable<Claim> claims);
        Task RequestResetPassword(RequestResetPasswordDto request, IUrlHelper url, string scheme);
        Task<ResetPasswordResultDto> ResetPassword(ResetPasswordDto resetPwInfo, IUrlHelper url, string scheme);

        Task SignOut(int userId);

        AuthenticationProperties ConfigureSignInWithExternalProvider(string provider, string redirectUrl);
    }
}
