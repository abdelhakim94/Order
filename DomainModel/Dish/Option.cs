using System.Collections.Generic;

namespace Order.DomainModel
{
    public class Option
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DishOption> DishesOption { get; set; }
            = new HashSet<DishOption>();

        public virtual ICollection<MenuOption> MenuesOption { get; set; }
            = new HashSet<MenuOption>();
    }
}
