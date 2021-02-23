using Microsoft.AspNetCore.Identity;

namespace Order.DomainModel
{
    public class UserToken : IdentityUserToken<int>
    {
        public virtual User User { get; set; }
    }
}
