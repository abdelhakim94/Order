using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Order.Shared.Security
{
    public static class JwtExtensions
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(this string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadJwtToken(jwt);
            return securityToken.Claims;
        }
    }
}
