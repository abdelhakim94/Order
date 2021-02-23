using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Order.DomainModel;
using System.Security.Claims;
using IdentityModel;

namespace IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public ProfileService(
            IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            this.userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string sub = context.Subject.GetSubjectId();
            var user = await userManager.FindByIdAsync(sub);
            var userClaims = await userClaimsPrincipalFactory.CreateAsync(user);

            var claims = userClaims.Claims.ToList();
            // claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            if (userManager.SupportsUserRole)
            {
                var roles = await userManager.GetRolesAsync(user);
                foreach (var roleName in roles)
                {
                    claims.Add(new Claim(JwtClaimTypes.Role, roleName));
                    if (roleManager.SupportsRoleClaims)
                    {
                        var role = await roleManager.FindByNameAsync(roleName);
                        if (role is not null)
                        {
                            claims.AddRange(await roleManager.GetClaimsAsync(role));
                        }
                    }
                }
            }

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            string sub = context.Subject.GetSubjectId();
            var user = await userManager.FindByIdAsync(sub);
            context.IsActive = user is not null;
        }
    }
}
