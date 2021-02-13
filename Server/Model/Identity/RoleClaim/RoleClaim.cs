using Microsoft.AspNetCore.Identity;

namespace Order.Server.Model
{
    public class RoleClaim : IdentityRoleClaim<int>
    {
        public virtual Role Role { get; set; }
    }
}
