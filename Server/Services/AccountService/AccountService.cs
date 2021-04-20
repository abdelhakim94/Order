using System;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using MediatR;
using Order.Shared.Dto.Account;
using Order.Shared.Security.Claims;
using Order.Server.Services.JwtAuthenticationService;
using Order.Shared.Contracts;
using Order.Server.Services.EmailService;
using Order.Server.Dto.Users;
using Order.Server.Middlewares;
using Order.Server.CQRS.Account.Commands;

namespace Order.Server.Services
{
    public class AccountService : IAccountService, IScopedService
    {
        private readonly UserManager<DomainModel.User> userManager;
        private readonly SignInManager<DomainModel.User> signInManager;
        private readonly IJwtAuthenticationService jwtAuthenticationService;
        private readonly IEmailService emailService;
        private readonly IdentityErrorDescriber errorDescriber;
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;

        public AccountService(
            UserManager<DomainModel.User> userManager,
            SignInManager<DomainModel.User> signInManager,
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

        public async Task<SignUpResultDto> SignUp(SignUpDto userInfo, Func<object, string> emailConfirmationUrlBuilder)
        {
            if (userInfo.Password != userInfo.ConfirmPassword)
            {
                return new SignUpResultDto { Successful = false, Error = errorDescriber.PasswordMismatch().Code };
            }

            var newUser = new DomainModel.User
            {
                UserName = userInfo.Email,
                Email = userInfo.Email,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
                TwoFactorEnabled = false,
                Claims = {
                    new DomainModel.UserClaim { ClaimType = ClaimTypes.Email, ClaimValue = userInfo.Email },
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

                var success = await mediator.Send(new AssociateToProfileCommand(registeredUser.Id, Profile.GUEST));
                if (!success)
                {
                    throw new ApplicationException("Le bon profile n'a pas pu être associé. Veuillez réessayer plus tard ou contacter le support.");
                }

                var additionalClaims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, registeredUser.Id.ToString()),
                    new(nameof(Profile), nameof(Profile.GUEST)),
                };

                result = await userManager.AddClaimsAsync(registeredUser, additionalClaims);
                if (!result.Succeeded)
                {
                    throw new ApplicationException("Le compte n'a pas pu être configuré. Veuillez réessayer plus tard ou contacter le support.");
                }

                var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(registeredUser);
                var confirmationUrl = emailConfirmationUrlBuilder(new EmailConfirmationDto
                {
                    ConfirmationToken = confirmationToken,
                    UserEmail = registeredUser.Email,
                });

                await emailService.SendEmailConfirmationMail(registeredUser.Email, confirmationUrl);

            }
            catch (System.ApplicationException)
            {
                await userManager.DeleteAsync(newUser);
                throw;
            }
            catch (System.Exception)
            {
                await userManager.DeleteAsync(newUser);
                throw new ApplicationException("Le compte n'a pas pu être configuré. Veuillez réessayer plus tard ou contacter le support.");
            }

            return new SignUpResultDto { Successful = true };
        }

        public async Task ConfirmEmail(EmailConfirmationDto confirmation, Func<object, string> emailConfirmationUrlBuilder)
        {
            if (string.IsNullOrWhiteSpace(confirmation.UserEmail))
            {
                throw new BadRequestException("L'adresse email fourni est invalide");
            }

            var user = await userManager.FindByEmailAsync(confirmation.UserEmail);
            if (user is null)
            {
                throw new NotFoundException("L'adresse email fourni ne correspond à aucun utilisateur!");
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
                    var confirmationUrl = emailConfirmationUrlBuilder(new EmailConfirmationDto
                    {
                        ConfirmationToken = confirmationToken,
                        UserEmail = user.Email,
                    });

                    try
                    {
                        await emailService.ReSendEmailConfirmationMail(user.Email, confirmationUrl);
                    }
                    catch (System.Exception)
                    {
                        throw new ApplicationException("Le serveur n'a pas pu envoyer le lien de confirmation à l'e-mail fourni. Veuillez réessayer plus tard ou contacter le support.");
                    }
                }
                else
                {
                    throw new ApplicationException("L'adresse email n'a pas pu être confirmé!");
                }
            }
        }

        public async Task<SignInResultDto> SignIn(SignInDto userInfo)
        {
            if (string.IsNullOrWhiteSpace(userInfo.Password))
            {
                return new SignInResultDto
                {
                    Successful = false,
                    IsEmailOrPasswordIncorrect = true,
                };
            }

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

        public async Task RequestResetPassword(RequestResetPasswordDto request, Func<object, string> resetPasswordUrlBuilder)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                throw new BadRequestException("L'email fournit ne correspond à aucun utilisateur");
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var resetPwUrl = resetPasswordUrlBuilder(new RequestResetPasswordTokenDto
            {
                UserEmail = user.Email,
                ResetPasswordToken = token,
            });
            try
            {
                await emailService.SendResetPasswordMail(user.Email, resetPwUrl);
            }
            catch (System.Exception)
            {
                throw new ApplicationException("Le serveur n'a pas pu envoyer le lien de confirmation à l'e-mail fourni. Veuillez réessayer plus tard ou contacter le support.");
            }
        }

        public async Task<ResetPasswordResultDto> ResetPassword(
            ResetPasswordDto resetPwInfo,
            Func<object, string> resetPasswordUrlBuilder)
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
                    var resetPwUrl = resetPasswordUrlBuilder(new RequestResetPasswordTokenDto
                    {
                        UserEmail = user.Email,
                        ResetPasswordToken = resetPwToken,
                    });

