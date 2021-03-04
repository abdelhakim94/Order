using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Shared.Dto.Users;
using Order.Server.Services;
using Order.Shared.Security;
using Order.Server.Dto.Users;

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
        public async Task<SignUpResultDto> SignUp([FromBody] UserSignUpDto userInfo)
        {
            return await userService.SignUp(userInfo, Url, Request.Scheme);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] EmailConfirmationDto confirmation)
        {
            try
            {
                await userService.ConfirmEmail(confirmation);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
            return Redirect("/");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<SignInResultDto> SignIn([FromBody] UserSignInDto userInfo)
        {
            // SignInManager adds cookies to the response when it
            // successfully authenticates the user. We rely on JWT
            // so we delete the cookie.
            var result = await userService.SignIn(userInfo);
            Response.Cookies.Delete(".AspNetCore.Identity.Application");
            return result;
        }

        [HttpGet]
        public new async Task SignOut()
        {
            await userService.SignOut(User.GetUserId());
        }

        [HttpPost]
        public async Task<ActionResult<TokenPairDto>> RefreshTokens([FromBody] string userRefreshToken)
        {
            try
            {
                return Ok(await userService.RefreshTokens(userRefreshToken, User.GetUserId(), User.Claims));
            }
            catch (System.Exception)
            {
                return Unauthorized();
            }
        }
    }
}
