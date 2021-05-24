using System.Collections.Generic;

namespace Order.DomainModel
{
    public class Extra
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public ICollection<DishExtra> DishesExtra { get; set; }
            = new HashSet<DishExtra>();

        public ICollection<MenuExtra> MenusExtra { get; set; }
            = new HashSet<MenuExtra>();
    }
}
