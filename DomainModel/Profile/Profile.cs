using System.Collections;
using System.Collections.Generic;

namespace Order.DomainModel
{
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserProfile> UsersProfile { get; set; } = new HashSet<UserProfile>();
    }
}
