using System.Collections.Generic;

namespace Order.DomainModel
{
    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<MenuSection> MenuSections { get; set; }
            = new HashSet<MenuSection>();

        public virtual ICollection<MenuDish> MenuDishes { get; set; }
            = new HashSet<MenuDish>();

        public virtual ICollection<CardMenu> CardsMenu { get; set; }
            = new HashSet<CardMenu>();

        public virtual ICollection<MenuOption> MenuOptions { get; set; }
            = new HashSet<MenuOption>();

        public virtual ICollection<MenuExtra> MenuExtras { get; set; }
            = new HashSet<MenuExtra>();
    }
}
