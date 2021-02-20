using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Order.Server.Model;
using Order.Shared.Dto.Users;

namespace Order.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public AccountController(
            IConfiguration configuration,
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.userManager = userManager;
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

        [HttpPost]
        public async Task<ActionResult<SignInResultDto>> SignIn([FromBody] UserSignInDto userInfo)
        {
            var result = await signInManager.PasswordSignInAsync(
                userInfo.Email,
                userInfo.Password,
                true,
                false);

            if (!result.Succeeded)
                return BadRequest(new SignInResultDto
                {
                    Successful = false,
                    Error = "Erreur lors de la creation du compte :(."
                });

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userInfo.Email),
                new Claim(ClaimTypes.Email, userInfo.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(configuration["JwtExpiryInDays"]));

            var token = new JwtSecurityToken(
                configuration["JwtIssuer"],
                configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            return Ok(new SignInResultDto
            {
                Successful = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}
