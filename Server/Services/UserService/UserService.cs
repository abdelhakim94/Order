using System;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Order.DomainModel;
using Order.Shared.Dto.Users;
using Order.Shared.Security.Claims;
using Order.Server.Services.JwtAuthenticationService;
using Order.Shared.Interfaces;
using Order.Server.Services.EmailService;
using Order.Server.Dto.Users;
using Order.Shared.Constants;

namespace Order.Server.Services.UserService
{
    public class UserService : IUserService, IService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IJwtAuthenticationService jwtAuthenticationService;
        private readonly IEmailService emailService;
        private readonly IdentityErrorDescriber errorDescriber;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtAuthenticationService jwtAuthenticationService,
            IEmailService emailService,
            IdentityErrorDescriber errorDescriber)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtAuthenticationService = jwtAuthenticationService;
            this.emailService = emailService;
            this.errorDescriber = errorDescriber;
        }

        public async Task<SignUpResultDto> SignUp(UserSignUpDto userInfo, IUrlHelper url, string scheme)
        {
            if (userInfo.Password != userInfo.ConfirmPassword)
            {
                return new SignUpResultDto { Successful = false, Error = errorDescriber.PasswordMismatch().Code };
            }

            var newUser = new User
            {
                UserName = userInfo.Email,
                Email = userInfo.Email,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                TwoFactorEnabled = false,
                Claims = {
                    new UserClaim { ClaimType = ClaimTypes.Email, ClaimValue = userInfo.Email },
                    new UserClaim { ClaimType = nameof(Profile), ClaimValue = nameof(Profile.GUEST) }
                },
            };

            var result = await userManager.CreateAsync(newUser, userInfo.Password);
            if (!result.Succeeded)
            {
                var error = result.Errors.Select(x => x.Code).FirstOrDefault();
                return new SignUpResultDto { Successful = false, Error = error };
            }

            try
            {
                var registeredUser = await userManager.FindByEmailAsync(userInfo.Email);
                await userManager.AddClaimsAsync(registeredUser, new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, registeredUser.Id.ToString())
                });

                var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(registeredUser);
                var confirmationUrl = url.Action(
                    "ConfirmEmail",
                    "User",
                    new EmailConfirmationDto
                    {
                        confirmationToken = confirmationToken,
                        userEmail = registeredUser.Email,
                    },
                    scheme);

                await emailService.SendEmailConfirmationMail(registeredUser.Email, confirmationUrl);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.StackTrace);
                await userManager.DeleteAsync(newUser);
                return new SignUpResultDto { Successful = false, Error = SignUpErrors.FailureSendingEmail };
            }

            return new SignUpResultDto { Successful = true };
        }

        public async Task ConfirmEmail(EmailConfirmationDto confirmation)
        {
            var user = await userManager.FindByEmailAsync(confirmation.userEmail);
            await userManager.ConfirmEmailAsync(user, confirmation.confirmationToken);
        }

        public async Task<SignInResultDto> SignIn(UserSignInDto userInfo)
        {
            var result = await signInManager.PasswordSignInAsync(
                userInfo.Email,
                userInfo.Password,
                true,
                true);

            var user = await userManager.FindByEmailAsync(userInfo.Email);

            if (!result.Succeeded)
            {
                return new SignInResultDto
                {
                    Successful = false,
                    IsNotAllowed = result.IsNotAllowed, // email is not confirmed
                    IsLockedOut = result.IsLockedOut, // 5 unsuccessful signin 
                    LockoutEndDate = result.IsLockedOut ? await userManager.GetLockoutEndDateAsync(user) : null,
                    IsEmailOrPasswordIncorrect = !(result.IsNotAllowed || result.IsLockedOut || result.RequiresTwoFactor),
                };
            }

            var claims = user.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue));

            var tokenPairAsync = jwtAuthenticationService.GenerateTokens(
                user.Id,
                claims,
                DateTime.Now
            );

            return new SignInResultDto
            {
                Successful = true,
                TokenPair = await tokenPairAsync,
            };
        }

        public async Task SignOut(int userId)
        {
            await jwtAuthenticationService.DeleteRefreshToken(userId);
            await signInManager.SignOutAsync();
        }

        public async Task<TokenPairDto> RefreshTokens(string userRefreshToken, int userId, IEnumerable<Claim> claims)
        {
            try
            {
                return await jwtAuthenticationService.RefreshTokens(userRefreshToken, userId, claims, DateTime.Now);
            }
            catch (System.Exception)
            {
                await this.SignOut(userId);
                throw;
            }
        }
    }
}
