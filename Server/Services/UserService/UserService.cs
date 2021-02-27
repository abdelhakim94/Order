using System;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Order.DomainModel;
using Order.Shared.Dto.Users;
using Order.Shared.Security.Claims;
using Order.Server.Services.JwtAuthenticationService;
using Order.Shared.Interfaces;

namespace Order.Server.Services.UserService
{
    public class UserService : IUserService, IService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IJwtAuthenticationService jwtAuthenticationService;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtAuthenticationService jwtAuthenticationService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtAuthenticationService = jwtAuthenticationService;
        }

        public async Task<SignUpResultDto> SignUp(UserSignUpDto userInfo)
        {
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
                var errors = result.Errors.Select(x => x.Code);
                return new SignUpResultDto { Successful = false, Errors = errors };
            }

            var registeredUser = await userManager.FindByEmailAsync(userInfo.Email);
            await userManager.AddClaimsAsync(registeredUser, new Claim[]
            {
                new(ClaimTypes.NameIdentifier, registeredUser.Id.ToString())
            });

            return new SignUpResultDto { Successful = true };
        }

        public async Task<SignInResultDto> SignIn(UserSignInDto userInfo)
        {
            var result = await signInManager.PasswordSignInAsync(
                userInfo.Email,
                userInfo.Password,
                true,
                false);

            var user = await userManager.FindByEmailAsync(userInfo.Email);

            if (!result.Succeeded)
            {
                return new SignInResultDto
                {
                    Successful = false,
                    IsNotAllowed = result.IsNotAllowed, // emil is not confirmed
                    IsLockedOut = result.IsLockedOut, // 5 unsuccessful signin 
                    LockoutEndDate = result.IsLockedOut ? await userManager.GetLockoutEndDateAsync(user) : null,
                    IsEmailOrPasswordIncorrect = !(result.IsNotAllowed || result.IsLockedOut || result.RequiresTwoFactor),
                };
            }

            var claims = user.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue));

            var tokenPair = await jwtAuthenticationService.GenerateTokens(
                user.Id,
                claims,
                DateTime.Now
            );

            return new SignInResultDto
            {
                Successful = true,
                TokenPair = tokenPair,
            };
        }

        public async Task SignOut(int userId)
        {
            await jwtAuthenticationService.DeleteRefreshToken(userId);
            await signInManager.SignOutAsync();
        }

        public Task<TokenPairDto> RefreshTokens(TokenPairDto previousTokens)
        {
            return jwtAuthenticationService.RefreshTokens(previousTokens, DateTime.Now);
        }
    }
}
