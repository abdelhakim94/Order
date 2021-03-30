using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Order.Shared.Dto.Users;
using Order.Server.Services;
using Order.Shared.Security;
using Order.Server.Dto.Users;
using Order.Server.Exceptions;
using Order.Shared.Dto;

namespace Order.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILogger<UserController> logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public Task<SignUpResultDto> SignUp([FromBody] SignUpDto userInfo)
        {
            return userService.SignUp(userInfo, p => Url.Action(
                "ConfirmEmail",
                "User",
                p,
                Request.Scheme
            ));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] EmailConfirmationDto confirmation)
        {
            try
            {
                await userService.ConfirmEmail(confirmation, p => Url.Action(
                    "ConfirmEmail",
                    "User",
                    p,
                    Request.Scheme
                ));
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
            return Redirect("/app/SignIn/");
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
            return userService.RequestResetPassword(request, p => Url.Action(
                "RedirectToResetPassword",
                "User",
                p,
                Request.Scheme
            ));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RedirectToResetPassword([FromQuery] RequestResetPasswordTokenDto resetInfo)
        {
            return Redirect($"/app/ResetPassword/{resetInfo.UserEmail}/{resetInfo.ResetPasswordToken}");
        }

        [HttpPost]
        [AllowAnonymous]
        public Task<ResetPasswordResultDto> ResetPassword([FromBody] ResetPasswordDto resetPwInfo)
        {
            return userService.ResetPassword(resetPwInfo, p => Url.Action(
                "RedirectToResetPassword",
                "User",
                p,
                Request.Scheme
            ));
        }

        [HttpPost]
        public async Task<ActionResult<TokenPairDto>> RefreshTokens([FromBody] ValueWrapperDto<string> refreshInfo)
        {
            try
            {
                return Ok(await userService.RefreshTokens(refreshInfo.Value, User.GetUserId().Value, User.Claims));
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
        // https://chsakell.com/2019/07/28/asp-net-core-identity-series-external-provider-authentication-registration-strategy/

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalProviderSignIn(ValueWrapperDto<string> provider)
        {
            var redirectUrl = Url.Action("ExternalProviderSignInCallback", "User");
            var properties = userService.ConfigureSignInWithExternalProvider(provider.Value, redirectUrl);
            return new ChallengeResult(provider.Value, properties);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalProviderSignInCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError is not null)
            {
                logger.LogError(remoteError);
                throw new UnauthorizedException("Nous n'avons pas réussi à vous connecter avec le fournisseur selectionné.");
            }

            var info = await userService.GetExternalLoginInfoAsync();
            if (info is null)
            {
                throw new UnauthorizedException("Nous n'avons pas réussi à vous connecter avec le fournisseur selectionné.");
            }

            var signInResult = await userService.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey);
            if (signInResult.Successful)
            {
                return Redirect($"/app/SignIn/{signInResult.TokenPair.AccessToken}/{signInResult.TokenPair.RefreshToken}");
            }

            var userEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                throw new UnauthorizedException($"Votre email n'a pas pu être recupéré auprés de {info.LoginProvider}");
            }

            var result = await userService.HandleFirstExternalSignIn(
                userEmail,
                info,
                p => Url.Action("ConfirmExternalProviderAssociation", "User", p, Request.Scheme)
            );

            return Redirect($"/app/SignIn/{result?.TokenPair?.AccessToken}/{result?.TokenPair?.RefreshToken}");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmExternalProviderAssociation([FromQuery] ConfirmExternalProviderAssociationDto info)
        {
            await userService.ConfirmExternalProviderAssociation(info);
            return Redirect("/app/SignIn");
        }
    }
}
