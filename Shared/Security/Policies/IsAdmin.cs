using Microsoft.AspNetCore.Authorization;
using Order.Shared.Security.Claims;

namespace Order.Shared.Security.Policies
{
    /// <summary>
    /// The user has the <see cref="Profile.ADMIN"/> profile
    /// </summary>
    public class IsAdmin
    {
        public const string Name = nameof(IsAdmin);

        public static AuthorizationPolicy Policy
        {
            get => new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(nameof(Profile), nameof(Profile.ADMIN))
                .Build();
        }
    }
}
