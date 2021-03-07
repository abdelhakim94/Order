using System.Linq;
using System.Security.Claims;

namespace Order.Shared.Security
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal principal)
        {
            var idClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(idClaim) || !int.TryParse(idClaim, out var _))
            {
                return default(int?);
            }

            return int.Parse(idClaim);
        }
    }
}
