using System.Collections.Generic;

namespace Order.DomainModel
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public int IdWilaya { get; set; }

        public virtual Wilaya Wilaya { get; set; }
        public virtual ICollection<Address> Addresses { get; set; } = new HashSet<Address>();
    }
}
