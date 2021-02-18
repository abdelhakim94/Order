using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Order.Server.Model;
using Order.Shared.Dto.Users;

namespace Order.Server.Hubs
{
    public class AccountHub : Hub
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AccountHub(
            IConfiguration configuration,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<SignUpResultDto> SignUp(UserSignUpDto user)
        {
            var newUser = new User { UserName = user.Email, Email = user.Email };
            var result = await userManager.CreateAsync(newUser, user.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                return new SignUpResultDto { Successful = false, Errors = errors };
            }
            return new SignUpResultDto { Successful = true };
        }

        public async Task<SignInResultDto> SignIn(UserSignInDto user)
        {
            var result = await signInManager.PasswordSignInAsync(user.Email, user.Password, false, false);

            if (!result.Succeeded)
                return new SignInResultDto { Successful = false, Error = "Username and password are invalid." };

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
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

            return new SignInResultDto { Successful = true, Token = token.ToString() };
        }
    }
}
