using System.Collections.Generic;

namespace Order.DomainModel
{
    public class Wilaya
    {
        public int Code { get; set; }
        public int ZipCode { get; set; }
        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; set; } = new HashSet<City>();
    }
}
