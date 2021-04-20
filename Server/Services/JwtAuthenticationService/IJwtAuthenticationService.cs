using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Order.Shared.Dto.Account;
using Order.Shared.Contracts;

namespace Order.Server.Services.JwtAuthenticationService
{
    public interface IJwtAuthenticationService : IScopedService
    {
        Task<TokenPairDto> GenerateTokens(int userId, IEnumerable<Claim> claims, DateTime now);
        Task<TokenPairDto> RefreshTokens(string refreshToken, int userId, IEnumerable<Claim> claims, DateTime now);
        Task<bool> DeleteRefreshToken(int userId);
    }
}
