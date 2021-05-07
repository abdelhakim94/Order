using System.Collections.Generic;

namespace Order.DomainModel
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DishSection> DishesSection { get; set; }
            = new HashSet<DishSection>();

        public virtual ICollection<MenuSection> MenuesSection { get; set; }
            = new HashSet<MenuSection>();

        public virtual ICollection<CardSection> CardsSection { get; set; }
            = new HashSet<CardSection>();
    }
}