                    try
                    {
                        await emailService.ReSendResetPasswordMail(user.Email, resetPwUrl);
                    }
                    catch (System.Exception)
                    {
                        throw new ApplicationException("Le serveur n'a pas pu envoyer le lien de confirmation à l'e-mail fourni. Veuillez réessayer plus tard ou contacter le support.");
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

        // ===================================== External identity providers ========================================= //

        public AuthenticationProperties ConfigureSignInWithExternalProvider(string provider, string redirectUrl)
        {
            return signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        public Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return signInManager.GetExternalLoginInfoAsync();
        }

        public async Task<SignInResultDto> ExternalLoginSignInAsync(string provider, string providerKey)
        {
            var result = await signInManager.ExternalLoginSignInAsync(provider, providerKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                var user = await userManager.FindByLoginAsync(provider, providerKey);
                if (user is not null)
                {
                    var tokenPair = await jwtAuthenticationService.GenerateTokens(
                        user.Id,
                        await userManager.GetClaimsAsync(user),
                        DateTime.Now
                    );

                    return new SignInResultDto
                    {
                        Successful = true,
                        TokenPair = tokenPair
                    };
                }
            }

            return new SignInResultDto { Successful = false };
        }

        public async Task<SignInResultDto> HandleFirstExternalSignIn(
            string userEmail,
            ExternalLoginInfo info,
            Func<object, string> emailConfirmationUrlBuilder)
        {
            var existingUser = await userManager.FindByEmailAsync(userEmail);
            if (existingUser is null)
            {
                var newUser = new DomainModel.User
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                    LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                    EmailConfirmed = true,
                    TwoFactorEnabled = false,
                    Claims = {
                        new DomainModel.UserClaim { ClaimType = ClaimTypes.Email, ClaimValue = userEmail },
                        new DomainModel.UserClaim { ClaimType = nameof(Profile), ClaimValue = nameof(Profile.GUEST) }
                    },
                };

                var createResult = await userManager.CreateAsync(newUser);
                if (!createResult.Succeeded)
                {
                    var error = createResult.Errors.Select(x => x.Code).FirstOrDefault();
                    throw new ApplicationException($"La connection à travers {info.ProviderDisplayName} a échouer avec le code \"{error}\".");
                }

                existingUser = await userManager.FindByEmailAsync(userEmail);
                await userManager.AddClaimsAsync(existingUser, new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, existingUser.Id.ToString())
                });
            }

            if (!existingUser.EmailConfirmed)
            {
                var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(existingUser);
                var confirmationUrl = emailConfirmationUrlBuilder(new ConfirmExternalProviderAssociationDto
                {
                    UserEmail = existingUser.Email,
                    ConfirmationToken = confirmationToken,
                    LoginProvider = info.LoginProvider,
                    ProviderDisplayName = info.ProviderDisplayName,
                    ProviderKey = info.ProviderKey
                });

                try
                {
                    await emailService.SendExternalProviderEmailConfirmationEmail(
                        existingUser.Email,
                        confirmationUrl,
                        info.ProviderDisplayName);
                }
                catch (System.Exception)
                {
                    throw new ApplicationException("Le serveur n'a pas pu envoyer le lien de confirmation à l'e-mail fourni. Veuillez réessayer plus tard ou contacter le support.");
                }

                return new SignInResultDto { Successful = false, IsNotAllowed = true };
            }
            else
            {
                await userManager.AddLoginAsync(existingUser, info);
                var signInResult = await signInManager.ExternalLoginSignInAsync(
                    info.LoginProvider,
                    info.ProviderKey,
                    isPersistent: false,
                    bypassTwoFactor: true);

                if (signInResult.Succeeded)
                {
                    return new SignInResultDto
                    {
                        Successful = signInResult.Succeeded,
                        TokenPair = await jwtAuthenticationService.GenerateTokens(
                            existingUser.Id,
                            await userManager.GetClaimsAsync(existingUser),
                            DateTime.Now)
                    };
                }

                return new SignInResultDto
                {
                    Successful = signInResult.Succeeded,
                    IsNotAllowed = signInResult.IsNotAllowed,
                    IsLockedOut = signInResult.IsLockedOut,
                    LockoutEndDate = signInResult.IsLockedOut ? await userManager.GetLockoutEndDateAsync(existingUser) : null,
                    IsEmailOrPasswordIncorrect = !(signInResult.IsNotAllowed || signInResult.IsLockedOut || signInResult.RequiresTwoFactor),
                };
            }
        }

        public async Task ConfirmExternalProviderAssociation(ConfirmExternalProviderAssociationDto info)
        {
            var user = await userManager.FindByEmailAsync(info.UserEmail);
            var confirmationResult = await userManager.ConfirmEmailAsync(user, info.ConfirmationToken);
            if (!confirmationResult.Succeeded)
            {
                throw new ApplicationException($"L'association de votre compte {info.ProviderDisplayName} à échoué. Veuillez réessayer.");
            }

            var newLoginresult = await userManager.AddLoginAsync(
                user,
                new ExternalLoginInfo
                (
                    null,
                    info.LoginProvider,
                    info.ProviderKey,
                    info.ProviderDisplayName
                ));

            if (!newLoginresult.Succeeded)
            {
                throw new ApplicationException($"L'association de votre compte {info.ProviderDisplayName} à échoué. Veuillez réessayer.");
            }

            var signInResult = await signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true
            );
        }
    }
}
