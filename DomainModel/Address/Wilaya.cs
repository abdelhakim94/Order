using System.Collections.Generic;

namespace Order.DomainModel
{
    public class Wilaya
    {
        public int Id { get; set; }
        public string ZipCode { get; set; }
        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; set; } = new HashSet<City>();
    }
}
