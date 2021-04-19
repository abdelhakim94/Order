using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Order.Server.Dto.Users;
using Order.Shared.Dto.Users;
using Order.Shared.Contracts;

namespace Order.Server.Services
{
    public interface IAccountService : IScopedService
    {
        Task<SignUpResultDto> SignUp(
            SignUpDto userInfo,
            Func<object, string> emailConfirmationUrlBuilder);

        Task ConfirmEmail(
            EmailConfirmationDto confirmation,
            Func<object, string> emailConfirmationUrlBuilder);

        Task<SignInResultDto> SignIn(SignInDto userInfo);

        Task<TokenPairDto> RefreshTokens(
            string userRefreshToken,
            int userId,
            IEnumerable<Claim> claims);

        Task RequestResetPassword(
            RequestResetPasswordDto request,
            Func<object, string> resetPasswordUrlBuilder);

        Task<ResetPasswordResultDto> ResetPassword(
            ResetPasswordDto resetPwInfo,
            Func<object, string> resetPasswordUrlBuilder);

        Task SignOut(int userId);

        AuthenticationProperties ConfigureSignInWithExternalProvider(
            string provider,
            string redirectUrl);

        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();

        Task<SignInResultDto> ExternalLoginSignInAsync(
            string provider,
            string providerKey);

        Task<SignInResultDto> HandleFirstExternalSignIn(
            string userEmail,
            ExternalLoginInfo info,
            Func<object, string> emailConfirmationUrlBuilder);

        Task ConfirmExternalProviderAssociation(ConfirmExternalProviderAssociationDto info);
    }
}
