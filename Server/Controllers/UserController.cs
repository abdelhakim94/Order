using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Shared.Dto.Users;
using Order.Server.Services;
using Order.Shared.Security.Policies;
using Order.Shared.Security;

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
            return await userService.SignUp(userInfo);
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
        public async Task<ActionResult<TokenPairDto>> RefreshTokens([FromBody] TokenPairDto previousTokens)
        {
            try
            {
                return Ok(await userService.RefreshTokens(previousTokens));
            }
            catch (System.Exception)
            {
                return Redirect("/");
            }
        }
    }
}
