using System.Collections.Generic;

namespace Order.DomainModel
{
    public class Address
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipCodeCity { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<UserAddress> UsersAddress { get; set; } = new HashSet<UserAddress>();
    }
}
