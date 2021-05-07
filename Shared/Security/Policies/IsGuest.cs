using Microsoft.AspNetCore.Authorization;
using Order.Shared.Security.Claims;

namespace Order.Shared.Security.Policies
{
    /// <summary>
    /// The user has the <see cref="Profile.GUEST"/> profile
    /// </summary>
    public class IsGuest
    {
        public const string Name = nameof(IsGuest);

        public static AuthorizationPolicy Policy
        {
            get => new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(nameof(Profile), nameof(Profile.GUEST))
                .Build();
        }
    }
}
