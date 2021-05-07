using Microsoft.AspNetCore.Authorization;
using Order.Shared.Security.Claims;

namespace Order.Shared.Security.Policies
{
    /// <summary>
    /// The user has the <see cref="Profile.CHEF"/> profile
    /// </summary>
    public class IsChef
    {
        public const string Name = nameof(IsChef);

        public static AuthorizationPolicy Policy
        {
            get => new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(nameof(Profile), nameof(Profile.CHEF))
                .Build();
        }
    }
}
