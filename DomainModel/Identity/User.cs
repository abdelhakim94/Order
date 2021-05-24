using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Order.DomainModel
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }

        public virtual UserRefreshToken RefreshToken { get; set; }

        public virtual ICollection<UserClaim> Claims { get; set; }
        public virtual ICollection<UserLogin> Logins { get; set; }
        public virtual ICollection<UserToken> Tokens { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserAddress> UserAddresses { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
        public virtual ICollection<Card> Cards { get; set; }

        public User()
        {
            Claims = new HashSet<UserClaim>();
            Logins = new HashSet<UserLogin>();
            Tokens = new HashSet<UserToken>();
            UserRoles = new HashSet<UserRole>();
            UserAddresses = new HashSet<UserAddress>();
            UserProfiles = new HashSet<UserProfile>();
            Cards = new HashSet<Card>();
        }
    }
}
