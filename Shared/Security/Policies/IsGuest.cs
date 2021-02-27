using Microsoft.AspNetCore.Authorization;
using Order.Shared.Security.Claims;

namespace Order.Shared.Security.Policies
{
    /// <summary>
    /// The Most basic policy in the application. Every user should be
    /// authenticated and has the <see cref="Profile.GUEST"/> profile
    /// to be able to use the application.
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
