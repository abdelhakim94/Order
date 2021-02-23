using Microsoft.AspNetCore.Identity;

namespace Order.DomainModel
{
    public class RoleClaim : IdentityRoleClaim<int>
    {
        public virtual Role Role { get; set; }
    }
}
