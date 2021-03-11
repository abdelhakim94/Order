using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Shared.Dto.Users;
using Order.Server.Services;
using Order.Shared.Security;
using Order.Server.Dto.Users;
using System;

namespace Order.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public Task<SignUpResultDto> SignUp([FromBody] SignUpDto userInfo)
        {
            return userService.SignUp(userInfo, Url, Request.Scheme);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] EmailConfirmationDto confirmation)
        {
            try
            {
                await userService.ConfirmEmail(confirmation, Url, Request.Scheme);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
            return Redirect("/");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<SignInResultDto> SignIn([FromBody] SignInDto userInfo)
        {
            // SignInManager adds cookies to the response when it
            // successfully authenticates the user. We rely on JWT
            // so we delete the cookie.
            var result = await userService.SignIn(userInfo);
            Response.Cookies.Delete(".AspNetCore.Identity.Application");
            return result;
        }

        [HttpPost]
        [AllowAnonymous]
        public Task RequestResetPassword([FromBody] RequestResetPasswordDto request)
        {
            return userService.RequestResetPassword(request, Url, Request.Scheme);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RedirectToResetPassword([FromQuery] RequestResetPasswordTokenDto resetInfo)
        {
            return Redirect($"/ResetPassword/{resetInfo.UserEmail}/{resetInfo.ResetPasswordToken}");
        }

        [HttpPost]
        [AllowAnonymous]
        public Task<ResetPasswordResultDto> ResetPassword([FromBody] ResetPasswordDto resetPwInfo)
        {
            return userService.ResetPassword(resetPwInfo, Url, Request.Scheme);
        }

        [HttpPost]
        public async Task<ActionResult<TokenPairDto>> RefreshTokens([FromBody] RefreshTokensDto refreshInfo)
        {
            try
            {
                return Ok(await userService.RefreshTokens(refreshInfo.RefreshToken, User.GetUserId().Value, User.Claims));
            }
            catch (System.Exception)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        public new Task SignOut()
        {
            return userService.SignOut(User.GetUserId().Value);
        }

        // ====================================== External identity providers =========================================//

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalProviderSignIn(ExternalProviderSignInDto provider)
        {
            var redirectUrl = Url.Action("ExternalProviderSignInCallback", "User");
            var properties = userService.ConfigureSignInWithExternalProvider(provider.Provider, redirectUrl);
            return new ChallengeResult(provider.Provider, properties);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ExternalProviderSignInCallback(string returnUrl = null, string remoteError = null)
        {
            Console.WriteLine("Callback called successfully");
            Console.WriteLine(returnUrl);
            Console.WriteLine(remoteError);
            return Ok();
            // var info = await signInManager.GetExternalLoginInfoAsync();
            // if (info == null)
            // {
            //     return RedirectToPage("./", new { ReturnUrl = returnUrl });
            // }

            // // Sign in the user with this external login provider if the user already has a login.
            // var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
            //     isPersistent: false, bypassTwoFactor: true);
            // if (result.Succeeded)
            // {
            //     return LocalRedirect(returnUrl);
            // }

            // var userEmail = info.Principal.FindFirstValue(ClaimTypes.Email);

            // if (string.IsNullOrEmpty(userEmail))
            // {
            //     return LocalRedirect(
            //         $"{returnUrl}?message=Email scope access is required to add {info.ProviderDisplayName} provider&type=danger");
            // }

            // var userDb = await userManager.FindByEmailAsync(userEmail);

            // if (userDb != null)
            // {
            //     // RULE #5
            //     if (!userDb.EmailConfirmed)
            //     {
            //         var token = await userManager.GenerateEmailConfirmationTokenAsync(userDb);

            //         var callbackUrl = Url.Action("ConfirmExternalProvider", "Account",
            //             values: new
            //             {
            //                 userId = userDb.Id,
            //                 code = token,
            //                 loginProvider = info.LoginProvider,
            //                 providerDisplayName = info.LoginProvider,
            //                 providerKey = info.ProviderKey
            //             },
            //             protocol: Request.Scheme);

            //         await emailSender.SendEmailAsync(userDb.Email, $"Confirm {info.ProviderDisplayName} external login",
            //             $"Please confirm association of your {info.ProviderDisplayName} account by clicking <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>here</a>.");

            //         return LocalRedirect(
            //             $"{returnUrl}?message=External account association with {info.ProviderDisplayName} is pending.Please check your email");
            //     }

            //     // Add the external provider
            //     await userManager.AddLoginAsync(userDb, info);

            //     await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
            //        isPersistent: false, bypassTwoFactor: true);

            //     return LocalRedirect(
            //         $"{returnUrl}?message={info.ProviderDisplayName} has been added successfully");
            // }

            // return LocalRedirect($"/register?associate={userEmail}&loginProvider={info.LoginProvider}&providerDisplayName={info.ProviderDisplayName}&providerKey={info.ProviderKey}");
        }
    }
}
