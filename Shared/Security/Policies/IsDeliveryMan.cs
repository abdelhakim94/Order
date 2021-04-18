using Microsoft.AspNetCore.Authorization;
using Order.Shared.Security.Claims;

namespace Order.Shared.Security.Policies
{
    /// <summary>
    /// The user has the <see cref="Profile.DELIVERY_MAN"/> profile
    /// </summary>
    public class IsDeliveryMan
    {
        public const string Name = nameof(IsDeliveryMan);

        public static AuthorizationPolicy Policy
        {
            get => new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(nameof(Profile), nameof(Profile.DELIVERY_MAN))
                .Build();
        }
    }
}
