using System.Collections.Generic;

namespace Order.DomainModel
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<DishCategory> DishCategories { get; set; }
             = new HashSet<DishCategory>();

        public virtual ICollection<DishSection> DishSections { get; set; }
            = new HashSet<DishSection>();

        public virtual ICollection<DishOption> DishOptions { get; set; }
            = new HashSet<DishOption>();

        public virtual ICollection<DishExtra> DishExtras { get; set; }
            = new HashSet<DishExtra>();
    }
}
