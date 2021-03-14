using System;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using MediatR;
using Order.Server.Dto.Users;
using Order.Shared.Contracts;
using Order.Shared.Dto.Users;
using Order.Server.CQRS.User.Commands;
using Order.Server.CQRS.User.Queries;
using Order.Server.Exceptions;

namespace Order.Server.Services.JwtAuthenticationService
{
    public class JwtAuthenticationService : IJwtAuthenticationService, IService
    {
        private readonly JwtTokenConfigDto jwtTokenConfig;
        private readonly IMediator mediator;

        public JwtAuthenticationService(JwtTokenConfigDto jwtTokenConfig, IMediator mediator)
        {
            this.jwtTokenConfig = jwtTokenConfig;
            this.mediator = mediator;
        }

        public async Task<TokenPairDto> GenerateTokens(int userId, IEnumerable<Claim> claims, DateTime now)
        {
            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);

            var jwtToken = new JwtSecurityToken(
                jwtTokenConfig.Issuer,
                // The audience claim is an array that keeps appending the new values
                // thus, we avoid providing it if it's not demanded so the token won't
                // be lengthy.
                shouldAddAudienceClaim ? jwtTokenConfig.Audience : string.Empty,
                claims,
                expires: now.AddMinutes(jwtTokenConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.secret)),
                    SecurityAlgorithms.HmacSha256Signature)
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var refreshToken = GenerateRefreshTokenString();

            await mediator.Send(new SaveRefreshTokenCommand(
                userId,
                refreshToken,
                DateTime.Now.AddMinutes(jwtTokenConfig.RefreshTokenExpiration)));

            return new TokenPairDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<TokenPairDto> RefreshTokens(string refreshToken, int userId, IEnumerable<Claim> claims, DateTime now)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new BadRequestException("Refresh token can't be null");
            }

            var previousRefreshToken = await mediator.Send(new LoadRefreshTokenQuery(userId));

            if (previousRefreshToken is null || previousRefreshToken.Token != refreshToken || previousRefreshToken.ExpireAt < now)
            {
                throw new BadRequestException("Invalid refresh token");
            }

            return await GenerateTokens(userId, claims, now);
        }

        public Task<bool> DeleteRefreshToken(int userId)
        {
            return mediator.Send(new DeleteRefreshTokenCommand(userId));
        }

        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
