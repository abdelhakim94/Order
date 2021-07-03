using System.Collections.Generic;

namespace Order.DomainModel
{
    public class Category
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Picture { get; set; }
        public int Order { get; set; }
        public bool IsMain { get; set; }

        public virtual ICollection<DishCategory> DishesCategory { get; set; }
            = new HashSet<DishCategory>();
    }
}
