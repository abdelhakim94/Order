using Microsoft.AspNetCore.Authorization;
using Order.Shared.Security.Claims;

namespace Order.Shared.Security.Policies
{
    /// <summary>
    /// The user has at least one of the profiles defined
    /// in <see cref="Profile"/>.
    /// </summary>
    public class HasAtLeastOneProfile
    {
        public const string Name = nameof(HasAtLeastOneProfile);

        public static AuthorizationPolicy Policy
        {
            get => new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(nameof(Profile),
                    nameof(Profile.ADMIN),
                    nameof(Profile.GUEST),
                    nameof(Profile.DELIVERY_MAN),
                    nameof(Profile.CHEF))
                .Build();
        }
    }
}
