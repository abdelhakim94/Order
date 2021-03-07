using System.Threading;
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
        public new async Task SignOut()
        {
            await userService.SignOut(User.GetUserId().Value);
        }
    }
}
