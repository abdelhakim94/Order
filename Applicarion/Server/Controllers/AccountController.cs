using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Order.Application.Shared.Dto.Users;
using Order.DomainModel;

namespace Order.Application.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly HttpClient httpClient;

        public AccountController(
            IConfiguration configuration,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            HttpClient httpClient)
        {
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.httpClient = httpClient;
        }

        [HttpPost]
        public async Task<SignUpResultDto> SignUp([FromBody] UserSignUpDto userInfo)
        {
            var newUser = new User
            {
                UserName = userInfo.Email,
                Email = userInfo.Email,
                FirstName = userInfo.FirstName,
                LastName = userInfo.LastName,
            };

            var result = await userManager.CreateAsync(newUser, userInfo.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                return new SignUpResultDto { Successful = false, Errors = errors };
            }

            return new SignUpResultDto { Successful = true };
        }

        [HttpGet]
        [Authorize]
        public ActionResult Load()
        {
            return Ok(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
