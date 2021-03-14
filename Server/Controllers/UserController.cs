using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Shared.Dto.Users;
using Order.Server.Services;
using Order.Shared.Security;
using Order.Server.Dto.Users;
using Order.Server.Exceptions;

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
            return userService.RequestResetPassword(request, p => Url.Action(
                "RedirectToResetPassword",
                "User",
                p,
                Request.Scheme)
            );
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
            return userService.ResetPassword(resetPwInfo, p => Url.Action(
                "RedirectToResetPassword",
                "User",
                p,
                Request.Scheme)
            );
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
        // https://chsakell.com/2019/07/28/asp-net-core-identity-series-external-provider-authentication-registration-strategy/

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
        public async Task<SignInResultDto> ExternalProviderSignInCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError is not null)
            {
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
                return signInResult;
            }

            var userEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                throw new UnauthorizedException($"Votre email n'a pas pu être recupéré auprés de {info.LoginProvider}");
            }

            return await userService.HandleFirstExternalSignIn(
                userEmail,
                info,
                p => Url.Action("ConfirmExternalProviderAssociation", "User", p, Request.Scheme)
            );
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmExternalProviderAssociation([FromQuery] ConfirmExternalProviderAssociationDto info)
        {
            await userService.ConfirmExternalProviderAssociation(info);
            return Redirect("/");
        }
    }
}
