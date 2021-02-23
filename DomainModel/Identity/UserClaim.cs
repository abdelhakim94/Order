using Microsoft.AspNetCore.Identity;

namespace Order.DomainModel
{
    public class UserClaim : IdentityUserClaim<int>
    {
        public virtual User User { get; set; }
    }
}