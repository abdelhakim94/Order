using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Order.Shared.Dto.Users;
using Order.Shared.Interfaces;

namespace Order.Server.Services.JwtAuthenticationService
{
    public interface IJwtAuthenticationService : IService
    {
        Task<TokenPairDto> GenerateTokens(int userId, IEnumerable<Claim> claims, DateTime now);
        Task<TokenPairDto> RefreshTokens(string refreshToken, int userId, IEnumerable<Claim> claims, DateTime now);
        Task<bool> DeleteRefreshToken(int userId);
    }
}
