using Microsoft.AspNetCore.Identity;

namespace Order.Server.Model
{
    public class UserClaim : IdentityUserClaim<int>
    {
        public virtual User User { get; set; }
    }
}