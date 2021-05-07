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
using Order.Shared.Dto.Account;
using Order.Server.CQRS.Account.Commands;
using Order.Server.CQRS.Account.Queries;
using Order.Server.Middlewares;
using Order.Shared.Security;

namespace Order.Server.Services.JwtAuthenticationService
{
    public class JwtAuthenticationService : IJwtAuthenticationService, IScopedService
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

        public async Task<TokenPairDto> RefreshExpiredTokens(TokenPairDto tokens, DateTime now)
        {
            if (string.IsNullOrWhiteSpace(tokens.AccessToken) || string.IsNullOrWhiteSpace(tokens.RefreshToken))
            {
                throw new BadRequestException("Refresh token can't be null");
            }

            var claims = tokens.AccessToken.ParseClaimsFromJwt();
            var idClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(idClaim) || !int.TryParse(idClaim, out var _))
            {
                throw new BadRequestException("Invalid access token");
            }

            var userId = int.Parse(idClaim);
            var previousRefreshToken = await mediator.Send(new LoadRefreshTokenQuery(userId));
            if (previousRefreshToken is null || previousRefreshToken.Token != tokens.RefreshToken || previousRefreshToken.ExpireAt < now)
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
