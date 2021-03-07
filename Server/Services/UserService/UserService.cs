using System;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using MediatR;
using Order.DomainModel;
using Order.Shared.Dto.Users;
using Order.Shared.Security.Claims;
using Order.Server.Services.JwtAuthenticationService;
using Order.Shared.Contracts;
using Order.Server.Services.EmailService;
using Order.Server.Dto.Users;
using Order.Shared.Constants;
using Order.Server.Exceptions;

namespace Order.Server.Services.UserService
{
    public class UserService : IUserService, IService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IJwtAuthenticationService jwtAuthenticationService;
        private readonly IEmailService emailService;
        private readonly IdentityErrorDescriber errorDescriber;
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtAuthenticationService jwtAuthenticationService,
            IEmailService emailService,
            IdentityErrorDescriber errorDescriber,
            IConfiguration configuration,
            IMediator mediator)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtAuthenticationService = jwtAuthenticationService;
            this.emailService = emailService;
            this.errorDescriber = errorDescriber;
            this.configuration = configuration;
            this.mediator = mediator;
        }

        public async Task<SignUpResultDto> SignUp(SignUpDto userInfo, IUrlHelper urlHelper, string scheme)
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

            var registeredUser = await userManager.FindByEmailAsync(userInfo.Email);
            await userManager.AddClaimsAsync(registeredUser, new Claim[]
            {
                    new(ClaimTypes.NameIdentifier, registeredUser.Id.ToString())
            });

            var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(registeredUser);
            var confirmationUrl = urlHelper.Action(
                "ConfirmEmail",
                "User",
                new EmailConfirmationDto
                {
                    ConfirmationToken = confirmationToken,
                    UserEmail = registeredUser.Email,
                },
                scheme);
            try
            {
                await emailService.SendEmailConfirmationMail(registeredUser.Email, confirmationUrl);
            }
            catch (System.Exception)
            {
                await userManager.DeleteAsync(newUser);
                return new SignUpResultDto { Successful = false, Error = SignUpErrors.FailureSendingEmail };
            }

            return new SignUpResultDto { Successful = true };
        }

        public async Task ConfirmEmail(EmailConfirmationDto confirmation, IUrlHelper urlHelper, string scheme)
        {
            if (string.IsNullOrWhiteSpace(confirmation.UserEmail))
            {
                throw new BadRequestException("L'adresse email fourni est invalide");
            }

            var user = await userManager.FindByEmailAsync(confirmation.UserEmail);
            if (user is null)
            {
                throw new NotFoundException("L'adresse email fourni ne correspond à aucun compte!");
            }

            var result = await userManager.ConfirmEmailAsync(user, confirmation.ConfirmationToken);
            if (!result.Succeeded)
            {
                var didTokenExpire = result.Errors
                    .Select(e => e.Code)
                    .Contains(errorDescriber.InvalidToken().Code);

                if (didTokenExpire)
                {
                    var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationUrl = urlHelper.Action(
                        "ConfirmEmail",
                        "User",
                        new EmailConfirmationDto
                        {
                            ConfirmationToken = confirmationToken,
                            UserEmail = user.Email,
                        },
                        scheme);
                    await emailService.ReSendEmailConfirmationMail(user.Email, confirmationUrl);
                }
                else
                {
                    throw new Exception("L'adresse email n'a pas pu être confirmé!");
                }
            }
        }

        public async Task<SignInResultDto> SignIn(SignInDto userInfo)
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

        public async Task<TokenPairDto> RefreshTokens(string refreshToken, int userId, IEnumerable<Claim> claims)
        {
            try
            {
                return await jwtAuthenticationService.RefreshTokens(refreshToken, userId, claims, DateTime.Now);
            }
            catch (System.Exception)
            {
                await this.SignOut(userId);
                throw;
            }
        }

        public async Task RequestResetPassword(RequestResetPasswordDto request, IUrlHelper url, string scheme)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                throw new BadRequestException("User not found");
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var resetPwUrl = url.Action(
                "RedirectToResetPassword",
                "User",
                new RequestResetPasswordTokenDto
                {
                    UserEmail = user.Email,
                    ResetPasswordToken = token,
                },
                scheme);

            await emailService.SendResetPasswordMail(user.Email, resetPwUrl);
        }

        public async Task<ResetPasswordResultDto> ResetPassword(
            ResetPasswordDto resetPwInfo,
            IUrlHelper urlHelper,
            string scheme)
        {

            if (resetPwInfo.Password != resetPwInfo.ConfirmPassword)
            {
                return new ResetPasswordResultDto
                {
                    Successful = false,
                    Error = errorDescriber.PasswordMismatch().Code
                };
            }

            var user = await userManager.FindByEmailAsync(resetPwInfo.Email);
            var result = await userManager.ResetPasswordAsync(
                user,
                resetPwInfo.ResetToken,
                resetPwInfo.Password);

            if (!result.Succeeded)
            {
                var didTokenExpire = result.Errors
                .Select(e => e.Code)
                .Contains(errorDescriber.InvalidToken().Code);

                if (didTokenExpire)
                {
                    var resetPwToken = await userManager.GeneratePasswordResetTokenAsync(user);
                    var resetPwUrl = urlHelper.Action(
                        "RedirectToResetPassword",
                        "User",
                        new RequestResetPasswordTokenDto
                        {
                            UserEmail = user.Email,
                            ResetPasswordToken = resetPwToken,
                        },
                        scheme);

                    try
                    {
                        await emailService.ReSendResetPasswordMail(user.Email, resetPwUrl);
                    }
                    catch (System.Exception)
                    {
                        return new ResetPasswordResultDto
                        {
                            Successful = false,
                            Error = SignUpErrors.FailureSendingEmail,
                        };
                    }

                    return new ResetPasswordResultDto
                    {
                        Successful = false,
                        Error = errorDescriber.InvalidToken().Code
                    };
                }

                return new ResetPasswordResultDto
                {
                    Successful = false,
                    Error = result.Errors.Select(e => e.Code).FirstOrDefault(),
                };
            }

            return new ResetPasswordResultDto { Successful = true };
        }
    }
}
