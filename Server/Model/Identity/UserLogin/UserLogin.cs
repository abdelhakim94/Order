using Microsoft.AspNetCore.Identity;

namespace Order.Server.Model
{
    public class UserLogin : IdentityUserLogin<int>
    {
        public virtual User User { get; set; }
    }
}
