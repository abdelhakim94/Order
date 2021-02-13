using Microsoft.AspNetCore.Identity;

namespace Order.Server.Model
{
    public class UserToken : IdentityUserToken<int>
    {
        public virtual User User { get; set; }
    }
}
