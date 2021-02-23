using Microsoft.AspNetCore.Identity;

namespace Order.DomainModel
{
    public class UserLogin : IdentityUserLogin<int>
    {
        public virtual User User { get; set; }
    }
}
