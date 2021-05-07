using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Order.Shared.Dto.Account;
using Order.Server.Services;
using Order.Shared.Security;
using Order.Server.Dto.Users;
using Order.Server.Middlewares;
using Order.Shared.Dto;

namespace Order.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly ILogger<UserController> logger;

        public UserController(IAccountService accountService, ILogger<UserController> logger)
        {
            this.accountService = accountService;
            this.logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public Task<SignUpResultDto> SignUp([FromBody] SignUpDto userInfo)
        {
            return accountService.SignUp(userInfo, p => Url.Action(
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
                await accountService.ConfirmEmail(confirmation, p => Url.Action(
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
            var result = await accountService.SignIn(userInfo);
            Response.Cookies.Delete(".AspNetCore.Identity.Application");
            return result;
        }

        [HttpPost]
        [AllowAnonymous]
        public Task RequestResetPassword([FromBody] RequestResetPasswordDto request)
        {
            return accountService.RequestResetPassword(request, p => Url.Action(
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
            return accountService.ResetPassword(resetPwInfo, p => Url.Action(
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
                return Ok(await accountService.RefreshTokens(refreshInfo.Value, User.GetUserId().Value, User.Claims));
            }
            catch (System.Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<TokenPairDto>> RefreshExpiredTokens([FromBody] TokenPairDto tokens)
        {
            try
            {
                return Ok(await accountService.RefreshExpiredTokens(tokens));
            }
            catch (System.Exception)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        public new Task SignOut()
        {
            return accountService.SignOut(User.GetUserId().Value);
        }

        // ====================================== External identity providers =========================================//
        // https://chsakell.com/2019/07/28/asp-net-core-identity-series-external-provider-authentication-registration-strategy/

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalProviderSignIn(ValueWrapperDto<string> provider)
        {
            var redirectUrl = Url.Action("ExternalProviderSignInCallback", "User");
            var properties = accountService.ConfigureSignInWithExternalProvider(provider.Value, redirectUrl);
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

            var info = await accountService.GetExternalLoginInfoAsync();
            if (info is null)
            {
                throw new UnauthorizedException("Nous n'avons pas réussi à vous connecter avec le fournisseur selectionné.");
            }

            var signInResult = await accountService.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey);
            if (signInResult.Successful)
            {
                return Redirect($"/app/SignIn/{signInResult.TokenPair.AccessToken}/{signInResult.TokenPair.RefreshToken}");
            }

            var userEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                throw new UnauthorizedException($"Votre email n'a pas pu être recupéré auprés de {info.LoginProvider}");
            }

            var result = await accountService.HandleFirstExternalSignIn(
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
            await accountService.ConfirmExternalProviderAssociation(info);
            return Redirect("/app/SignIn");
        }
    }
}
