using System.Collections.Generic;

namespace Order.DomainModel
{
    public class UserAddress
    {
        public int IdUser { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipCodeCity { get; set; }

        public virtual User User { get; set; }
        public virtual Address Address { get; set; }
    }
}
